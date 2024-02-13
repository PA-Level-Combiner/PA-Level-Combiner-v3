using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using Semver;
using System;
using System.Collections.Generic;
using System.IO;

namespace PALC.Main.ViewModels.Splash;




public partial class SplashVM : ViewModelBase
{
    public static SemVersion? Version => ProgramInfo.GetProgramVersion();


    private readonly Random randomizer = new(DateTime.Now.ToString().GetHashCode());

    [ObservableProperty]
    public string currentSplashText = string.Empty;


    private List<string>? _splashTexts = null;
    public List<string> SplashTexts
    {
        get {
            if (_splashTexts != null) return _splashTexts;

            Stream s = AssetLoader.Open(new Uri("avares://PALC.Main/Assets/splash_texts.txt"));
            string result = new StreamReader(s).ReadToEnd();

            _splashTexts = [.. result.Split('\n')];
            return _splashTexts;
        }
    }

    public void RandomSplashText()
    {
        int randomIdx = randomizer.Next(0, SplashTexts.Count);
        CurrentSplashText = SplashTexts[randomIdx];
    }
}
