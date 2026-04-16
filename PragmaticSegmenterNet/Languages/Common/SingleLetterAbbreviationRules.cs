namespace PragmaticSegmenterNet.Languages.Common;

internal static class SingleLetterAbbreviationRules
{
    public static IReadOnlyList<Rule> All =
    [
        new Rule(@"(?<=^[A-Z])\.(?=,?\s)", "∯"),
        new Rule(@"(?<=\s[A-Z])\.(?=,?\s)", "∯")
    ];
}
