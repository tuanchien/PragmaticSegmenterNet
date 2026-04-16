using Xunit;

namespace PragmaticSegmenterNet.Tests.Unit;

public class GetEndOfSentenceIndexesTests
{
    // ── helpers ────────────────────────────────────────────────────────────────

    /// <summary>
    /// Verifies that every end index is consistent with what <see cref="Segmenter.Segment"/>
    /// returns: the indexed character in the original text must be the last character of the
    /// corresponding segment string.
    /// </summary>
    private static void AssertConsistentWithSegment(string text)
    {
        var segments = Segmenter.Segment(text).ToList();
        var indexes  = Segmenter.GetEndOfSentenceIndexes(text).ToList();

        Assert.Equal(segments.Count, indexes.Count);

        for (int i = 0; i < segments.Count; i++)
        {
            var lastChar = segments[i][^1];
            Assert.Equal(lastChar, text[indexes[i]]);
        }
    }

    // ── edge cases ─────────────────────────────────────────────────────────────

    [Fact]
    public void HandlesNull()
    {
        Assert.Empty(Segmenter.GetEndOfSentenceIndexes(null));
    }

    [Fact]
    public void HandlesEmptyString()
    {
        Assert.Empty(Segmenter.GetEndOfSentenceIndexes(string.Empty));
    }

    [Fact]
    public void HandlesWhitespace()
    {
        Assert.Empty(Segmenter.GetEndOfSentenceIndexes("        "));
    }

    [Fact]
    public void HandlesNewLine()
    {
        Assert.Empty(Segmenter.GetEndOfSentenceIndexes("\n"));
    }

    [Fact]
    public void HandlesSingleChar()
    {
        var result = Segmenter.GetEndOfSentenceIndexes("a").ToList();

        Assert.Single(result);
        Assert.Equal(0, result[0]);
    }

    // ── basic sentence boundary types ──────────────────────────────────────────

    [Fact]
    public void TwoSentencesPeriod()
    {
        const string text = "Hello world. Hello.";
        //                   0123456789012345678
        //                             1111111111

        var result = Segmenter.GetEndOfSentenceIndexes(text).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(11, result[0]); // end of "Hello world."
        Assert.Equal(18, result[1]); // end of "Hello."
        AssertConsistentWithSegment(text);
    }

    [Fact]
    public void TwoSentencesQuestionMark()
    {
        const string text = "What is your name? My name is Jonas.";
        //                   012345678901234567890123456789012345
        //                             1111111111222222222233333333

        var result = Segmenter.GetEndOfSentenceIndexes(text).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(17, result[0]); // end of "What is your name?"
        Assert.Equal(35, result[1]); // end of "My name is Jonas."
        AssertConsistentWithSegment(text);
    }

    [Fact]
    public void TwoSentencesExclamationMark()
    {
        const string text = "There it is! I found it.";
        //                   012345678901234567890123
        //                             111111111122222

        var result = Segmenter.GetEndOfSentenceIndexes(text).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(11, result[0]); // end of "There it is!"
        Assert.Equal(23, result[1]); // end of "I found it."
        AssertConsistentWithSegment(text);
    }

    [Fact]
    public void ThreeSentences()
    {
        const string text = "One. Two. Three.";

        var result = Segmenter.GetEndOfSentenceIndexes(text).ToList();

        Assert.Equal(3, result.Count);
        Assert.Equal('.', text[result[0]]);
        Assert.Equal('.', text[result[1]]);
        Assert.Equal('.', text[result[2]]);
        Assert.Equal(text.Length - 1, result[2]);
        AssertConsistentWithSegment(text);
    }

    // ── abbreviations must not produce extra indexes ────────────────────────────

    [Fact]
    public void AbbreviationDoesNotSplit()
    {
        const string text = "My name is Jonas E. Smith.";

        var result = Segmenter.GetEndOfSentenceIndexes(text).ToList();

        Assert.Single(result);
        Assert.Equal(text.Length - 1, result[0]);
        AssertConsistentWithSegment(text);
    }

