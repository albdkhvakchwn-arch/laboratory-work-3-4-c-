namespace CombinedLab.Services;

public static class StopWordProvider
{
    public static HashSet<string> Load(StopWordLanguage language)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var directory = Path.Combine(AppContext.BaseDirectory, "Data");

        if (language == StopWordLanguage.Russian || language == StopWordLanguage.Both)
        {
            result.UnionWith(ReadList(Path.Combine(directory, "stopwords_ru.txt")));
        }

        if (language == StopWordLanguage.English || language == StopWordLanguage.Both)
        {
            result.UnionWith(ReadList(Path.Combine(directory, "stopwords_en.txt")));
        }

        return result;
    }

    private static List<string> ReadList(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"Список не найден: {path}");
            return new List<string>();
        }

        List<string> words = new List<string>();
        foreach (string line in File.ReadLines(path))
        {
            string word = line.Trim();
            if (word.Length > 0 && !word.StartsWith("#"))
            {
                words.Add(word);
            }
        }

        return words;
    }
}
