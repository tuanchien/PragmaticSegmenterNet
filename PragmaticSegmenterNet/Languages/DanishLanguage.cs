using System.Text;
using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet.Languages;

internal sealed class DanishLanguage : LanguageBase
{
    private static readonly IReadOnlyList<string> AbbreviationStore =
    [
        "adm", "adr", "afd", "afs", "al", "alm", "alm", "ang", "ank", "anm", "ann", "ansvh", "apr", "arr", "ass", "att", "aud", "aug", "aut", "bd", "bdt", "bet", "bhk", "bio", "biol", "bk", "bl.a", "bot", "br", "bto", "ca", "cal", "cirk", "cit",
        "co", "cpr-nr", "cvr-nr", "d.d", "d.e", "d.m", "d.s", "d.s.s", "d.y", "d.\u00e5", "d.\u00e6", "da", "dav", "dec", "def", "del", "dep", "diam", "din", "dir", "disp", "distr", "do", "dobb", "dr", "ds", "dvs", "e.b", "e.kr", "e.l", "e.o", "e.v.t",
        "eftf", "eftm", "egl", "eks", "eksam", "ekskl", "eksp", "ekspl", "el", "emer", "endv", "eng", "enk", "etc", "eur", "evt", "exam", "f", "f", "f.eks", "f.kr", "f.m", "f.n", "f.o", "f.o.m", "f.s.v", "f.t", "f.v.t", "f.\u00e5", "fa", "fakt",
        "feb", "fec", "ff", "fg", "fg", "fhv", "fig", "fl", "flg", "fm", "fm", "fmd", "forb", "foreg", "foren", "forf", "forh", "fork", "form", "forr", "fors", "forsk", "forts", "fp", "fr", "frk", "fuldm", "fuldm", "fung", "fung", "fys", "f\u00e6r",
        "g", "g.d", "g.m", "gd", "gdr", "gg", "gh", "gl", "gn", "gns", "gr", "grdl", "gross", "h.a", "h.c", "hdl", "henh", "henv", "hf", "hft", "hhv", "hort", "hosp", "hpl", "hr", "hrs", "hum", "i", "i.e", "ib", "ibid", "if", "ifm", "ill",
        "indb", "indreg", "ing", "inkl", "insp", "instr", "isl", "istf", "jan", "jf", "jfr", "jnr", "jr", "jul", "jun", "jur", "jvf", "kal", "kap", "kat", "kbh", "kem", "kgl", "kin", "kl", "kld", "km/t", "knsp", "komm", "kons", "korr", "kp",
        "kr", "kr", "kst", "kt", "ktr", "kv", "kvt", "l", "l.c", "lab", "lat", "lb", "lb.", "lb.nr", "lejl", "lgd", "lic", "lign", "lin", "ling.merc", "litt", "lok", "lrs", "ltr", "l\u00f8", "m", "m.a.o", "m.fl.st", "m.m", "m/", "ma", "mag", "maks",
        "mar", "mat", "matr.nr", "md", "mdl", "mdr", "mdtl", "med", "medd", "medflg", "medl", "merc", "mezz", "mf", "mfl", "mgl", "mhp", "mht", "mi", "mia", "mio", "ml", "mods", "modsv", "modt", "mr", "mrk", "mrs", "ms", "mul", "mv", "mvh", "n",
        "n.br", "n.f", "nat", "ned", "nedenn", "nedenst", "nederl", "nkr", "nl", "no", "nord", "nov", "nr", "nr", "nto", "nuv", "o", "o.a", "o.fl.st", "o.g", "o.h", "o.m.a", "obj", "obl", "obs", "odont", "oecon", "off", "ofl", "okt", "omg",
        "omr", "omtr", "on", "op.cit", "opg", "opl", "opr", "org", "orig", "osfr", "osv", "ovenn", "ovenst", "overs", "ovf", "oz", "p", "p.a", "p.b.v", "p.c", "p.m.v", "p.p", "p.s", "p.t", "p.v.a", "p.v.c", "par", "partc", "pass", "pct", "pd",
        "pens", "perf", "pers", "pg", "pga", "pgl", "ph", "ph.d", "pharm", "phil", "pinx", "pk", "pkt", "pl", "pluskv", "polit", "polyt", "port", "pos", "pp", "pr", "prc", "priv", "prod", "prof", "pron", "pr\u00e6d", "pr\u00e6f", "pr\u00e6p", "pr\u00e6s", "pr\u00e6t",
        "psych", "pt", "p\u00e6d", "q.e.d", "rad", "red", "ref", "reg", "regn", "rel", "rep", "repr", "rest", "rk", "russ", "s", "s.br", "s.d", "s.e", "s.f", "s.m.b.a", "s.u", "s.\u00e5", "s/", "sa", "sb", "sc", "scient", "sek", "sek", "sekr", "sem",
        "sen", "sep", "sept", "sg", "sign", "sj", "skr", "skt", "slutn", "sml", "smp", "sms", "smst", "soc", "soc", "sort", "sp", "spec", "spm", "spr", "spsk", "st", "stk", "str", "stud", "subj", "subst", "suff", "sup", "suppl", "sv", "s\u00e5k",
        "s\u00e6dv", "s\u00f8", "t", "t.h", "t.o.m", "t.v", "tab", "td", "tdl", "tdr", "techn", "tekn", "temp", "th", "ti", "tidl", "tilf", "tilh", "till", "tilsv", "tjg", "tlf", "tlgr", "to", "tr", "trp", "tv", "ty", "u", "u.p", "u.st", "u.\u00e5", "uafh",
        "ubf", "ub\u00f8j", "udb", "udbet", "udd", "udg", "uds", "ugtl", "ulin", "ult", "undt", "univ", "v.f", "var", "vb", "vbsb", "vedk", "vedl", "vedr", "vejl", "vh", "vol", "vs", "vsa", "v\u00e6r", "zool", "\u00e5rg", "\u00e5rh", "\u00e5rl", "\u00f8.f", "\u00f8v", "\u00f8vr"
    ];

