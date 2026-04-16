using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet;

internal sealed class MatchSet
{
    public IEnumerable<Match> Matches => matches;
    public int Count => matches?.Count ?? 0;

    private readonly IReadOnlyList<Match> matches;

    public MatchSet(MatchCollection matchCollection)
    {
        if (matchCollection == null)
        {
            matches = [];
            return;
        }

        var result = new List<Match>(matchCollection.Count);

        foreach (Match match in matchCollection)
        {
            result.Add(match);
        }

        matches = result;
    }

    public MatchSet(IReadOnlyList<Match> matches)
    {
        this.matches = matches;
    }
}
