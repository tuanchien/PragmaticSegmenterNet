using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet.Languages;

internal sealed class KazakhLanguage : LanguageBase
{
    private static readonly IReadOnlyList<string> AbbrevationStore =
    [
        "afp", "anp", "atp", "bae", "bg", "bp", "cam", "cctv", "cd", "cez", "cgi", "cnpc", "farc", "fbi", "eiti", "epo", "er", "gp", "gps", "has", "hiv", "hrh", "http", "icu", "idf", "imd", "ime", "icu", "idf", "ip", "iso", "kaz", "kpo", "kpa",
        "kz", "kz", "mri", "nasa", "nba", "nbc", "nds", "ohl", "omlt", "ppm", "pda", "pkk", "psm", "psp", "raf", "rss", "rtl", "sas", "sme", "sms", "tnt", "udf", "uefa", "usb", "utc", "x", "zdf", "\u04d9\u049b\u0431\u043a", "\u04d9\u049b\u0431\u043a", "\u0430\u0430\u049b", "\u0430\u0432\u0433.", "a\u0431\u0431", "\u0430\u0435\u043a", "\u0430\u043a",
        "\u0430\u049b", "\u0430\u043a\u0446\u0438\u043e\u043d.", "\u0430\u043a\u0441\u0440", "\u0430\u049b\u0448", "\u0430\u043d\u0433\u043b", "\u0430\u04e9\u0441\u0448\u043a", "\u0430\u043f\u0440", "\u043c.", "\u0430.", "\u0440.", "\u0493.", "\u0430\u043f\u0440.", "\u0430\u0443\u043c.", "\u0430\u0446\u0430\u0442", "\u04d9\u0447", "\u0442. \u0431.", "\u0431. \u0437. \u0431.", "\u0431. \u0437. \u0431.", "\u0431. \u0437. \u0434.", "\u0431. \u0437. \u0434.", "\u0431\u0438\u0456\u043a\u0442.", "\u0431. \u0442.", "\u0431\u0438\u043e\u043b.", "\u0431\u0438\u043e\u0445\u0438\u043c", "\u0431\u04e9", "\u0431. \u044d. \u0434.", "\u0431\u0442\u0430", "\u0431\u04b1\u04b1",
        "\u0432\u0438\u0447", "\u0432\u0441\u043e\u043e\u043d\u043b", "\u0433\u0435\u043e\u0433\u0440.", "\u0433\u0435\u043e\u043b.", "\u0433\u043b\u0435\u043d\u043a\u043e\u0440", "\u0433\u044d\u0441", "\u049b\u043a", "\u043a\u043c", "\u0433", "\u043c\u043b\u043d", "\u043c\u043b\u0440\u0434", "\u0442", "\u0493. \u0441.", "\u0493.", "\u049b.", "\u0493.", "\u0434\u0435\u043a.", "\u0434\u043d\u049b", "\u0434\u0441\u04b1", "\u0435\u0430\u049b\u043a", "\u0435\u049b\u044b\u04b1", "\u0435\u043c\u0431\u0456\u043c\u04b1\u043d\u0430\u0439\u0433\u0430\u0437", "\u0435\u043e", "\u0435\u0443\u0440\u0430\u0437\u044d\u049b", "\u0435\u0443\u0440\u043e\u043e\u0434\u0430\u049b", "\u0435\u04b1\u0443", "\u0436.", "\u0436.", "\u0436\u0436.", "\u0436\u043e\u043e",
        "\u0436\u0456\u04e9", "\u0436\u0441\u0434\u043f", "\u0436\u0448\u0441", "\u0456\u0456\u043c", "\u0438\u043d\u0442\u0430", "\u0438\u0441\u0430\u0444", "\u043a\u0430\u043c\u0430\u0437", "\u043a\u0433\u0431", "\u043a\u0435\u0443", "\u043a\u0433", "\u043a\u043c\u00b2", "\u043a\u043c\u00b2", "\u043a\u043c\u00b3", "\u043a\u043c\u00b3", "\u043a\u0438\u043c\u0435\u043f", "\u043a\u0441\u0440", "\u043a\u0441\u0440\u043e", "\u043a\u043e\u043a\u043f", "\u043a\u0445\u0434\u0440", "\u049b\u0430\u0437\u0430\u0442\u043e\u043c\u043f\u0440\u043e\u043c", "\u049b\u0430\u0437\u043a\u0441\u0440", "\u049b\u0430\u0437\u04b1\u0443", "\u049b\u0430\u0437\u043c\u04b1\u043d\u0430\u0439\u0433\u0430\u0437", "\u049b\u0430\u0437\u043f\u043e\u0448\u0442\u0430", "\u049b\u0430\u0437\u0442\u0430\u0433", "\u049b\u0430\u0437\u04b1\u0443", "\u049b\u043a\u043f", "\u049b\u043c\u0434\u0431",
        "\u049b\u0440", "\u049b\u0445\u0440", "\u043b\u0430\u0442.", "\u043c\u00b2", "\u043c\u00b2", "\u043c\u00b3", "\u043c\u00b3", "\u043c\u0430\u0433\u0430\u0442\u044d", "\u043c\u0430\u0439.", "\u043c\u0430\u043a\u0441\u0430\u043c", "\u043c\u0431", "\u043c\u0432\u0442", "\u043c\u0435\u043c\u043b", "\u043c", "\u043c\u0441\u043e\u043f", "\u043c\u0442\u043a", "\u043c\u044b\u0441.", "\u043d\u0430\u0441\u0430", "\u043d\u0430\u0442\u043e", "\u043d\u043a\u0432\u0434", "\u043d\u043e\u044f\u0431.", "\u043e\u0431\u043b.", "\u043e\u0433\u043f\u0443", "\u043e\u043a\u0442.", "\u043e\u04a3\u0442.", "\u043e\u043f\u0435\u043a", "\u043e\u0435\u0431", "\u04e9\u0437\u0435\u043d\u043c\u04b1\u043d\u0430\u0439\u0433\u0430\u0437", "\u04e9\u0444", "\u043f\u04d9\u043a", "\u043f\u0435\u0434.",
        "\u0440\u043a\u0444\u0441\u0440", "\u0440\u043d\u049b", "\u0440\u0441\u0444\u0441\u0440", "\u0440\u0444", "\u0441\u0432\u0441", "\u0441\u0432\u0443", "\u0441\u0434\u0443", "\u0441\u0435\u0441", "\u0441\u0435\u043d\u0442.", "\u0441\u043c", "\u0441\u043d\u043f\u0441", "\u0441\u043e\u043b\u0442.", "\u0441\u043e\u043b\u0442.", "\u0441\u043e\u043e\u043d\u043e", "\u0441\u0441\u0440\u043e", "\u0441\u0441\u0440", "\u0441\u0441\u0441\u0440", "\u0441\u0441\u0441", "\u0441\u044d\u0441", "\u0434\u043a", "\u0442. \u0431.", "\u0442", "\u0442\u0432", "\u0442\u0435\u0440\u0435\u04a3\u0434.", "\u0442\u0435\u0445.", "\u0442\u0436\u049b", "\u0442\u043c\u0434", "\u0442\u04e9\u043c.", "\u0442\u0440\u043b\u043d", "\u0442\u0440", "\u0442.", "\u0438.",
        "\u043c.", "\u0441.", "\u0448.", "\u0442.", "\u0442. \u0441. \u0441.", "\u0442\u044d\u0446", "\u0443\u0430\u0437", "\u0443\u0435\u0444\u0430", "\u0435\u049b\u044b\u04b1", "\u04b1\u049b\u043a", "\u04b1\u049b\u0448\u04b1", "\u0444\u0435\u0432\u0440.", "\u0444\u049b\u049b", "\u0444\u0441\u0431", "\u0445\u0438\u043c.", "\u0445\u049b\u043a\u043e", "\u0448\u04b1\u0430\u0440", "\u0448\u044b\u04b1", "\u044d\u043a\u043e\u043d.", "\u044d\u043a\u0441\u043f\u043e", "\u0446\u0442\u043f", "\u0446\u0430\u0441", "\u044f\u043d\u0432.", "dvd", "\u0436\u043a\u0442", "\u049b\u049b\u0441", "\u043a\u043c", "\u0430\u0446\u0430\u0442", "\u044e\u043d\u0435\u0441\u043a\u043e", "\u0431\u0431\u0441", "mgm",
        "\u0436\u0441\u043a", "\u0437\u043e\u043e", "\u0431\u0441\u043d", "\u04e9\u04b1\u049b", "\u043e\u0430\u0440", "\u0431\u043e\u0430\u043a", "\u044d\u04e9\u043a\u043a", "\u0445\u0442\u049b\u043e", "\u04d9\u04e9\u043a", "\u0436\u044d\u043a", "\u0445\u0434\u043e", "\u0441\u043f\u0431\u043c\u0443", "\u0430\u0444", "\u0441\u0431\u0434", "\u0430\u043c\u0442", "\u0433\u0441\u0434\u043f", "\u0433\u0441\u0431\u043f", "\u044d\u044b\u0434\u04b1", "\u043d\u04b1\u0441\u0436\u043f", "\u0448\u044b\u04b1", "\u0436\u0442\u0441\u0445", "\u0445\u0434\u043f", "\u044d\u049b\u043a", "\u0444\u043a\u049b\u049b", "\u043f\u0438\u049b", "\u04e9\u0433\u043a", "\u043c\u0431\u0444", "\u043c\u0430\u0436", "\u043a\u043e\u0442\u0430", "\u0442\u0436", "\u0443\u043a", "\u043e\u0431\u0431",
        "\u0441\u0431\u043b", "\u0436\u0445\u043b", "\u043a\u043c\u0441", "\u0431\u043c\u0442\u0440\u043a", "\u0436\u049b\u049b", "\u0431\u0445\u043e\u043e\u043e", "\u043c\u049b\u043e", "\u0440\u0436\u043c\u0431", "\u0433\u0443\u043b\u0430\u0433", "\u0436\u043a\u043e", "\u0435\u044d\u044b", "\u0435\u0430\u044d\u044b", "\u043a\u0445\u0434\u0440", "\u0440\u0444\u043a\u043f", "\u0440\u043b\u0434\u043f", "\u0445\u0432\u049b", "\u043c\u0440", "\u043c\u0442", "\u043a\u0442\u0443", "\u0440\u0442\u0436", "\u0442\u0438\u043c", "\u043c\u0435\u043c\u0434\u0443\u043c", "\u043a\u0441\u0440\u043e", "\u0442.\u0441.\u0441", "\u0441.\u0448.", "\u0448.\u0431.", "\u0431.\u0431.", "\u0440\u0443\u0431", "\u043c\u0438\u043d", "\u0430\u043a\u0430\u0434.", "\u0493.",
        "\u043c\u043c", "\u043c\u043c."
    ];

