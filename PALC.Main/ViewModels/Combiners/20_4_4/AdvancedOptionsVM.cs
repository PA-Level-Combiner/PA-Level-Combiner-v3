using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using PALC.Main.Models.Combiners._20_4_4;
using PALC.Main.Models.Combiners._20_4_4.LevelComponents;

namespace PALC.Main.ViewModels.Combiners._20_4_4;

public partial class AdvancedOptionsVM : ViewModelBase
{
    [ObservableProperty]
    private IncludeOptions includeOptionSettings = new();


    [ObservableProperty]
    public string? themeFolderDisplay;

    public ObservableCollection<ThemeFileInfo> ThemeFiles { get; set; } = [];

    public class InvalidThemesFolderArgs
    {
        public required Exception ex;
    }
    public event AsyncEventHandler<InvalidThemesFolderArgs>? InvalidThemesFolder;

    public class NoThemesArgs { }
    public event AsyncEventHandler<NoThemesArgs>? NoThemes;

    [RelayCommand]
    public async Task SetThemesFolder(string path)
    {
        try
        {
            var themeFiles = ThemeFolderOpener.GetThemeFilesFromFolder(path);
            if (themeFiles.Count == 0)
            {
                if (NoThemes != null)
                    await NoThemes(this, new NoThemesArgs { });

                return;
            }

            foreach (var themeFile in themeFiles) ThemeFiles.Add(themeFile);
            

            ThemeFolderDisplay = path;
        }
        catch (Exception ex)
        {
            if (InvalidThemesFolder != null)
                await InvalidThemesFolder(this, new InvalidThemesFolderArgs { ex = ex });

            return;
        }
    }


    public readonly string defaultThemesPath = @"C:\Program Files (x86)\Steam\steamapps\common\Project Arrhythmia\beatmaps\themes";

    public class ThemesLoadFailedArgs
    {
        public required Exception ex;
        public required string defaultThemesPath;
    }
    public event AsyncEventHandler<ThemesLoadFailedArgs>? InitialThemeLoadFailed;

    [RelayCommand]
    public async Task LoadDefaultThemes()
    {
        try
        {
            var themeFiles = ThemeFolderOpener.GetThemeFilesFromFolder(defaultThemesPath);
            foreach (var themeFile in themeFiles) ThemeFiles.Add(themeFile);
            ThemeFolderDisplay = defaultThemesPath;
        }
        catch (Exception ex)
        {
            if (InitialThemeLoadFailed != null)
                await InitialThemeLoadFailed(this, new ThemesLoadFailedArgs { ex = ex, defaultThemesPath = defaultThemesPath });

            return;
        }

    }


    public event AsyncEventHandler<ThemesLoadFailedArgs>? ResetThemeLoadFailed;

    [RelayCommand]
    public async Task Reset()
    {
        IncludeOptionSettings = new();

        try
        {
            var themeFiles = ThemeFolderOpener.GetThemeFilesFromFolder(defaultThemesPath);
            foreach (var themeFile in themeFiles) ThemeFiles.Add(themeFile);
            ThemeFolderDisplay = defaultThemesPath;
        }
        catch (Exception ex)
        {
            if (ResetThemeLoadFailed != null)
                await ResetThemeLoadFailed(this, new ThemesLoadFailedArgs { ex = ex, defaultThemesPath = defaultThemesPath });

            return;
        }
    }
}
