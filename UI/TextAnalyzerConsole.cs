using CombinedLab.Models;
using CombinedLab.Services;

namespace CombinedLab.UI;

public sealed class TextAnalyzerConsole
{
    public int Run(Text text)
    {
        while (true)
        {
            ShowMenu();
            Console.Write("Выберите действие: ");
            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1": ShowSentences(text.SentencesByWordCount(), "Предложения по возрастанию числа слов"); break;
                case "2": ShowSentences(text.SentencesByLength(), "Предложения по возрастанию длины"); break;
                case "3": FindQuestionWords(text); break;
                case "4": RemoveConsonantWords(text); break;
                case "5": ReplaceWords(text); break;
                case "6": RemoveStopWords(text); break;
                case "7": ExportXml(text); break;
                case "8": ShowConcordance(text); break;
                case "0": return 0;
                default: Console.WriteLine("Введите номер пункта от 0 до 8."); break;
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("1 — сортировка предложений по количеству слов");
        Console.WriteLine("2 — сортировка предложений по длине");
        Console.WriteLine("3 — уникальные слова заданной длины в вопросах");
        Console.WriteLine("4 — удалить слова заданной длины, начинающиеся с согласной");
        Console.WriteLine("5 — заменить слова в выбранном предложении");
        Console.WriteLine("6 — удалить стоп-слова");
        Console.WriteLine("7 — экспортировать текст в XML");
        Console.WriteLine("8 — построить конкорданс");
        Console.WriteLine("0 — выход");
    }

    private static void ShowSentences(IEnumerable<Sentence> sentences, string title)
    {
        Console.WriteLine($"\n{title}:");
        var index = 0;
        foreach (var sentence in sentences)
        {
            index++;
            Console.WriteLine($"{index}. ({sentence.WordCount} слов, {sentence.Length} символов) {sentence}");
        }

        if (index == 0)
        {
            Console.WriteLine("Предложений нет.");
        }
    }

    private static void FindQuestionWords(Text text)
    {
        var length = ConsoleInput.ReadPositiveInt("Длина слова: ");
        var words = text.FindUniqueWordsInQuestions(length);
        Console.WriteLine(words.Count == 0 ? "Подходящих слов не найдено." : string.Join(", ", words));
    }

    private static void RemoveConsonantWords(Text text)
    {
        var length = ConsoleInput.ReadPositiveInt("Длина удаляемых слов: ");
        var removed = text.RemoveConsonantWords(length);
        Console.WriteLine($"Удалено слов: {removed}");
        Console.WriteLine(text);
    }

    private static void ReplaceWords(Text text)
    {
        if (text.Sentences.Count == 0)
        {
            Console.WriteLine("В тексте нет предложений.");
            return;
        }

        ShowSentences(text.Sentences, "Предложения (нумерация с 1)");
        var sentenceNumber = ConsoleInput.ReadIntInRange("Номер предложения: ", 1, text.Sentences.Count);
        var length = ConsoleInput.ReadPositiveInt("Длина заменяемых слов: ");
        Console.Write("Подстрока для замены (пустая строка удалит подходящие слова): ");
        var replacement = Console.ReadLine() ?? string.Empty;
        var replaced = text.ReplaceWordsInSentence(sentenceNumber - 1, length, replacement);
        Console.WriteLine($"Заменено слов: {replaced}");
        Console.WriteLine(text.Sentences[sentenceNumber - 1]);
    }

    private static void RemoveStopWords(Text text)
    {
        Console.WriteLine("1 — русский, 2 — английский, 3 — оба списка");
        var choice = ConsoleInput.ReadIntInRange("Выбор: ", 1, 3);
        var language = (StopWordLanguage)choice;
        var stopWords = StopWordProvider.Load(language);
        var removed = text.RemoveStopWords(stopWords);
        Console.WriteLine($"Удалено стоп-слов: {removed}");
        Console.WriteLine(text);
    }

    private static void ExportXml(Text text)
    {
        Console.Write("Имя XML-файла (Enter — text.xml): ");
        var path = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(path))
        {
            path = "text.xml";
        }

        var outputPath = path.Trim().Trim('"');
        TextXmlExporter.Export(text, outputPath);
        Console.WriteLine($"Текст сохранён в {Path.GetFullPath(outputPath)}");
    }

    private static void ShowConcordance(Text text)
    {
        var entries = text.BuildConcordance();
        if (entries.Count == 0)
        {
            Console.WriteLine("В тексте нет слов.");
            return;
        }

        Console.WriteLine("Слово — частота — номера строк:");
        foreach (var entry in entries)
        {
            Console.WriteLine($"{entry.Word} — {entry.Frequency}: {string.Join(", ", entry.Lines)}");
        }
    }
}
