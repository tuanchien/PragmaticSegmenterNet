namespace PragmaticSegmenterNet.Languages.Common;

internal static class ReinsertEllipsisRules
{
    public static readonly IReadOnlyList<Rule> All =
    [
        // SubThreeConsecutivePeriod
        new Rule("ƪ", "..."),
        // SubThreeSpacePeriod
        new Rule("♟", " . . . "),
        // SubFourSpacePeriod
        new Rule("♝", ". . . ."),
        // SubTwoConsecutivePeriod
        new Rule("☏", ".."),
        // SubOnePeriod
        new Rule("∮", ".")
    ];
}
