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
        _logger.Debug("Setting source tp {path}...", path);

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
        catch (Exception ex) when (
            ex is UnauthorizedAccessException ||
            ex is PathTooLongException
        )
        {
            _logger.Error(ex, "Source level folder {path} cannot be accessed.", path);
            await AEHHelper.RunAEH(InvalidSource, this, new(
                "The source folder cannot be accessed. " + AdditionalErrors.noAccessHelp,
                ex
            ));

            return;
        }
        catch (DirectoryNotFoundException ex)
        {
            _logger.Error(ex, "Source level folder {path} doesn't exist.", path);
            await AEHHelper.RunAEH(InvalidSource, this, new(
                "The source folder doesn't exist. ", ex
            ));

            return;
        }
        catch (FileNotFoundException ex)
        {
            _logger.Error("File {exFile} from source level folder {path} not found.", ex.FileName, path);
            await AEHHelper.RunAEH(InvalidSource, this, new(
                $"Cannot find the file \"{ex.FileName}\". ", ex
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
    
    public class InvalidLevelArgs
    {
        public required Exception ex;
        public required string failedLevelPath;
    }
    public event AsyncEventHandler<InvalidLevelArgs>? InvalidLevel;

    [RelayCommand]
    private async Task AddLevels(IReadOnlyList<IStorageFile> files)
    {
        foreach (var file in files)
        {
            var path = file.Path.LocalPath;
            
            try
            {
                var level = Level.FromFile(path);
                LevelList.Add(new LevelFileInfo { LevelProp = level, Path = path });
            }
            catch (Exception ex)
            {
                if (InvalidLevel != null) await InvalidLevel(this, new InvalidLevelArgs { ex = ex, failedLevelPath = path});
                return;
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

    public class InvalidOutputFolderPathArgs
    {
        public required Exception ex;
    }
    public event AsyncEventHandler<InvalidOutputFolderPathArgs>? InvalidOutputFolderPath;

    [RelayCommand]
    private async Task SetOutputFolderPath(string? path)
    {
        if (path == null)
        {
            if (InvalidOutputFolderPath != null)
                await InvalidOutputFolderPath(this, new InvalidOutputFolderPathArgs { ex = new Exception("No path provided.") });
            return;
        }

        OutputFolderPath = path;
    }



    public class MissingFieldsArgs
    {
        public required List<string> fields;
    }
    public event AsyncEventHandler<MissingFieldsArgs>? MissingFields;

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
                await MissingFields(this, new MissingFieldsArgs { fields = missingFields });
            return false;
        }

        return true;
    }
}
