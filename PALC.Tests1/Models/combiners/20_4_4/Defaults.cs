using System.Text.Json;

namespace PALC.Models.Combiners._20_4_4.Tests
{
    public static class Defaults
    {
        public static readonly string defaultThemePath = Path.Combine(Paths.themesPath, "cycle_h.lst");
        public static readonly LevelComponents.Theme defaultTheme = JsonSerializer.Deserialize<LevelComponents.Theme>(
            File.ReadAllText(defaultThemePath)
        ) ?? throw new JsonException();
    }
}
