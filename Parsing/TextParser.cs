using System.Text;
using CombinedLab.Models;

namespace CombinedLab.Parsing;

public static class TextParser
{
    public static Text Parse(string source)
    {
        Text text = new Text();
        Sentence sentence = new Sentence();
        int lineNumber = 1;
        int index = 0;

        while (index < source.Length)
        {
            char current = source[index];

            if (char.IsLetterOrDigit(current))
            {
                int wordLine = lineNumber;
                StringBuilder wordValue = new StringBuilder();

                while (index < source.Length)
                {
                    current = source[index];
                    if (char.IsLetterOrDigit(current))
                    {
                        wordValue.Append(current);
                        index++;
                    }
                    else if ((current == '\'' || current == '’') &&
                             wordValue.Length > 0 &&
                             index + 1 < source.Length &&
                             char.IsLetter(source[index + 1]))
                    {
                        wordValue.Append(current);
                        index++;
                    }
                    else
                    {
                        break;
                    }
                }

                sentence.Tokens.Add(new Word(wordValue.ToString(), wordLine));
                continue;
            }

            if (current == '\r' || current == '\n')
            {
                if (current == '\n' || (current == '\r' && (index + 1 >= source.Length || source[index + 1] != '\n')))
                {
                    lineNumber++;
                }

                index++;
                continue;
            }

            if (char.IsWhiteSpace(current))
            {
                index++;
                continue;
            }

            sentence.Tokens.Add(new Punctuation(current.ToString()));

            if (IsSentenceEnd(current))
            {
                while (index + 1 < source.Length &&
                       (IsSentenceEnd(source[index + 1]) || IsClosingMark(source[index + 1])))
                {
                    index++;
                    sentence.Tokens.Add(new Punctuation(source[index].ToString()));
                }

                text.Sentences.Add(sentence);
                sentence = new Sentence();
            }

            index++;
        }

        if (sentence.Tokens.Count > 0)
        {
            text.Sentences.Add(sentence);
        }

        return text;
    }

    private static bool IsSentenceEnd(char character)
    {
        return character == '.' || character == '!' || character == '?' || character == '…';
    }

    private static bool IsClosingMark(char character)
    {
        return character == '"' || character == '\'' || character == '’' ||
               character == '»' || character == '”' || character == ')' ||
               character == ']' || character == '}';
    }
}
