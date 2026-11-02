using CombinedLab.Analysis;
using System.Xml.Serialization;

namespace CombinedLab.Models;

[XmlRoot("Text")]
public sealed class Text
{
    [XmlElement("Sentence")]
    public List<Sentence> Sentences { get; set; } = new();

    public List<Sentence> SentencesByWordCount()
    {
        List<Sentence> result = new List<Sentence>(Sentences);
        result.Sort((first, second) => first.WordCount.CompareTo(second.WordCount));
        return result;
    }

    public List<Sentence> SentencesByLength()
    {
        List<Sentence> result = new List<Sentence>(Sentences);
        result.Sort((first, second) => first.Length.CompareTo(second.Length));
        return result;
    }

    public IReadOnlyList<string> FindUniqueWordsInQuestions(int length)
    {
        List<string> words = new List<string>();

        foreach (Sentence sentence in Sentences)
        {
            if (!sentence.IsQuestion)
            {
                continue;
            }

            foreach (Token token in sentence.Tokens)
            {
                Word? word = token as Word;
                if (word == null || word.Value.Length != length)
                {
                    continue;
                }

                bool alreadyAdded = false;
                foreach (string addedWord in words)
                {
                    if (string.Equals(addedWord, word.Value, StringComparison.OrdinalIgnoreCase))
                    {
                        alreadyAdded = true;
                        break;
                    }
                }

                if (!alreadyAdded)
                {
                    words.Add(word.Value);
                }
            }
        }

        words.Sort(StringComparer.OrdinalIgnoreCase);
        return words;
    }

    public int RemoveConsonantWords(int length)
    {
        int removed = 0;

        foreach (Sentence sentence in Sentences)
        {
            for (int index = 0; index < sentence.Tokens.Count; index++)
            {
                Word? word = sentence.Tokens[index] as Word;
                if (word != null && word.Value.Length == length && StartsWithConsonant(word.Value))
                {
                    sentence.Tokens.RemoveAt(index);
                    index--;
                    removed++;
                }
            }
        }

        return removed;
    }

    public int ReplaceWordsInSentence(int sentenceIndex, int length, string replacement)
    {
        Sentence sentence = Sentences[sentenceIndex];
        int replaced = 0;

        for (int index = 0; index < sentence.Tokens.Count; index++)
        {
            Word? word = sentence.Tokens[index] as Word;
            if (word != null && word.Value.Length == length)
            {
                if (replacement.Length == 0)
                {
                    sentence.Tokens.RemoveAt(index);
                    index--;
                }
                else
                {
                    sentence.Tokens[index] = new Word(replacement, word.LineNumber);
                }

                replaced++;
            }
        }

        return replaced;
    }

    public int RemoveStopWords(ISet<string> stopWords)
    {
        int removed = 0;

        foreach (Sentence sentence in Sentences)
        {
            for (int index = 0; index < sentence.Tokens.Count; index++)
            {
                Word? word = sentence.Tokens[index] as Word;
                if (word != null && stopWords.Contains(word.Value))
                {
                    sentence.Tokens.RemoveAt(index);
                    index--;
                    removed++;
                }
            }
        }

        return removed;
    }

    public IReadOnlyList<ConcordanceEntry> BuildConcordance() => ConcordanceBuilder.Build(this);

    private static bool StartsWithConsonant(string value)
    {
        if (value.Length == 0 || !char.IsLetter(value[0]))
        {
            return false;
        }

        var first = char.ToLowerInvariant(value[0]);
        const string vowels = "aeiouаеёиоуыэюя";
        return !vowels.Contains(first);
    }

    public override string ToString()
    {
        string result = "";
        foreach (Sentence sentence in Sentences)
        {
            if (result.Length > 0)
            {
                result += " ";
            }

            result += sentence.ToString();
        }

        return result;
    }
}
