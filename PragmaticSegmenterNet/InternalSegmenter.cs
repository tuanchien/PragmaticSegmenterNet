using System.Text;
using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet;

internal static class InternalSegmenter
{
    private static readonly Regex BetweenQuotesFirstRegex = new Regex(@"\s(?=\()");
    private static readonly Regex BetweenQuotesSecondRegex = new Regex(@"(?<=\))\s");
    private static readonly Regex ShortSegmentRegex = new Regex(@"\A[a-zA-Z]*\Z");
    private static readonly Regex ConsecutiveUnderscoreRegex = new Regex(@"_{3,}");

    public static IReadOnlyList<string> Segment(string text, ILanguage language)
    {
        text = Preprocess(text, language);

        // Phase 4: Find all sentence boundaries at once (indices only)
        var matches = language.SentenceBoundaryRegex.Matches(text);

        // Phase 5: Materialize output strings from boundary indices
        var results = new List<string>(matches.Count);
        for (int i = 0; i < matches.Count; i++)
        {
            var match = matches[i];
            var segment = text.Substring(match.Index, match.Length);
            segment = language.SubSymbolsRules.Apply(segment);
            CollectPostProcessed(segment, language, results);
        }

        return results;
    }

    // Non-iterator facade: preprocessing runs eagerly before any values are yielded.
    public static IEnumerable<int> EnumerateEndIndexes(string text, string originalText, ILanguage language)
    {
        text = Preprocess(text, language);
        var matches = language.SentenceBoundaryRegex.Matches(text);
        return EnumerateEndIndexesCore(matches, text, originalText, language);
    }

    private static IEnumerable<int> EnumerateEndIndexesCore(MatchCollection matches, string processedText, string originalText, ILanguage language)
    {
        int searchFrom = 0;
        for (int i = 0; i < matches.Count; i++)
        {
            var match = matches[i];
            var segment = processedText.Substring(match.Index, match.Length);
            segment = language.SubSymbolsRules.Apply(segment);
            foreach (var endIndex in YieldEndIndexesFromSegment(segment, language, originalText, searchFrom))
            {
                searchFrom = endIndex + 1;
                yield return endIndex;
            }
        }
    }

    private static string Preprocess(string text, ILanguage language)
    {
        // Phase 1: Full-text preprocessing
        text = ReferenceSeparator.SeparateReferences(text);
        text = CheckForParenthesesBetweenQuotes(text, language);
        text = language.SingleNewLineRule.Apply(text);
        text = language.EllipsisRules.Apply(text);
        text = InsertEndMarkers(text, language);
        text = ExclamationWords.Apply(text);

        // Phase 2: BetweenPunctuationReplacer must run per-range because its
        // quote/paren regexes (e.g. [^'] , [^"\\]) match across \r boundaries.
        text = ApplyBetweenPunctuationPerRange(text, language);

        // Phase 3: Remaining rules safe for full text (word/char-level patterns)
        text = language.DoublePunctuationRules.Apply(text);
        text = language.QuestionMarkInQuotationRule.Apply(text);
        text = language.ExclamationMarkRules.Apply(text);
        text = ListItemReplacer.ReplaceParentheses(text);
        text = language.ReplaceColonBetweenNumbersRule.Apply(text);
        text = language.ReplaceNonSentenceBoundaryCommaRule.Apply(text);

        return text;
    }

    private static string ApplyBetweenPunctuationPerRange(string text, ILanguage language)
    {
        // Fast path: no \r boundaries means single range = full text
        if (text.IndexOf('\r') < 0)
        {
            return language.BetweenPunctuationReplacer.Replace(text);
        }

        var sb = new StringBuilder(text.Length);
        int rangeStart = 0;

        for (int i = 0; i <= text.Length; i++)
        {
            if (i < text.Length && text[i] != '\r')
            {
                continue;
            }

            if (i > rangeStart)
            {
                var range = text.Substring(rangeStart, i - rangeStart);
                range = language.BetweenPunctuationReplacer.Replace(range);
                sb.Append(range);
            }

            if (i < text.Length)
            {
                sb.Append('\r');
            }

            rangeStart = i + 1;
        }

        return sb.ToString();
    }

    private static string InsertEndMarkers(string text, ILanguage language)
    {
        // First pass: check if any range needs a ȸ marker
        bool needsMarker = false;
        int rangeStart = 0;

        for (int i = 0; i <= text.Length; i++)
        {
            if (i < text.Length && text[i] != '\r')
            {
                continue;
            }

            if (i > rangeStart && !RangeEndsWithPunctuation(text, rangeStart, i, language))
            {
                needsMarker = true;
                break;
            }

            rangeStart = i + 1;
        }

        if (!needsMarker)
        {
            return text;
        }

        // Second pass: build text with ȸ markers inserted
        var sb = new StringBuilder(text.Length + 16);
        rangeStart = 0;

        for (int i = 0; i <= text.Length; i++)
        {
            if (i < text.Length && text[i] != '\r')
            {
                continue;
            }

            if (i > rangeStart)
            {
                sb.Append(text, rangeStart, i - rangeStart);
                if (!RangeEndsWithPunctuation(text, rangeStart, i, language))
                {
                    sb.Append('ȸ');
                }
            }

            if (i < text.Length)
            {
                sb.Append('\r');
            }

            rangeStart = i + 1;
        }

        return sb.ToString();
    }

