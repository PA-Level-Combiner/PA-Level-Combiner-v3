using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using PALC.Main.Models.Combiners._20_4_4;
using PALC.Main.Models.Combiners._20_4_4.exceptions;
using PALC.Main.Models.Combiners._20_4_4.LevelComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PALC.Main.ViewModels.Combiners._20_4_4;


public partial class MainVM(AdvancedOptionsVM advancedOptionsVM) : ViewModelBase
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();


    public AdvancedOptionsVM advancedOptionsVM = advancedOptionsVM;
    public MainVM() : this(new AdvancedOptionsVM()) {}



    [ObservableProperty]
    public string? sourceDisplay;

    public LevelFolder? SourceLevelFolder { get; set; }
    
    public event AsyncEventHandler<DisplayGeneralErrorArgs>? InvalidSource;

    [RelayCommand]
    private async Task SetSource(string? path)
    {
        _logger.Info("Setting source level to {path}...", path);

        if (path == null)
        {
            _logger.Error("No path provided.");
            await AEHHelper.RunAEH(InvalidSource, this, new("There is no path provided.", null));
            return;
        }


        _logger.Info("Loading source level folder {path}...", path);
        try
        {
            SourceLevelFolder = new LevelFolder(path);
            SourceDisplay = path;
        }
        catch (Exception ex) when (ErrorHelper.IsFileException(ex))
        {
            _logger.Error(ex, "Source level folder {path} cannot be used.", path);
            await AEHHelper.RunAEH(InvalidSource, this, new(
                "The source folder cannot be used. ",
                ex
            ));

            return;
        }
        catch (JsonException ex)
        {
            _logger.Error(ex, "Failed trying to load JSON.");
            await AEHHelper.RunAEH(InvalidSource, this, new(
                "The JSON of a file inside the folder failed to load.", ex
            ));

            return;
        }
    }



    public ObservableCollection<LevelFileInfo> LevelList { get; } = [];
    

    public event AsyncEventHandler<DisplayGeneralErrorArgs>? InvalidLevel;

    [RelayCommand]
    private async Task AddLevels(IReadOnlyList<IStorageFile> files)
    {
        _logger.Info("Adding levels {files}...", files.Select(x => x.Path));

        foreach (var file in files)
        {
            var path = file.Path.LocalPath;
            
            try
            {
                var level = Level.FromFile(path);
                LevelList.Add(new LevelFileInfo { LevelProp = level, Path = path });
            }
            catch (Exception ex) when (ErrorHelper.IsFileException(ex))
            {
                _logger.Warn(ex, "Level file {filePath} cannot be accessed or doesn't exist. Skipping...", file.Path);
                await AEHHelper.RunAEH(InvalidSource, this, new(
                    $"The level \"{file.Path}\" cannot be accessed or doesn't exist. ",
                    ex
                ));

                continue;
            }
            catch (JsonException ex)
            {
                _logger.Warn(ex, "Failed trying to load JSON of file {filePath}.", file.Path);
                await AEHHelper.RunAEH(InvalidSource, this, new(
                    $"The level \"{file.Path}\" has corrupt JSON and failed to load.", ex
                ));

                continue;
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, "The file {filePath} failed to load. Skipping...", file.Path);
                await AEHHelper.RunAEH(InvalidSource, this, new(
                    $"The level \"{file.Path}\" failed to load.", ex
                ));

                continue;
            }
        }
    }


    public List<LevelFileInfo> SelectedItems { get; set; } = [];

    [RelayCommand]
    private void DeleteLevels()
    {
        var copy = new List<LevelFileInfo>(SelectedItems);
        foreach (var item in copy)
            LevelList.Remove(item);
    }



    [ObservableProperty]
    public string? outputFolderPath = null;


    public event AsyncEventHandler<DisplayGeneralErrorArgs>? InvalidOutputFolderPath;

    [RelayCommand]
    private async Task SetOutputFolderPath(string? path)
    {
        _logger.Info("Setting output folder path to {path}...", path);

        if (path == null)
        {
            _logger.Error("No path provided.");
            await AEHHelper.RunAEH(InvalidSource, this, new("There is no path provided.", null));
            return;
        }

        OutputFolderPath = path;
    }




    public event AsyncEventHandler<List<string>>? MissingFields;

    public async Task<bool> AreFieldsFilled()
    {
        List<string> missingFields = [];
        if (SourceLevelFolder == null) missingFields.Add("Source Level");
        if (LevelList.Count == 0) missingFields.Add("Level List");
        if (advancedOptionsVM.ThemeFiles.Count == 0) missingFields.Add("Themes (likely no set loaded themes)");
        if (OutputFolderPath == null) missingFields.Add("Output Folder");

        if (missingFields.Count > 0)
        {
            if (MissingFields != null)
                await MissingFields(this, missingFields);
            return false;
        }

        return true;
    }
}
