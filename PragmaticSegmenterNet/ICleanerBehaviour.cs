using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet;

internal interface ICleanerBehaviour
{
    Regex NoSpaceBetweenSentencesRegex { get; }

    Rule NoSpaceBetweenSentencesRule { get; }

    string OnClean(string text);
}
