using System.Text;
using CombinedLab.Models;
using CombinedLab.Parsing;
using CombinedLab.UI;

namespace CombinedLab;

internal static class Program
{
    private static int Main(string[] args)
    {
        Console.OutputEncoding = new UTF8Encoding(false);
        Console.InputEncoding = new UTF8Encoding(false);

        string? inputPath = null;
        if (args.Length > 0)
        {
            inputPath = args[0];
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            Console.Write("Путь к текстовому файлу (Enter — использовать sample.txt): ");
            inputPath = Console.ReadLine();
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            inputPath = Path.Combine(AppContext.BaseDirectory, "sample.txt");
        }
        else
        {
            inputPath = inputPath.Trim().Trim('"');
        }

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Файл не найден: {inputPath}");
            return 1;
        }

        Text text = TextParser.Parse(File.ReadAllText(inputPath));
        Console.WriteLine($"Загружено предложений: {text.Sentences.Count}");
        return new TextAnalyzerConsole().Run(text);
    }
}
