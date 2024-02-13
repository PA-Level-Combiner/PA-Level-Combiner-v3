using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using PALC.Main.Models;
using PALC.Main.Models.Combiners._20_4_4;
using PALC.Main.Models.Combiners._20_4_4.Exceptions;
using PALC.Main.Models.Combiners._20_4_4.LevelComponents;

namespace PALC.Main.ViewModels.Combiners._20_4_4;

public partial class AdvancedOptionsVM : ViewModelBase
{
    private static Logger _logger = LogManager.GetCurrentClassLogger();


    [ObservableProperty]
    private IncludeOptions includeOptionSettings = new();


    [ObservableProperty]
    public string? themeFolderDisplay;

    public ObservableCollection<ThemeFileInfo> ThemeFiles { get; set; } = [];


    public event AsyncEventHandler<DisplayGeneralErrorArgs>? InvalidThemesFolder;

    public void SetThemesFolder(string path)
    {
        var themeFiles = ThemeFolderOpener.GetThemeFilesFromFolder(path);
        if (themeFiles.Count == 0) throw new ArgumentException(path);
        foreach (var themeFile in themeFiles) ThemeFiles.Add(themeFile);
        ThemeFolderDisplay = path;
    }


    [RelayCommand]
    public async Task SetThemesFolderLogged(string path)
    {
        try
        {
            SetThemesFolder(path);
        }
        catch (Exception ex) when (ErrorHelper.IsFileException(ex))
        {
            _logger.Error(ex, "Cannot access directory or files of {path}.", path);
            await AEHHelper.RunAEH(InvalidThemesFolder, this, new(
                $"The program cannot access the theme folder \"{path}\" or its themes.",
                ex
            ));
            return;
        }
        catch (ThemeDuplicateException ex)
        {
            _logger.Error(ex, "The themes {path1} and {path2} have the duplicate ID {themeId}.", ex.path1, ex.path2, ex.themeId);
            await AEHHelper.RunAEH(InvalidThemesFolder, this, new(
                $"Duplicate theme ID ${ex.themeId} found in \"{ex.path1}\" and \"{ex.path2}\".",
                ex
            ));
            return;
        }
        catch (ArgumentException ex) when (ex.Message == path)
        {
            _logger.Error("{path} has no themes.", path);
            await AEHHelper.RunAEH(InvalidThemesFolder, this, new(
                $"The theme folder \"{path}\" has no themes.", null
            ));
            return;
        }
    }


    public readonly string defaultThemesPath = @"C:\Program Files (x86)\Steam\steamapps\common\Project Arrhythmia\beatmaps\themes";
    public event AsyncEventHandler<DisplayGeneralErrorArgs>? InitialThemeLoadFailed;

    [RelayCommand]
    public async Task LoadDefaultThemes()
    {
        try
        {
            SetThemesFolder(defaultThemesPath);
        }
        catch (Exception ex) {
            _logger.Error(ex, "Cannot load default themes path {defaultThemesPath}.", defaultThemesPath);
            await AEHHelper.RunAEH(InitialThemeLoadFailed, this, new(
                $"The program cannot load the default themes path at \"{defaultThemesPath}\". Please set a new theme folder in the \"Advanced Options\" button.\n",
                ex
            ));
            return;
        }
    }


    [RelayCommand]
    public async Task Reset()
    {
        IncludeOptionSettings = new();
        await LoadDefaultThemes();
    }
}
