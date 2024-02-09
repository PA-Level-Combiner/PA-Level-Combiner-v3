using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PALC.Main.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PALC.Main.ViewModels.Splash.Inner;


public partial class AboutVM : ViewModelBase
{
    public static string Version => Globals.PALCVersion;

    public static string GithubLink => Globals.githubLink;
    public static string GithubIssuesLink => Globals.githubIssuesLink;
}
