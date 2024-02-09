using System.Diagnostics;

namespace PALCCrashHandler;

internal class Program
{
    static void Main(string[] args)
    {
        string palcVersion = args[0];
        string logFilesPath = args[1];
        string githubIssuesLink = args[2];
        string errorMessage = args[3];
        string stackTrace = args[4];

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(
            $"------------------------------------------------------------\n" +
            $"A fatal error has occurred.\n" +
            $"\n" +
            $"\n" +
            $"Here are some resources to report this error:\n" +
            $"\n" +
            $"REPORT YOUR ISSUES HERE: {githubIssuesLink}\n" +
            $"Log files are stored in: {logFilesPath}\n" +
            $"\n" +
            $"Current PALC version: {palcVersion}\n" +
            $"\n" +
            $"\n" +
            $"Scroll down for more options, such as opening the issues page.\n" +
            $"------------------------------------------------------------");

        Console.ResetColor();
        Console.WriteLine(
            $"\n" +
            $"\n" +
            $"\n" +
            $"{errorMessage}\n" +
            $"{stackTrace}"
        );


        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(
            "\n\n\n" +
            "--- PALC crashed. Please scroll up to view more important information. ---" +
            "\n\n"
        );

        Console.ResetColor();

        while (true)
        {
            Console.WriteLine(
                "Please choose an action (repeatable until exit):\n" +
                "[O] Open Github Link for reporting\n" +
                "[L] Open logs folder\n" +
                "[any other key] Exit\n"
            );

            Console.Write("> ");
            ConsoleKeyInfo input = Console.ReadKey(true);

            Console.WriteLine("\n\n");

            try
            {
                if (input.Key == ConsoleKey.O)
                    Process.Start(new ProcessStartInfo(githubIssuesLink) { UseShellExecute = true });
                else if (input.Key == ConsoleKey.L)
                {
                    Process.Start("explorer.exe", logFilesPath);
                }
                else break;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); };
        }
    }
}
