using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet.Languages;

internal sealed class RussianLanguage : LanguageBase
{
    private static readonly IReadOnlyList<string> AbbreviationStore =
    [
        "y", "y.e", "\u0430", "\u0430\u0432\u0442", "\u0430\u0434\u043c.-\u0442\u0435\u0440\u0440", "\u0430\u043a\u0430\u0434", "\u0432", "\u0432\u0432", "\u0432\u043a\u0437", "\u0432\u043e\u0441\u0442.-\u0435\u0432\u0440\u043e\u043f", "\u0433", "\u0433\u0433", "\u0433\u043e\u0441", "\u0433\u0440", "\u0434", "\u0434\u0435\u043f", "\u0434\u0438\u0441\u0441", "\u0434\u043e\u043b", "\u0434\u043e\u043b\u043b", "\u0435\u0436\u0435\u0434\u043d", "\u0436", "\u0436\u0435\u043d", "\u0437", "\u0437\u0430\u043f", "\u0437\u0430\u043f.-\u0435\u0432\u0440\u043e\u043f", "\u0437\u0430\u0440\u0443\u0431", "\u0438", "\u0438\u043d", "\u0438\u043d\u043e\u0441\u0442\u0440", "\u0438\u043d\u0441\u0442", "\u043a", "\u043a\u0430\u043d\u0434",
        "\u043a\u0432", "\u043a\u0433", "\u043a\u0443\u0431", "\u043b", "\u043b.h", "\u043b.\u043d", "\u043c", "\u043c\u0438\u043d", "\u043c\u043e\u0441\u043a", "\u043c\u0443\u0436", "\u043d", "\u043d\u0435\u0434", "\u043e", "\u043f", "\u043f\u0433\u0442", "\u043f\u0435\u0440", "\u043f\u043f", "\u043f\u0440", "\u043f\u0440\u043e\u0441\u043f", "\u043f\u0440\u043e\u0444", "\u0440", "\u0440\u0443\u0431", "\u0441", "\u0441\u0435\u043a", "\u0441\u043c", "\u0441\u043f\u0431", "\u0441\u0442\u0440", "\u0442", "\u0442\u0435\u043b", "\u0442\u043e\u0432", "\u0442\u0442", "\u0442\u044b\u0441", "\u0443", "\u0443.\u0435", "\u0443\u043b", "\u0444", "\u0447"
    ];

    public override IReadOnlyList<string> Abbreviations { get; } = AbbreviationStore;

    public override IReadOnlyList<string> PrepositiveAbbreviations { get; } = Empty;

    public override IReadOnlyList<string> NumberAbbreviations { get; } = Empty;

    public override IReadOnlyList<string> SentenceStarters { get; } = Empty;

    public override IAbbreviationReplacer AbbreviationReplacer { get; }

    public RussianLanguage()
    {
        AbbreviationReplacer = new RussianAbbreviationReplacer(this);
    }

    private sealed class RussianAbbreviationReplacer : AbbreviationReplacerBase
    {
        public RussianAbbreviationReplacer(ILanguage language) : base(language)
        {
        }

        protected override string ReplacePeriodInAbbreviation(string text, string abbreviation)
        {
            var trimmed = abbreviation.Trim();
            var result = Regex.Replace(text, $"(?<=\\s{trimmed})\\.", "∯");
            result = Regex.Replace(result, $"(?<=^{trimmed})\\.", "∯");

            return result;
        }
    }
}
