using Avalonia.Controls;

using PALC.Main.Models;
using PALC.Main.ViewModels.Splash;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System;
using PALC.Main.Views.Splash.Inner;
using PALC.Common.Views.Templates;

namespace PALC.Main.Views.Splash;


public class VersionChoice
{
    public required Models.Version version;
    public required Func<Window> combinerWindow;

    public string DisplayText
    {
        get => $"{version.version_str} ({version.branch})";
    }
}


public partial class SplashV : Window
{
    private readonly SplashVM vm;
    public SplashV()
    {
        InitializeComponent();

        vm = new SplashVM();
        DataContext = vm;
    }

    private void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        vm.RandomSplashText();
    }

    public static List<VersionChoice> VersionChoices => [
       new VersionChoice {
            version = Versions._20_4_4,
            combinerWindow = () => new Combiners._20_4_4.MainV()
        }
    ];

    public VersionChoice? SelectedVersionChoice { get; set; } = null;



    [RelayCommand]
    public async Task OpenVersion()
    {
        if (SelectedVersionChoice == null) {
            await MessageBoxTools.CreateErrorMsgBox("You have not selected a version.").ShowWindowDialogAsync(this);
            return;
        }

        Hide();
        SelectedVersionChoice.combinerWindow().Show();
        Close();
    }


    private AsyncRelayCommand? _onAboutCommand = null;
    public AsyncRelayCommand OnAboutCommand => _onAboutCommand ??= new(OnAbout);
    public async Task OnAbout()
    {
        About window = new();
        await window.ShowDialog(this);
    }
}