    private static bool RangeEndsWithPunctuation(string text, int start, int end, ILanguage language)
    {
        if (end <= start)
        {
            return false;
        }

        var lastChar = text[end - 1];
        var punctuations = language.Punctuations;

        for (int i = 0; i < punctuations.Count; i++)
        {
            var p = punctuations[i];
            if (p.Length == 1 && p[0] == lastChar)
            {
                return true;
            }
        }

        return false;
    }

    private static void CollectPostProcessed(string segment, ILanguage language, List<string> output)
    {
        if (segment.Length <= 2)
        {
            AddIfNotEmpty(segment, output);
            return;
        }

        var postParts = PostProcessSegment(segment, language);
        if (postParts.Length == 0)
        {
            return;
        }

        for (int p = 0; p < postParts.Length; p++)
        {
            var final = language.SubSingleQuoteRule.Apply(postParts[p]);
            AddIfNotEmpty(final, output);
        }
    }

    private static void AddIfNotEmpty(string segment, List<string> output)
    {
        if (!string.IsNullOrWhiteSpace(segment))
        {
            output.Add(RevertRegexGroupReplacement(segment));
        }
    }

    private static IEnumerable<int> YieldEndIndexesFromSegment(string segment, ILanguage language, string originalText, int searchFrom)
    {
        if (segment.Length <= 2)
        {
            int idx = FindEndIndex(segment, originalText, searchFrom);
            if (idx >= 0) yield return idx;
            yield break;
        }

        if (HasConsecutiveUnderscore(segment)) yield break;

        segment = language.ReinsertEllipsisRules.Apply(segment);
        segment = language.ExtraWhiteSpaceRule.Apply(segment);

        if (language.QuotationAtEndOfSentenceRegex.IsMatch(segment))
        {
            var parts = language.SplitSpaceQuotationAtEndOfSentenceRegex.Split(segment);
            for (int p = 0; p < parts.Length; p++)
            {
                var final = language.SubSingleQuoteRule.Apply(parts[p]);
                int idx = FindEndIndex(final, originalText, searchFrom);
                if (idx >= 0)
                {
                    searchFrom = idx + 1;
                    yield return idx;
                }
            }
            yield break;
        }

        segment = segment.Replace("\n", string.Empty).Trim();
        segment = language.SubSingleQuoteRule.Apply(segment);
        int endIdx = FindEndIndex(segment, originalText, searchFrom);
        if (endIdx >= 0) yield return endIdx;
    }

    private static int FindEndIndex(string segment, string originalText, int searchFrom)
    {
        segment = RevertRegexGroupReplacement(segment);
        if (string.IsNullOrWhiteSpace(segment)) return -1;
        int pos = originalText.IndexOf(segment, searchFrom, StringComparison.Ordinal);
        if (pos < 0) return -1;
        return pos + segment.Length - 1;
    }

    private static string RevertRegexGroupReplacement(string text)
    {
        return text.Replace("&☃", "$");
    }

    private static string CheckForParenthesesBetweenQuotes(string text, ILanguage language)
    {
        if (!language.ParenthesesBetweenDoubleQuotesRegex.IsMatch(text))
        {
            return text;
        }

        text = language.ParenthesesBetweenDoubleQuotesRegex.Replace(text, match =>
        {
            var withNewline = BetweenQuotesFirstRegex.Replace(match.Value, "\r");
            var result = BetweenQuotesSecondRegex.Replace(withNewline, "\r");

            return result;
        });

        return text;
    }

    private static string[] PostProcessSegment(string segment, ILanguage language)
    {
        if (segment.Length < 2 && ShortSegmentRegex.IsMatch(segment))
        {
            return [segment];
        }

        if (segment.Length < 2 || HasConsecutiveUnderscore(segment))
        {
            return [];
        }

        segment = language.ReinsertEllipsisRules.Apply(segment);
        segment = language.ExtraWhiteSpaceRule.Apply(segment);

        if (language.QuotationAtEndOfSentenceRegex.IsMatch(segment))
        {
            return language.SplitSpaceQuotationAtEndOfSentenceRegex.Split(segment);
        }

        segment = segment.Replace("\n", string.Empty).Trim();

        return [segment];
    }

    private static bool HasConsecutiveUnderscore(string text)
    {
        var replaced = ConsecutiveUnderscoreRegex.Replace(text, string.Empty);

        return replaced.Length == 0;
    }
}
