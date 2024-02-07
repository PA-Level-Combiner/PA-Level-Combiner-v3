using System.Linq;

using Avalonia.Controls;
using MsBox.Avalonia;

using PALC.Models;
using PALC.ViewModels.Splash;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using CommunityToolkit.Mvvm.Input;
using System;

namespace PALC.Views;


public class VersionChoice
{
    public required Models.Version version;
    public required Func<Window> combinerWindow;

    public string DisplayText
    {
        get => $"{version.version_str} ({version.branch})";
    }
}

public static class VersionChoices
{
    public static readonly List<VersionChoice> versionChoices = [
       new VersionChoice {
            version = Versions._20_4_4,
            combinerWindow = () => new Combiners._20_4_4.MainV()
        }
   ];
}


public partial class SplashV : Window
{
    public SplashV()
    {
        InitializeComponent();

        DataContext = new SplashVM();
    }


    public static List<string> VersionChoicesDisplay
    {
        get => ["Select a version...", ..VersionChoices.versionChoices.Select(x => x.DisplayText).ToList()];
    }


    public static readonly StyledProperty<int> SelectedIndexProperty =
        AvaloniaProperty.Register<SplashV, int>(nameof(SelectedIndex));

    public int SelectedIndex
    {
        get => GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }



    [RelayCommand]
    public async Task OpenVersion(int selectedIndex)
    {
        if (selectedIndex == 0)
        {
            await MessageBoxManager.GetMessageBoxStandard("Error", "You have not selected a version.").ShowWindowDialogAsync(this);
            return;
        }

        int newIndex = selectedIndex - 1;

        var window = VersionChoices.versionChoices[newIndex].combinerWindow;

        Hide();
        window().Show();
        Close();
    }
}
