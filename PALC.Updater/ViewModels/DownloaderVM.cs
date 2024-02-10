using System;

namespace PALC.Updater.ViewModels;



public class GithubRelease
{
    public required string name;
    public required string url;
    public required Version version;
}


public partial class DownloaderVM : ViewModelBase
{
    public static string Greeting => "Welcome to Avalonia!";
}
