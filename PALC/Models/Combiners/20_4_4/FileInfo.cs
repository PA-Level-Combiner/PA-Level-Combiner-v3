using PALC.Models.Combiners._20_4_4.LevelComponents;

namespace PALC.Models.Combiners._20_4_4;

public class LevelFileInfo
{
    public required Level LevelProp { get; set; }
    public required string Path { get; set; }
}

public class ThemeFileInfo
{
    public required Theme ThemeProp { get; set; }
    public required string Path { get; set; }
}
