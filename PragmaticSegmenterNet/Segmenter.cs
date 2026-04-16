namespace PragmaticSegmenterNet;

public static class Segmenter
{
    public static IReadOnlyList<string> Segment(string? text, Language language = Language.English, bool cleanText = true, DocumentType documentType = DocumentType.Any)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return [];
        }

        if (text.Length == 1)
        {
            return [text];
        }

        var matchingLanguage = LanguageProvider.Get(language);

        if (cleanText)
        {
            text = Cleaner.Clean(text, matchingLanguage, documentType);
        }

        var result = Processor.Process(text, matchingLanguage);

        return result;
    }

    /// <summary>
    /// Lazily yields the end index (inclusive) in the original text of each segmented sentence.
    /// Segments are located in the original text via sequential search, so indexes reflect
    /// positions in the input string. Segments that cannot be found (due to text cleaning
    /// transformations) are omitted from the result.
    /// </summary>
    public static IEnumerable<int> GetEndOfSentenceIndexes(string? text, Language language = Language.English, bool cleanText = true, DocumentType documentType = DocumentType.Any)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return [];
        }

        if (text.Length == 1)
        {
            return [0];
        }

        var originalText = text;
        var matchingLanguage = LanguageProvider.Get(language);

        if (cleanText)
        {
            text = Cleaner.Clean(text, matchingLanguage, documentType);
        }

        return Processor.EnumerateEndIndexes(text, originalText, matchingLanguage);
    }
}
