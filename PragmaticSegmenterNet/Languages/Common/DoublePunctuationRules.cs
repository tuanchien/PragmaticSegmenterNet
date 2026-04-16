namespace PragmaticSegmenterNet.Languages.Common;

internal static class DoublePunctuationRules
{
    public static readonly IReadOnlyList<Rule> All =
    [
        new Rule(@"\?!", "☉"),
        new Rule(@"!\?", "☈"),
        new Rule(@"\?\?", "☇"),
        new Rule(@"!!", "☄")
    ];
}
