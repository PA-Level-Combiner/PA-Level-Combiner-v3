using CommunityToolkit.Mvvm.Input;
using System;

namespace PALC.Main.ViewModels.Splash.Inner;


public partial class AboutVM : ViewModelBase
{
    public static string Version => Globals.PALCVersion;

    public static string GithubLink => Globals.githubLink;
    public static string GithubIssuesLink => Globals.githubIssuesLink;


    [RelayCommand]
    public static void Crash()
    {
        throw new Exception("Manual crash!! WTF!! Why!!");
    }
}
