using System;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using PALC.Views;

namespace PALC;

public partial class App : Application
{
    public override void Initialize()
    {
        SetupExceptionHandling();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new SplashV();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new SplashV();
        }

        base.OnFrameworkInitializationCompleted();
    }


    private static void SetupExceptionHandling()
    {
        AppDomain.CurrentDomain.UnhandledException += (s, a) =>
        {
            ExceptionDispatchInfo.Capture((Exception)a.ExceptionObject).Throw();
        };

        //Current.DispatcherUnhandledException += (s, a) =>
        //{
        //    HandleException(a.Exception);
        //    a.Handled = true;
        //};

        TaskScheduler.UnobservedTaskException += (s, a) =>
        {
            var ex = a.Exception;
            a.SetObserved();
        };
    }


    //void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    //{
    //    string errorMessage = $"An unhandled exception occurred: {e.Exception.Message}";
    //    var msbox = MessageBoxManager.GetMessageBoxStandard("Error!", errorMessage);
    //    msbox.ShowAsync();
    //    throw new Exception(errorMessage);
    //}
}
