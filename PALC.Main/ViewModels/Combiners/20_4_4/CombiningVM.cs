using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Media;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using NLog;
using PALC.Main.Models.Combiners._20_4_4;
using PALC.Main.Models.Combiners._20_4_4.LevelComponents;

namespace PALC.Main.ViewModels.Combiners._20_4_4;

public partial class CombiningVM : ViewModelBase
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();


    public CombiningVM(
        LevelFolder? sourceLevelFolder = null,
        List<LevelFileInfo>? levelFiles = null,
        IncludeOptions? includeOptions = null,
        List<ThemeFileInfo>? themeFiles = null,
        string? outputFolderPath = null
    )
    {
        this.sourceLevelFolder = sourceLevelFolder;
        this.levelFiles = levelFiles ?? [];
        this.includeOptions = includeOptions;
        this.themeFiles = themeFiles ?? [];
        this.outputFolderPath = outputFolderPath;

        CombineError += OnCombineError;
        Finished += OnFinished;
    }
    public CombiningVM() : this(null, null, null, null, null) { }

    public LevelFolder? sourceLevelFolder;
    public List<LevelFileInfo> levelFiles;
    public IncludeOptions? includeOptions;
    public List<ThemeFileInfo> themeFiles;
    public string? outputFolderPath;


    [ObservableProperty]
    public bool enableExit = false;


    public ObservableCollection<string> Logs { get; } = [];

    private void CreateLog(string log, LogLevel? severity = null)
    {
        severity ??= LogLevel.Info;
        _logger.Log(severity, log);
        Logs.Add($"[{DateTime.Now:hh:mm:ss}] [{severity.Name}] {log}");
    }

    private async Task OnCombineError(object? sender, DisplayGeneralErrorArgs e)
    {
        await Task.Run(() => CreateLog($"{e.message}\n\n{e.ex?.Message ?? ""}", LogLevel.Error));
        EnableExit = true;
    }

    private async Task OnFinished(object? sender, FinishedArgs e)
    {
        await Task.Run(() => CreateLog($"Finished! Took {e.length:hh\\:mm\\:ss}."));
        EnableExit = true;
    }



    public event AsyncEventHandler<DisplayGeneralErrorArgs>? CombineError;

    public class FinishedArgs
    {
        public required TimeSpan length;
    }
    public event AsyncEventHandler<FinishedArgs>? Finished;

    public async Task Combine()
    {
        _logger.Info("Combining levels...");

        try
        {
            LevelFolder sourceLevelFolder = this.sourceLevelFolder ?? throw new ArgumentNullException();
            IncludeOptions includeOptions = this.includeOptions ?? throw new ArgumentNullException();
            string outputFolderPath = this.outputFolderPath ?? throw new ArgumentNullException();
        }
        catch (ArgumentNullException ex)
        {
            _logger.Error(ex, "One or more fields are null.");
            await AEHHelper.RunAEH(CombineError, this, new(
                "One or more fields are null.",
                ex
            ));
            return;
        }


        DateTime start = DateTime.Now;
        _20_4_4_Combiner combiner = new(sourceLevelFolder, includeOptions);


        CreateLog("Combining:\n" + string.Join("\n", levelFiles.Select(x => $"\t{x.Path}")));
        LevelFolder combined;
        try
        {
            combined = combiner.Combine(levelFiles.Select(x => x.LevelProp).ToList());
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while trying to combine levels.");
            await AEHHelper.RunAEH(CombineError, this, new(
                "An error occurred during combining.",
                ex
            ));
            return;
        }
            


        /*
         * outputFolderPath
         * -- level
         * -- -- level.lsb
         * -- -- audio.lsb
         * -- -- metadata.lsb
         * -- themes
         * -- -- theme1.lst
         * -- -- ...
        */

        CreateLog("Creating folders...");
        string levelFolderPath = Path.Join(outputFolderPath, "level");
        string themesPath = Path.Join(outputFolderPath, "themes");

        foreach (var path in new List<string>([levelFolderPath, themesPath]))
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception ex) when (ErrorHelper.IsFileException(ex))
            {
                _logger.Error(ex, "Cannot create directory {directory}.", path);
                await AEHHelper.RunAEH(CombineError, this, new(
                    $"An error occurred while trying to create the folder \"{path}\".",
                    ex
                ));
                return;
            }
        }



        CreateLog("Writing all themes...");
        Dictionary<string, ThemeFileInfo> cache = [];


        foreach (var themeFile in themeFiles)
        {
            if (cache.TryGetValue(themeFile.ThemeProp.EventThemeId, out ThemeFileInfo? value))
            {
                var currentThemeFilename = Path.GetFileName(themeFile.Path);
                var conflictThemeFilename = Path.GetFileName(value.Path);
                CreateLog(
                    $"Theme files {currentThemeFilename} and {conflictThemeFilename} " +
                    $"have the same ID {themeFile.ThemeProp.EventThemeId}. Going to use {currentThemeFilename}...",
                    LogLevel.Warn
                );

                cache[themeFile.ThemeProp.EventThemeId] = themeFile;
                continue;
            }

            cache.Add(themeFile.ThemeProp.EventThemeId, themeFile);
        }


        var themes = combined.level.FilterThemes(themeFiles.Select(x => x.ThemeProp).ToList());
        foreach (var theme in themes)
        {
            if (!cache.ContainsKey(theme.EventThemeId))
            {
                CreateLog($"Theme ID {theme.EventThemeId} not found in theme folder. Skipping...", LogLevel.Warn);
                continue;
            }

            var themeFile = cache[theme.EventThemeId];
            var filename = Path.GetFileName(themeFile.Path);
            var filePath = Path.Combine(themesPath, filename);
            CreateLog($"Writing {filename}...");

            try
            {
                File.WriteAllText(filePath, theme.ToFileJson());
            }
            catch (Exception ex) when (ErrorHelper.IsFileException(ex))
            {
                _logger.Error(ex, "Cannot create theme {path}.", filePath);
                await AEHHelper.RunAEH(CombineError, this, new(
                    $"An error occurred while trying to create the theme \"{filePath}\".",
                    ex
                ));
                return;
            }
        }


        CreateLog("Writing level file...");
        string levelPath = Path.Combine(levelFolderPath, Level.defaultFileName);
        try
        {
            File.WriteAllText(levelPath, combined.level.ToFileJson());
        }
        catch (Exception ex) when (ErrorHelper.IsFileException(ex))
        {
            _logger.Error(ex, "Cannot create level file {path}.", levelPath);
            await AEHHelper.RunAEH(CombineError, this, new(
                $"An error occurred while trying to create the level file \"{levelPath}\".",
                ex
            ));
            return;
        }


        CreateLog("Writing metadata...");
        string metadataPath = Path.Combine(levelFolderPath, Metadata.defaultFileName);
        try
        {
            File.WriteAllText(metadataPath, combined.metadata.ToFileJson());
        }
        catch (Exception ex) when (ErrorHelper.IsFileException(ex))
        {
            _logger.Error(ex, "Cannot create metadata file {path}.", metadataPath);
            await AEHHelper.RunAEH(CombineError, this, new(
                $"An error occurred while trying to create the metadata file \"{metadataPath}\".",
                ex
            ));
            return;
        }


        CreateLog("Copying audio...");
        string audioPath = Path.Combine(levelFolderPath, Audio.defaultFileName);
        try
        {
            File.Copy(combined.audioPath, audioPath, true);
        }
        catch (Exception ex) when (ErrorHelper.IsFileException(ex))
        {
            _logger.Error(ex, "Cannot copy audio file from {source} to {dest}.", combined.audioPath, audioPath);
            await AEHHelper.RunAEH(CombineError, this, new(
                $"An error occurred while trying to copy the audio file from \"{combined.audioPath}\" to \"{audioPath}\".",
                ex
            ));
            return;
        }


        TimeSpan length = DateTime.Now - start;
        if (Finished != null)
            await Finished(this, new FinishedArgs { length = length });
    }
}
