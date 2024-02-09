using System.Text.Json;

using PALC.Main.Models.Combiners._20_4_4.LevelComponents;

namespace PALC.Main.Models.Combiners._20_4_4.Tests;

public static class Defaults
{
    public static readonly string defaultThemePath = Path.Combine(Paths.themesPath, "cycle_h.lst");
    public static readonly Theme defaultTheme = JsonSerializer.Deserialize<Theme>(
        File.ReadAllText(defaultThemePath)
    ) ?? throw new JsonException();
}
