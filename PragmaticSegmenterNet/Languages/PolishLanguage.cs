namespace PragmaticSegmenterNet.Languages;

internal sealed class PolishLanguage : LanguageBase
{
    private static readonly IReadOnlyList<string> AbbreviationStore =
    [
        "ags", "alb", "ang", "aor", "awest", "ba\u0142t", "bojkow", "bret", "brus", "bs\u0142", "bu\u0142g", "c.b.d.o", "c.b.d.u", "celt", "chorw", "cs", "czakaw", "czerw", "czes", "d\u0142u\u017c", "dniem", "dor", "dubrow", "du\u0144", "ekaw", "fi\u0144", "franc", "gal", "germ",
        "g\u0142u\u017c", "gniem", "goc", "gr", "grudz", "hebr", "het", "hol", "I cont", "ie", "ikaw", "ira\u0144", "irl", "islandz", "itd", "itd.", "itp", "jekaw", "kajkaw", "kasz", "kirg", "kwiec", "\u0142ac", "lip", "listop", "lit", "\u0142ot", "lp", "maced", "mar",
        "m\u0142pol", "moraw", "n.e", "nb.", "ngr", "niem", "nord", "norw", "np", "np.", "ok.", "orm", "oset", "osk", "p.n", "p.n.e", "p.o", "pazdz", "pers", "pie", "pod red.", "podhal", "pol", "po\u0142ab", "port", "prekm", "pskow", "ps\u0142", "R cont",
        "rez", "rom", "rozdz.", "rum", "rus", "rys.", "sas", "sch", "scs", "serb", "sierp", "\u015bl", "s\u0142a", "s\u0142e", "s\u0142i", "s\u0142ow", "sp. z o.o", "\u015brdniem", "\u015brgniem", "\u015brirl", "stbu\u0142g", "stind", "stpol", "stpr", "str.", "strus", "stwniem", "stycz",
        "sztokaw", "szwedz", "t.", "tj.", "t\u0142um.", "toch", "tur", "tzn", "ukr", "ul", "umbr", "wed", "w\u0119g", "wlkpol", "w\u0142os", "wrzes", "wyd.", "zakarp"
    ];

    public override IReadOnlyList<string> Abbreviations { get; } = AbbreviationStore;

    public override IReadOnlyList<string> PrepositiveAbbreviations { get; } = Empty;

    public override IReadOnlyList<string> NumberAbbreviations { get; } = Empty;

    public override IReadOnlyList<string> SentenceStarters { get; } = Empty;
}
