namespace PragmaticSegmenterNet.Languages.Common;

internal static class ExclamationMarkRules
{
    public static readonly IReadOnlyList<Rule> All =
    [
        new Rule(@"\!(?=(\'|\""))", "&ᓴ&"),
        new Rule(@"\!(?=\,\s[a-z])", "&ᓴ&"),
        new Rule(@"\!(?=\s[a-z])", "&ᓴ&")
    ];
}
