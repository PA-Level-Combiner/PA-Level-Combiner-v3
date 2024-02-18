using PALC.Main.Models.Combiners._20_4_4.LevelComponents;
using System.Reflection;

namespace PALC.Main.Models.Combiners._20_4_4.Tests;

public static class Paths
{
    public static readonly string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
    public static readonly string projectPath = exePath[..exePath.IndexOf(@"bin\Debug\net7.0")];

    public static readonly string assetsPath = "assets\\20_4_4";
    public static readonly string levelsPath = Path.Combine(assetsPath, "levels");
    public static readonly string themesPath = Path.Combine(assetsPath, "themes");
}

public static class LevelPaths
{
    public static string GetLevelPath(string levelFolderName)
        => Path.Combine(Paths.levelsPath, "20_4_4_combiner_" + levelFolderName);

    public static readonly string input_first = GetLevelPath("input_first");
    public static readonly string input_second = GetLevelPath("input_second");
    public static readonly string output = GetLevelPath("output\\level");

    public static readonly string blank = GetLevelPath("blank");
}

public static class ThemePaths
{
    public static string GetThemePath(string themeFolderName)
        => Path.Combine(Paths.themesPath, themeFolderName);

    public static readonly string exists = GetThemePath("exists");
    public static readonly string invalid = GetThemePath("invalid");
}

public static class Defaults
{
    public static readonly string defaultThemePath = Path.Combine(Paths.themesPath, "exists", "cycle_h.lst");
    public static readonly Theme defaultTheme = Theme.FromFile(defaultThemePath);
}