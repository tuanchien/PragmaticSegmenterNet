using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet;

internal class CleanerBehaviourBase : ICleanerBehaviour
{
    private static readonly Regex NoSpaceBetweenSentenceRegexStore = new Regex(@"(?<=[a-z])\.(?=[A-Z][^\.]+)");

    public Regex NoSpaceBetweenSentencesRegex => NoSpaceBetweenSentenceRegexStore;
    public Rule NoSpaceBetweenSentencesRule { get; } = new Rule(NoSpaceBetweenSentenceRegexStore, ". ");

    public virtual string OnClean(string text)
    {
        return text;
    }
}
