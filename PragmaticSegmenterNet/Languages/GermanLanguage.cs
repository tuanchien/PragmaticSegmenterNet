using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet.Languages;

internal sealed class GermanLanguage : LanguageBase
{
    private static readonly IReadOnlyList<string> AbbbreviationStore =
    [
        "\u00c4", "\u00e4", "adj", "adm", "adv", "art", "asst",
        "b.a", "b.s", "bart", "bldg", "brig", "bros", "bse", "buchst", "bzgl", "bzw",
        "c.-\u00e0-d", "ca", "capt", "chr", "cmdr", "co", "col", "comdr", "con", "corp", "cpl",
        "d.h", "d.j", "dergl", "dgl", "dkr", "dr",
        "ens", "etc", "ev", "evtl",
        "ff",
        "g.g.a", "g.u", "gen", "ggf", "gov",
        "hon", "hosp",
        "i.f", "i.h.v", "ii", "iii", "insp", "iv", "ix",
        "jun",
        "k.o", "kath",
        "lfd", "lt", "ltd",
        "m.e", "maj", "med", "messrs", "mio", "mlle", "mm", "mme", "mr", "mrd", "mrs", "ms", "msgr", "mwst",
        "no", "nos", "nr",
        "o.\u00e4", "op", "ord",
        "pfc", "ph", "pp", "prof", "pvt",
        "rep", "reps", "res", "rev", "rt",
        "s.p.a", "sa", "sen", "sens", "sfc", "sgt", "sog", "sogen", "spp", "sr", "st", "std", "str", "supt", "surg",
        "u.a", "u.e", "u.s.w", "u.u", "u.\u00e4", "usf", "usw",
        "v", "vgl", "vi", "vii", "viii", "vs",
        "x", "xi", "xii", "xiii", "xiv", "xix", "xv", "xvi", "xvii", "xviii", "xx",
        "z.b", "z.t", "z.z", "z.zt", "zt", "zzt",
        "univ.-prof", "o.univ.-prof", "ao.univ.prof", "ass.prof", "hon.prof", "univ.-doz", "univ.ass", "stud.ass", "projektass", "ass", "di", "dipl.-ing", "mag"
    ];

    private static readonly GermanNumberRules NumberRulesStore = new GermanNumberRules();

    public override IReadOnlyList<string> Abbreviations { get; } = AbbbreviationStore;
    public override IReadOnlyList<string> NumberAbbreviations { get; } = ["art", "ca", "no", "nos", "nr", "pp"];
    public override IReadOnlyList<string> PrepositiveAbbreviations { get; } = Empty;

    public override IReadOnlyList<string> SentenceStarters { get; } =
    [
        "Am", "Auch", "Auf",
        "Bei",
        "Da", "Das", "Der", "Die",
        "Ein", "Eine", "Es",
        "F\u00fcr",
        "Heute",
        "Ich", "Im", "In", "Ist",
        "Jetzt",
        "Mein", "Mit",
        "Nach",
        "So",
        "Und",
        "Warum", "Was", "Wenn", "Wer", "Wie", "Wir"
    ];

    public override INumberRules NumberRules { get; } = NumberRulesStore;

    public override IAbbreviationReplacer AbbreviationReplacer { get; }

    public override IBetweenPunctuationReplacer BetweenPunctuationReplacer { get; } = new GermanBetweenPunctuationReplacer();

    public GermanLanguage()
    {
        AbbreviationReplacer = new GermanAbbreviationReplacer(this);
    }

    private sealed class GermanNumberRules : INumberRules
    {
        private static readonly NumbersBase BaseNumbers = new NumbersBase();

        private static readonly Rule NumberPeriodSpaceRule = new Rule(@"(?<=\s[0-9]|\s([1-9][0-9]))\.(?=\s)", "∯");
        private static readonly Rule NegativeNumberPeriodSpaceRule = new Rule(@"(?<=-[0-9]|-([1-9][0-9]))\.(?=\s)", "∯");

        private static readonly IReadOnlyList<string> Months =
        [
            "Januar", "Februar", "M\u00e4rz", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember"
        ];

        public string Apply(string input)
        {
            var result = BaseNumbers.Apply(input);
            result = NumberPeriodSpaceRule.Apply(result);
            result = NegativeNumberPeriodSpaceRule.Apply(result);

            result = ReplacePeriodsInDates(result);

            return result;
        }

        private static string ReplacePeriodsInDates(string text)
        {
            for (var i = 0; i < Months.Count; i++)
            {
                var month = Months[i];

                var pattern = $"(?<=\\d)\\.(?=\\s*{Regex.Escape(month)})";

                text = Regex.Replace(text, pattern, "∯");
            }

            return text;
        }
    }

    private sealed class GermanAbbreviationReplacer : AbbreviationReplacerBase
    {
        private static readonly Rule SingleLowerCaseLetterRule = new Rule(@"(?<=\s[a-z])\.(?=\s)", "∯");
        private static readonly Rule SingleLowerCaseLetterAtStartOfLineRule = new Rule(@"(?<=^[a-z])\.(?=\s)", "∯");

        public GermanAbbreviationReplacer(ILanguage language) : base(language)
        {
        }

        public override string Replace(string text)
        {
            text = Language.PossessiveAbbreviationRule.Apply(text);
            text = Language.SingleLetterAbbreviationRules.Apply(text);
            text = SingleLowerCaseLetterRule.Apply(text);
            text = SingleLowerCaseLetterAtStartOfLineRule.Apply(text);

            text = SearchForAbbreviations(text);
            text = ReplaceMultiPeriodAbbreviations(text);
            text = Language.AmPmRules.Apply(text);
            text = ReplaceAbbreviationAsSentenceBoundary(text);

            return text;
        }

        protected override string ScanForReplacements(string text, int index, Match match, MatchCollection characterArray)
        {
            var result = Regex.Replace(text, $"(?<={match.Value})\\.(?=\\s)", "∯");
            return result;
        }
    }

    private sealed class GermanBetweenPunctuationReplacer : BetweenPunctuationReplacer
    {
        private static readonly Regex BetweenUnconventionalDoubleQuoteDeRegex = new Regex(",,(?>[^\u201c\\\\]+|\\\\{2}|\\\\.)*\u201c");

        private static readonly Regex SplitDoubleQuotesDeRegex = new Regex("^\u201e(?>[^\u201c\\\\]+|\\\\{2}|\\\\.)*\u201c");

        private static readonly Regex BetweenDoubleQuotesDeRegex = new Regex("\u201e(?>[^\u201c\\\\]+|\\\\{2}|\\\\.)*\u201c");


        protected override MatchSet GetTextBetweenDoubleQuotes(string text)
        {
            if (text.Contains('\u201e'))
            {
                var result = BetweenDoubleQuotesDeRegex.Matches(text);

                var matches = new List<Match>();

                foreach (Match match in result)
                {
                    matches.Add(match);
                }

                foreach (Match match in SplitDoubleQuotesDeRegex.Matches(text))
                {
                    matches.Add(match);
                }

                return new MatchSet(matches);
            }

            if (text.Contains(",,"))
            {
                var result = new MatchSet(BetweenUnconventionalDoubleQuoteDeRegex.Matches(text));

                return result;
            }

            return base.GetTextBetweenDoubleQuotes(text);
        }
    }
}
