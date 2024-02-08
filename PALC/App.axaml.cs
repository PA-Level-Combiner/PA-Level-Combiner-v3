using System;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using PALC.Views.Splash;
using Avalonia.Controls;
using PALC.Views.Templates;
using Avalonia.Threading;
using Avalonia.Media;
using Avalonia.Layout;

namespace PALC;

public partial class App : Application
{
    public SplashV? mainWindow = null;

    public override void Initialize()
    {
        SetupExceptionHandling();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = mainWindow ??= new();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = mainWindow ??= new();
        }

        base.OnFrameworkInitializationCompleted();
    }



    private void SetupExceptionHandling()
    {
        //Current.DispatcherUnhandledException += (s, a) =>
        //{
        //    HandleException(a.Exception);
        //    a.Handled = true;
        //};

        // TaskScheduler.UnobservedTaskException += OnUnhandledException;
    }


    //void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    //{
    //    string errorMessage = $"An unhandled exception occurred: {e.Exception.Message}";
    //    var msbox = MessageBoxManager.GetMessageBoxStandard("Error!", errorMessage);
    //    msbox.ShowAsync();
    //    throw new Exception(errorMessage);
    //}
}
