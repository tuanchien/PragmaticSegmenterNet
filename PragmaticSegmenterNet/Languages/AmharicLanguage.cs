using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet.Languages;

internal sealed class AmharicLanguage : LanguageBase
{
    public override Regex SentenceBoundaryRegex { get; } = new Regex(@".*?[፧።!\?]|.*?$");

    public override IReadOnlyList<string> Punctuations { get; } = ["።", "፧", "?", "!"];

    public override IReadOnlyList<string> SentenceStarters { get; } = Empty;
}