    public override IReadOnlyList<string> Abbreviations { get; } = AbbrevationStore;

    public override IReadOnlyList<string> SentenceStarters { get; } = Empty;

    public override IReadOnlyList<string> PrepositiveAbbreviations { get; } = Empty;

    public override IReadOnlyList<string> NumberAbbreviations { get; } = Empty;

    public override Regex MultiPeriodAbbreviationRegex { get; } = new Regex(@"\b\p{IsCyrillic}(?:\.\s?\p{IsCyrillic})+[.]|b[a-z](?:\.[a-z])+[.]");

    public override ICleanerBehaviour CleanerBehaviour { get; } = new KazakhCleanerBehaviour();

    public override IReadOnlyList<string> CleanedAbbreviations { get; } = Empty;

    public override IAbbreviationReplacer AbbreviationReplacer { get; }

    public KazakhLanguage()
    {
        AbbreviationReplacer = new KazakhAbbreviationReplacer(this);
    }

    private sealed class KazakhCleanerBehaviour : ICleanerBehaviour
    {
        public Regex NoSpaceBetweenSentencesRegex { get; } = new Regex(@"(?<=[a-z\p{IsCyrillic}])\.(?=[A-Z\u0401\u0410-\u042f][^\.]+)");
        public Rule NoSpaceBetweenSentencesRule { get; }

        public KazakhCleanerBehaviour()
        {
            NoSpaceBetweenSentencesRule = new Rule(NoSpaceBetweenSentencesRegex, ". ");
        }

        public string OnClean(string text)
        {
            return text;
        }
    }

    private sealed class KazakhAbbreviationReplacer : AbbreviationReplacerBase
    {
        private static readonly Rule SingleUpperCaseCyrillicLetterAtStartOfLineRule = new Rule(@"(?<=^[\u0410-\u042f\u0401])\.(?=\s)", "∯");
        private static readonly Rule SingleUpperCaseCyrillicLetterRule = new Rule(@"(?<=\s[\u0410-\u042f\u0401])\.(?=\s)", "∯");

        public KazakhAbbreviationReplacer(ILanguage language) : base(language)
        {
        }

        public override string Replace(string text)
        {
            var result = base.Replace(text);
            result = SingleUpperCaseCyrillicLetterAtStartOfLineRule.Apply(result);
            result = SingleUpperCaseCyrillicLetterRule.Apply(result);

            return result;
        }
    }
}
