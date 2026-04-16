using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet;

internal static class Processor
{
    private static readonly Regex NumberedGroups = new Regex(@"\$(\d+)");

    public static IReadOnlyList<string> Process(string text, ILanguage language)
    {
        text = ApplyPreRules(text, language);
        return InternalSegmenter.Segment(text, language);
    }

    // Non-iterator facade: pre-rules run eagerly before any values are yielded.
    public static IEnumerable<int> EnumerateEndIndexes(string text, string originalText, ILanguage language)
    {
        text = ApplyPreRules(text, language);
        return InternalSegmenter.EnumerateEndIndexes(text, originalText, language);
    }

    private static string ApplyPreRules(string text, ILanguage language)
    {
        text = ReplaceRegexGroupsSyntax(text);
        text = ListItemReplacer.AddLineBreak(text);
        text = language.AbbreviationReplacer.Replace(text);
        text = language.NumberRules.Apply(text);
        text = ReplaceContinuousPunctuation(text, language);
        text = language.WithMultiplePeriodsAndEmailRule.Apply(text);
        text = language.GeoLocationRule.Apply(text);
        text = language.FileFormatRule.Apply(text);
        return text;
    }

    private static string ReplaceContinuousPunctuation(string input, ILanguage language)
    {
        input = language.ContinuousPunctuationRegex.Replace(input, x =>
        {
            var val = x.Value;
            return val.Replace("!", "&ᓴ&")
                .Replace("?", "&ᓷ&");
        });

        return input;
    }

    private static string ReplaceRegexGroupsSyntax(string input)
    {
        var result = NumberedGroups.Replace(input, @"&☃$1");

        return result;
    }
}
