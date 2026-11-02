namespace CombinedLab.UI;

internal static class ConsoleInput
{
    public static int ReadPositiveInt(string prompt) => ReadIntInRange(prompt, 1, int.MaxValue);

    public static int ReadIntInRange(string prompt, int minimum, int maximum)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value) && value >= minimum && value <= maximum)
            {
                return value;
            }

            Console.WriteLine($"Введите целое число от {minimum} до {maximum}.");
        }
    }
}