    [Fact]
    public void TwoLetterAbbreviationDoesNotSplit()
    {
        const string text = "Were Jane and co. at the party?";

        var result = Segmenter.GetEndOfSentenceIndexes(text).ToList();

        Assert.Single(result);
        Assert.Equal(text.Length - 1, result[0]);
        AssertConsistentWithSegment(text);
    }

    // ── result is lazy (IEnumerable) ──────────────────────────────────────────

    [Fact]
    public void ReturnsLazyEnumerable()
    {
        // IEnumerable<int>, not IList or array
        var result = Segmenter.GetEndOfSentenceIndexes("Hello world. Hello.");

        Assert.IsNotType<int[]>(result);
        Assert.IsNotAssignableFrom<IList<int>>(result);
    }

    [Fact]
    public void CanBeEnumeratedMultipleTimes()
    {
        var result = Segmenter.GetEndOfSentenceIndexes("Hello world. Hello.");

        Assert.Equal(result.ToList(), result.ToList());
    }

    // ── special content ────────────────────────────────────────────────────────

    [Fact]
    public void HandlesRegexContainingText()
    {
        const string text = "('$0 xyz, $1 abc, $0 def').";

        var result = Segmenter.GetEndOfSentenceIndexes(text).ToList();

        Assert.Single(result);
        Assert.Equal(text.Length - 1, result[0]);
    }

    [Fact]
    public void HandlesNonBreakingSpaceText()
    {
        const string text = "Trututu\u00A01. trututu\u00A02. trututu";

        var segments = Segmenter.Segment(text).ToList();
        var result   = Segmenter.GetEndOfSentenceIndexes(text).ToList();

        Assert.Equal(segments.Count, result.Count);
        AssertConsistentWithSegment(text);
    }

    // ── count always matches Segment ───────────────────────────────────────────

    [Theory]
    [InlineData("Hello World. My name is Jonas.")]
    [InlineData("What is your name? My name is Jonas.")]
    [InlineData("There it is! I found it.")]
    [InlineData("My name is Jonas E. Smith.")]
    [InlineData("Please turn to p. 55.")]
    [InlineData("Let's ask John Smith from FY Inc. for a quote.")]
    [InlineData("I visited the U.S.A. last year.")]
    public void CountMatchesSegmentCount(string text)
    {
        var segmentCount = Segmenter.Segment(text).Count;
        var indexCount   = Segmenter.GetEndOfSentenceIndexes(text).Count();

        Assert.Equal(segmentCount, indexCount);
    }

    // ── cleanText = false ──────────────────────────────────────────────────────

    [Fact]
    public void WithCleanTextFalse()
    {
        const string text = "Hello world. Hello.";

        var withClean    = Segmenter.GetEndOfSentenceIndexes(text, cleanText: true).ToList();
        var withoutClean = Segmenter.GetEndOfSentenceIndexes(text, cleanText: false).ToList();

        // Plain text is unaffected by cleaning, so results must be identical.
        Assert.Equal(withClean, withoutClean);
    }

    // ── indexes are within bounds and ordered ─────────────────────────────────

    [Theory]
    [InlineData("Hello World. My name is Jonas.")]
    [InlineData("What is your name? My name is Jonas.")]
    [InlineData("There it is! I found it.")]
    [InlineData("One. Two. Three. Four. Five.")]
    public void IndexesAreWithinBoundsAndStrictlyIncreasing(string text)
    {
        var indexes = Segmenter.GetEndOfSentenceIndexes(text).ToList();

        Assert.NotEmpty(indexes);

        int previous = -1;
        foreach (var idx in indexes)
        {
            Assert.InRange(idx, 0, text.Length - 1);
            Assert.True(idx > previous, $"index {idx} is not greater than previous {previous}");
            previous = idx;
        }
    }
}