    private static readonly DanishNumberRules NumberRuleStore = new DanishNumberRules();

    private static readonly IReadOnlyList<string> SentenceStarterStore =
    [
        "At", "De", "Dem", "Den", "Der", "Det", "Du", "En", "Et", "For", "F\u00e5", "Gjorde", "Han", "Hun", "Hvad", "Hvem", "Hvilke", "Hvor", "Hvordan", "Hvorfor", "Hvorledes", "Hvorn\u00e5r", "I", "Jeg", "Mange", "Vi", "V\u00e6re"
    ];

    private static readonly ICleanerBehaviour CleanerBehaviourInstance = new DanishCleanerBehaviour();

    public override IReadOnlyList<string> Abbreviations { get; } = AbbreviationStore;

    public override IReadOnlyList<string> PrepositiveAbbreviations { get; } = ["adm", "skt", "dr", "hr", "fru", "st"];

    public override IReadOnlyList<string> NumberAbbreviations { get; } = ["nr", "s"];

    public override IReadOnlyList<string> CleanedAbbreviations { get; } = Empty;

    public override IReadOnlyList<string> SentenceStarters { get; } = SentenceStarterStore;

    public override INumberRules NumberRules { get; } = NumberRuleStore;

    public override IAbbreviationReplacer AbbreviationReplacer { get; }

    public override ICleanerBehaviour CleanerBehaviour { get; } = CleanerBehaviourInstance;

    public DanishLanguage()
    {
        AbbreviationReplacer = new DanishAbbreviationReplacer(this);
    }

    private sealed class DanishNumberRules : INumberRules
    {
        private static readonly NumbersBase NumberBase = new NumbersBase();

        private static readonly Rule NumberPeriodSpaceRule =new Rule(@"(?<=\s[0-9]|\s([1-9][0-9]))\.(?=\s)", "∯");
        private static readonly Rule  NegativeNumberPeriodSpaceRule =new Rule(@"(?<=-[0-9]|-([1-9][0-9]))\.(?=\s)", "∯");

        public string Apply(string input)
        {
            var result = NumberBase.Apply(input);
            result = NumberPeriodSpaceRule.Apply(result);
            result = NegativeNumberPeriodSpaceRule.Apply(result);

            return result;
        }
    }

    private sealed class DanishAbbreviationReplacer : AbbreviationReplacerBase
    {
        private static readonly IReadOnlyList<string> EscapedPreSentenceEndAbbreviations;

        static DanishAbbreviationReplacer()
        {
            EscapedPreSentenceEndAbbreviations = new List<string>(
            [
                "U∯S∯",
                "U.S∯",
                "U∯K∯",
                "U.K∯",
                "E∯U∯",
                "E.U∯",
                "U∯S∯A∯",
                "U.S.A∯",
                "I∯",
                "s.u∯",
                "S.U∯"
            ]).Select(Regex.Escape).ToList();
        }

        public DanishAbbreviationReplacer(ILanguage language) : base(language)
        {
        }

        protected override string ReplaceAbbreviationAsSentenceBoundary(string text)
        {
            // Finds the last replaced period in an abbreviation prior to a sentence starter and replace with a period again.
            var result = text;

            // Reused to reduce collections.
            var builder = new StringBuilder();

            foreach (var word in Language.SentenceStarters)
            {
                var escaped = Regex.Escape(word);
                foreach (var preSentenceEndAbbreviation in EscapedPreSentenceEndAbbreviations)
                {
                    var pattern = $"{preSentenceEndAbbreviation}\\s{escaped}\\s";

                    result = Regex.Replace(result, pattern, x =>
                    {
                        builder.Clear();
                        var replacedLast = false;
                        var match = x.Value;

                        for (var i = match.Length - 1; i >= 0; i--)
                        {
                            if (match[i] == '∯' && !replacedLast)
                            {
                                builder.Insert(0, '.');
                                replacedLast = true;
                            }
                            else
                            {
                                builder.Insert(0, match[i]);
                            }
                        }

                        return builder.ToString();
                    });
                }
            }

            return result;
        }
    }

    private sealed class DanishCleanerBehaviour : CleanerBehaviourBase
    {
        public override string OnClean(string text)
        {
            var result = base.OnClean(text);

            result = result.Replace('`', '\'');

            return result;
        }
    }
}
