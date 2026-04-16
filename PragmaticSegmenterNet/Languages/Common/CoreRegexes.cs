using System.Text.RegularExpressions;

namespace PragmaticSegmenterNet.Languages.Common;

internal static class CoreRegexes
{
    public static Regex MultiPeriodAbbreviationRegex = new Regex(@"\b[a-z](?:\.[a-z])+[.]", RegexOptions.IgnoreCase);

    public static Regex ContinuousPunctuationRegex = new Regex(@"(?<=\S)(!|\?){3,}(?=(\s|\z|$))");

    public static Regex ParenthesesBetweenDoubleQuotesRegex = new Regex("[\"\u201c\u201d]\\s\\(.*\\)\\s[\"\u201c\u201d]");

    public static Regex SentenceBoundaryRegex = new Regex("\uff08(?:[^\uff09])*\uff09(?=\\s?[A-Z])|\u300c(?:[^\u300d])*\u300d(?=\\s[A-Z])|\\((?:[^\\)]){2,}\\)(?=\\s[A-Z])|'(?:[^'])*[^,]'(?=\\s[A-Z])|\"(?:[^\"])*[^,]\"(?=\\s[A-Z])|\u201c(?:[^\u201d])*[^,]\u201d(?=\\s[A-Z])|\\S.*?[\u3002\uff0e.\uff01!?\uff1f\u0238\u0239\u2609\u2608\u2607\u2604]");

    public static Regex QuotationAtEndOfSentenceRegex = new Regex("[!?\\.-][\"'\u201d\u201c]\\s{1}[A-Z]");

    public static Regex SplitSpaceQuotationAtEndOfSentenceRegex = new Regex("(?<=[!?\\.-][\"'\u201d\u201c])\\s{1}(?=[A-Z])");
}
