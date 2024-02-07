using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PALC.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PALC.ViewModels.Splash;




public partial class SplashVM : ViewModelBase
{
    private readonly Random randomizer = new(DateTime.Now.ToString().GetHashCode());

    [ObservableProperty]
    public string currentSplashText = string.Empty;
    private List<string> splashTexts = [
        "project heart attack",
        "Insert Splash Text Here",
        "Now with added syrup. Specific kinds of syrup.",
        "collab with me uwu",
        "Levels are just 1s and 0s to give you euphoria",
        "JSON is easy to use but is incredibly inefficient",
        "Does this also combine audio files??",
        "Real",
        "How deep is your dictionary traversal",
        "Now with 15 different quantum states",
        "Fire in the hole",
        "Now with gradients! Ha- got you excited-",
        "You up at 3am combining levels",
        "Beans"
    ];

    public void RandomSplashText()
    {
        int randomIdx = randomizer.Next(0, splashTexts.Count);
        CurrentSplashText = splashTexts[randomIdx];
    }
}
