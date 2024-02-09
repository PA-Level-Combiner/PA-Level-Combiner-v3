using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Primitives;
using Avalonia.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using PALC.Main.Models.Combiners._20_4_4;
using PALC.Main.Models.Combiners._20_4_4.LevelComponents;
using PALC.Main.Models.Combiners._20_4_4.LevelComponents.level;

namespace PALC.Main.ViewModels.Combiners._20_4_4;

public partial class CombiningVM : ViewModelBase
{
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

    private void CreateLog(string log)
    {
        Logs.Add($"[{DateTime.Now:hh:mm:ss}] {log}");
    }

    private async Task OnCombineError(object? sender, CombineErrorArgs e)
    {
        await Task.Run(() => CreateLog($"{e.message}\n\n{e.ex?.Message ?? ""}"));
        EnableExit = true;
    }

    private async Task OnFinished(object? sender, FinishedArgs e)
    {
        await Task.Run(() => CreateLog($"Finished! Took {e.length:hh\\:mm\\:ss}."));
        EnableExit = true;
    }



    public class CombineErrorArgs
    {
        public required Exception? ex;
        public required string message;
    }
    public event AsyncEventHandler<CombineErrorArgs>? CombineError;

    public class FinishedArgs
    {
        public required TimeSpan length;
    }
    public event AsyncEventHandler<FinishedArgs>? Finished;

    public async Task Combine()
    {
        async Task RaiseCombineError(CombineErrorArgs args)
        {
            if (CombineError != null)
                await CombineError(this, args);
        }

        try
        {
            LevelFolder sourceLevelFolder = this.sourceLevelFolder ?? throw new ArgumentNullException();
            IncludeOptions includeOptions = this.includeOptions ?? throw new ArgumentNullException();
            string outputFolderPath = this.outputFolderPath ?? throw new ArgumentNullException();
        }
        catch (Exception ex)
        {
            await RaiseCombineError(new CombineErrorArgs { ex = ex, message = "One or more fields are null." });
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
            await RaiseCombineError(new CombineErrorArgs { ex = ex, message = "An error occurred during combining." });
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
        string levelPath = Path.Join(outputFolderPath, "level");
        string themesPath = Path.Join(outputFolderPath, "themes");

        foreach (var path in new List<string>([levelPath, themesPath]))
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                await RaiseCombineError(new CombineErrorArgs { ex = ex, message = "An error occurred while creating folders inside the output path." });
                return;
            }



        CreateLog("Writing all themes...");
        Dictionary<string, ThemeFileInfo> cache = [];

        try
        {
            foreach (var themeFile in themeFiles)
            {
                if (cache.TryGetValue(themeFile.ThemeProp.EventThemeId, out ThemeFileInfo? value))
                {
                    var currentThemeFilename = Path.GetFileName(themeFile.Path);
                    var conflictThemeFilename = Path.GetFileName(value.Path);
                    CreateLog(
                        $"Theme files {currentThemeFilename} and {conflictThemeFilename} " +
                        $"have the same ID {themeFile.ThemeProp.EventThemeId}. Going to use {currentThemeFilename}..."
                    );

                    cache[themeFile.ThemeProp.EventThemeId] = themeFile;
                    continue;
                }

                cache.Add(themeFile.ThemeProp.EventThemeId, themeFile);
            }
        }
        catch (Exception ex)
        {
            await RaiseCombineError(new CombineErrorArgs { ex = ex, message = "An error occurred while creating the theme cache." });
            return;
        }


        var themes = combined.level.FilterThemes(themeFiles.Select(x => x.ThemeProp).ToList());
        foreach (var theme in themes)
        {
            if (!cache.ContainsKey(theme.EventThemeId))
            {
                CreateLog($"Theme ID {theme.EventThemeId} not found in theme folder. Skipping...");
                continue;
            }

            var themeFile = cache[theme.EventThemeId];
            var filename = Path.GetFileName(themeFile.Path);
            CreateLog($"Writing {filename}...");

            try
            {
                File.WriteAllText(Path.Join(themesPath, filename), theme.ToFileJson());
            }
            catch (Exception ex)
            {
                await RaiseCombineError(new CombineErrorArgs { ex = ex, message = $"An error occurred while writing the theme {filename}." });
                return;
            }
        }


        CreateLog("Writing level file...");
        try
        {
            File.WriteAllText(Path.Join(levelPath, "level.lsb"), combined.level.ToFileJson());
        }
        catch (Exception ex)
        {
            await RaiseCombineError(new CombineErrorArgs { ex = ex, message = $"An error occurred while writing the level file." });
            return;
        }


        CreateLog("Writing metadata...");
        try
        {
            File.WriteAllText(Path.Join(levelPath, "metadata.lsb"), combined.metadata.ToFileJson());
        }
        catch (Exception ex)
        {
            await RaiseCombineError(new CombineErrorArgs { ex = ex, message = $"An error occurred while writing the metadata file." });
            return;
        }


        CreateLog("Copying audio...");
        try
        {
            File.Copy(combined.audioPath, Path.Join(levelPath, "audio.ogg"), true);
        }
        catch (Exception ex)
        {
            await RaiseCombineError(new CombineErrorArgs { ex = ex, message = $"An error occurred while trying to copying the audio." });
            return;
        }


        TimeSpan length = DateTime.Now - start;
        if (Finished != null)
            await Finished(this, new FinishedArgs { length = length });
    }
}
