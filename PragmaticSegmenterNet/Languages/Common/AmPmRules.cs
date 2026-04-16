namespace PragmaticSegmenterNet.Languages.Common;

internal static class AmPmRules
{
    public static IReadOnlyList<Rule> All =
    [
        // uppercase P.M.
        new Rule(@"(?<=P∯M)∯(?=\s[A-Z])", "."),
        // uppercase A.M.
        new Rule(@"(?<=A∯M)∯(?=\s[A-Z])", "."),
        // lowercase P.M.
        new Rule(@"(?<=p∯m)∯(?=\s[A-Z])", "."),
        // lowercase A.M.
        new Rule(@"(?<=a∯m)∯(?=\s[A-Z])", ".")
    ];
}
