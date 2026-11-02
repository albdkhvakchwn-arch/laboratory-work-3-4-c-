using System.Xml.Serialization;

namespace CombinedLab.Models;

public sealed class Sentence
{
    [XmlElement("Word", typeof(Word))]
    [XmlElement("Punctuation", typeof(Punctuation))]
    public List<Token> Tokens { get; set; } = new();

    [XmlIgnore]
    public int WordCount
    {
        get
        {
            int count = 0;
            foreach (Token token in Tokens)
            {
                if (token is Word)
                {
                    count++;
                }
            }

            return count;
        }
    }

    [XmlIgnore]
    public bool IsQuestion
    {
        get
        {
            foreach (Token token in Tokens)
            {
                Punctuation? punctuation = token as Punctuation;
                if (punctuation != null && punctuation.Value.Contains('?'))
                {
                    return true;
                }
            }

            return false;
        }
    }

    [XmlIgnore]
    public int Length => ToString().Length;

    public override string ToString()
    {
        string result = "";
        bool insideDoubleQuotes = false;
        bool previousWasOpeningMark = false;

        for (int i = 0; i < Tokens.Count; i++)
        {
            Token token = Tokens[i];
            bool addSpace = i > 0;
            bool currentIsOpeningMark = false;

            Punctuation? punctuation = token as Punctuation;
            if (punctuation != null)
            {
                if (punctuation.Value == "\"")
                {
                    currentIsOpeningMark = !insideDoubleQuotes;
                    if (insideDoubleQuotes)
                    {
                        addSpace = false;
                    }

                    insideDoubleQuotes = !insideDoubleQuotes;
                }
                else if (punctuation.Value == "«" || punctuation.Value == "“" || IsOpeningPunctuation(punctuation.Value))
                {
                    currentIsOpeningMark = true;
                }
                else if (IsClosingPunctuation(punctuation.Value))
                {
                    addSpace = false;
                }
            }

            if (previousWasOpeningMark)
            {
                addSpace = false;
            }

            if (addSpace)
            {
                result += " ";
            }

            result += token.Value;
            previousWasOpeningMark = currentIsOpeningMark;
        }

        return result.ToString();
    }

    private static bool IsOpeningPunctuation(string value) => "([{“.Contains(value);

    private static bool IsClosingPunctuation(string value) => ",.;:!?…)]}»”'’".Contains(value);
}
