using Avalonia.Controls.Selection;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PALC.Models.Combiners._20_4_4;
using PALC.Models.Combiners._20_4_4.LevelComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PALC.ViewModels.Combiners._20_4_4;


public partial class MainVM(AdvancedOptionsVM advancedOptionsVM) : ViewModelBase
{
    public AdvancedOptionsVM advancedOptionsVM = advancedOptionsVM;
    public MainVM() : this(new AdvancedOptionsVM()) {}



    [ObservableProperty]
    public string? sourceDisplay;

    public LevelFolder? SourceLevelFolder { get; set; }
    
    public class InvalidSourceArgs
    {
        public required Exception ex;
    }
    public event AsyncEventHandler<InvalidSourceArgs>? InvalidSource;

    [RelayCommand]
    private async Task SetSource(string? path)
    {
        if (path == null)
        {
            if (InvalidSource != null)
                await InvalidSource(this, new InvalidSourceArgs { ex = new Exception("No path provided.") });
            return;
        }

        try
        {
            SourceLevelFolder = new LevelFolder(path);
            SourceDisplay = path;
        }
        catch (Exception ex)
        {
            if (InvalidSource != null)
                await InvalidSource(this, new InvalidSourceArgs { ex = ex });

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
