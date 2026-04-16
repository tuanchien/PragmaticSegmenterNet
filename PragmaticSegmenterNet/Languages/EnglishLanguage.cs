namespace PragmaticSegmenterNet.Languages;

internal sealed class EnglishLanguage : LanguageBase
{
    private static readonly ICleanerBehaviour CleanerBehaviourInstance = new EnglishCleanerBehaviour();

    public override IReadOnlyList<string> SentenceStarters { get; } =
    [
        "A", "Being", "Did", "For", "He", "How", "However", "I", "In", "It", "Millions", "More", "She", "That", "The", "There", "They", "We", "What", "When", "Where", "Who", "Why"
    ];

    public override IReadOnlyList<string> CleanedAbbreviations { get; } = [];

    public override ICleanerBehaviour CleanerBehaviour { get; } = CleanerBehaviourInstance;

    private sealed class EnglishCleanerBehaviour : CleanerBehaviourBase
    {
        public override string OnClean(string text)
        {
            return text.Replace('`', '\'');
        }
    }
}
