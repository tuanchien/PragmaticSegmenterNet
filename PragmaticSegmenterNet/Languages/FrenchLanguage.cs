namespace PragmaticSegmenterNet.Languages;

internal sealed class FrenchLanguage : LanguageBase
{
    private static readonly IReadOnlyList<string> AbbreviationsStore =
    [
        "a.c.n", "a.m", "al", "ann", "apr", "art", "auj", "av",
        "b.p", "boul",
        "c.-\u00e0-d", "c.n", "c.n.s", "c.p.i", "c.q.f.d", "c.s", "ca", "cf", "ch.-l", "chap", "co", "co", "contr",
        "dir",
        "e.g", "e.v", "env", "etc", "ex",
        "fasc", "fig", "fr", "f\u00e9m",
        "hab",
        "i.e", "ibid", "id", "inf",
        "l.d", "lib", "ll.aa", "ll.aa.ii", "ll.aa.rr", "ll.aa.ss", "ll.ee", "ll.mm", "ll.mm.ii.rr", "loc.cit", "ltd", "ltd",
        "masc", "mm", "ms",
        "n.b", "n.d", "n.d.a", "n.d.l.r", "n.d.t", "n.p.a.i", "n.s", "n/r\u00e9f", "nn.ss",
        "p.c.c", "p.ex", "p.j", "p.s", "pl", "pp",
        "r.-v", "r.a.s", "r.i.p", "r.p",
        "s.a", "s.a.i", "s.a.r", "s.a.s", "s.e", "s.m", "s.m.i.r", "s.s", "sec", "sect", "sing", "sq", "sqq", "ss", "suiv", "sup", "suppl",
        "t.s.v.p", "t\u00e9l",
        "vb", "vol", "vs",
        "x.o",
        "z.i",
        "\u00e9d"
    ];

    public override IReadOnlyList<string> Abbreviations { get; } = AbbreviationsStore;

    public override IReadOnlyList<string> PrepositiveAbbreviations { get; } = Empty;

    public override IReadOnlyList<string> NumberAbbreviations { get; } = Empty;

    public override IReadOnlyList<string> SentenceStarters { get; } = Empty;
}
