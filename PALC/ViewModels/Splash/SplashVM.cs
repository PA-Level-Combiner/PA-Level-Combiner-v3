using Avalonia;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Shapes;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PALC.Models.Combiners._20_4_4.LevelComponents;
using PALC.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PALC.ViewModels.Splash;




public partial class SplashVM : ViewModelBase
{
    public static string Version => Globals.palcVersion;


    private readonly Random randomizer = new(DateTime.Now.ToString().GetHashCode());

    [ObservableProperty]
    public string currentSplashText = string.Empty;


    private List<string>? _splashTexts = null;
    public List<string> SplashTexts
    {
        get {
            if (_splashTexts != null) return _splashTexts;

            Stream s = AssetLoader.Open(new Uri("avares://PALC/Assets/splash_texts.txt"));
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
