using CombinedLab.Models;

namespace CombinedLab.Analysis;

public class ConcordanceEntry
{
    public ConcordanceEntry(string word, int frequency, List<int> lines)
    {
        Word = word;
        Frequency = frequency;
        Lines = lines;
    }

    public string Word { get; set; }
    public int Frequency { get; set; }
    public List<int> Lines { get; set; }
}

public static class ConcordanceBuilder
{
    public static List<ConcordanceEntry> Build(Text text)
    {
        Dictionary<string, int> frequencies = new Dictionary<string, int>();
        Dictionary<string, SortedSet<int>> linesByWord = new Dictionary<string, SortedSet<int>>();
        SortedSet<string> sortedWords = new SortedSet<string>();

        foreach (Sentence sentence in text.Sentences)
        {
            foreach (Token token in sentence.Tokens)
            {
                Word? word = token as Word;
                if (word == null)
                {
                    continue;
                }

                string normalizedWord = word.Value.ToLowerInvariant();
                if (!frequencies.ContainsKey(normalizedWord))
                {
                    frequencies[normalizedWord] = 0;
                    linesByWord[normalizedWord] = new SortedSet<int>();
                    sortedWords.Add(normalizedWord);
                }

                frequencies[normalizedWord]++;
                linesByWord[normalizedWord].Add(word.LineNumber);
            }
        }

        List<ConcordanceEntry> result = new List<ConcordanceEntry>();
        foreach (string word in sortedWords)
        {
            List<int> lines = new List<int>(linesByWord[word]);
            result.Add(new ConcordanceEntry(word, frequencies[word], lines));
        }

        return result;
    }
}
