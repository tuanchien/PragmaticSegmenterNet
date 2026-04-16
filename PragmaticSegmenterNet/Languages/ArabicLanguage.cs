using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet.Languages;

internal sealed class ArabicLanguage : LanguageBase
{
    public override IReadOnlyList<string> Punctuations { get; } =
    [
        "?", "!", ":", ".", "\u061f", "\u060c"
    ];

    public override Regex SentenceBoundaryRegex { get; } = new Regex(@".*?[:\.!\?\u061f\u060c]|.*?\z|.*?$");

    public override IReadOnlyList<string> Abbreviations { get; } =
    [
        "\u0627", "\u0627. \u062f", "\u0627.\u062f", "\u0627.\u0634.\u0627", "\u0627.\u0634.\u0627", "\u0625\u0644\u062e", "\u062a.\u0628", "\u062a.\u0628", "\u062c.\u0628", "\u062c\u0645", "\u062c.\u0628", "\u062c.\u0645.\u0639", "\u062c.\u0645.\u0639", "\u0633.\u062a", "\u0633.\u062a", "\u0633\u0645", "\u0635.\u0628.", "\u0635.\u0628", "\u0643\u062c.", "\u0643\u0644\u0645.", "\u0645", "\u0645.\u0628", "\u0645.\u0628", "\u0647", "\u062f\u202a"
    ];

    public override IReadOnlyList<string> PrepositiveAbbreviations { get; } = Empty;

    public override IReadOnlyList<string> NumberAbbreviations { get; } = Empty;

    public override Rule ReplaceColonBetweenNumbersRule { get; } = new Rule(@"(?<=\d):(?=\d)", "♭");

    public override Rule ReplaceNonSentenceBoundaryCommaRule { get; } = new Rule(@"\u060c(?=\s\S+\u060c)", "♬");

    public override IReadOnlyList<string> SentenceStarters { get; } = Empty;

    public override IAbbreviationReplacer AbbreviationReplacer { get; }

    public ArabicLanguage()
    {
        AbbreviationReplacer = new ArabicAbbreviationReplacer(this);
    }

    private sealed class ArabicAbbreviationReplacer : AbbreviationReplacerBase
    {
        public ArabicAbbreviationReplacer(ILanguage language) : base(language)
        {
        }

        protected override string ScanForReplacements(string text, int index, Match match, MatchCollection characterArray)
        {
            var result = Regex.Replace(text, $"(?<={match.Value})\\.", "∯");

            return result;
        }
    }
}
