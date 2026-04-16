using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet;

internal sealed class Rule
{
    public static readonly Rule Empty = new Rule();
    private readonly Regex? regex;

    public string Replacement { get; } = null!;

    private Rule()
    {
    }

    public Rule(string regex, string replacement)
    {
        this.regex = new Regex(regex);
        Replacement = replacement;
    }

    public Rule(Regex regex, string replacement)
    {
        this.regex = regex;
        Replacement = replacement;
    }

    public string Apply(string input)
    {
        if(input == null || regex == null)
        {
            return input!;
        }

        input = regex.Replace(input, Replacement);

        return input;
    }
}
