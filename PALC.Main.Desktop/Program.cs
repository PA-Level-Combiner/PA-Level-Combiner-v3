using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Avalonia;

using PALC.Main;
using System.Diagnostics;
using System.Collections.Generic;

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
            _logger.Info("Test!");
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            string crashHandlerPath = "PALCCrashHandler.exe";

            List<string> crashArgs = new() {
                Globals.PALCVersion,
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
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
