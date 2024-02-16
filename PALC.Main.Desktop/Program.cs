using System;
using System.Runtime.ExceptionServices;
using Avalonia;
using System.Diagnostics;
using System.Collections.Generic;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.MaterialDesign;

namespace PALC.Main.Desktop;

class Program
{
    private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) {
        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            string crashHandlerPath = "CrashHandler\\PALC.CrashHandler.exe";

            List<string> crashArgs = new() {
                Globals.programName,
                ProgramInfo.GetProgramVersion()?.ToString() ?? "Unknown version",
                Globals.logsPath,
                Globals.githubIssuesLink,
                ex.Message,
                ex.StackTrace ?? "No stack trace available"
            };


            _logger.Fatal(
                "A fatal error occurred.\n" +
                $"{ex.StackTrace}\n" +
                $"\n" +
                $"{ex.Message}"
            );

            _logger.Info("Running crash handler...");
            Process.Start(crashHandlerPath, crashArgs);
            _logger.Info("Exiting with exception...");
            ExceptionDispatchInfo.Capture(ex).Throw();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        IconProvider.Current.Register<MaterialDesignIconProvider>();

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}
