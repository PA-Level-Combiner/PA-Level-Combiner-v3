using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PALC.Main.Models.Combiners._20_4_4.LevelComponents;

namespace PALC.Main.Models.Combiners._20_4_4.Tests;





[TestClass()]
public class LevelFolderTests
{
    private static void CompareLevelFolderToSerialized(string levelFolderPath, int expectedThemeCount)
    {
        LevelFolder levelFolder = new(levelFolderPath);

        // Level
        var rawLevel = File.ReadAllText(Path.Combine(levelFolderPath, "level.lsb"));
        var loadedLevel = levelFolder.level.ToFileJson();
        Assert.AreEqual(rawLevel, loadedLevel);

        // Audio
        Assert.AreEqual(
            Path.Combine(levelFolderPath, Audio.defaultFileName),
            levelFolder.audioPath
        );

        // Themes
        var themes = levelFolder.level.GetThemes(ThemePaths.exists);
        Assert.AreEqual(expectedThemeCount, themes.Count);

        if (expectedThemeCount > 0)
        {
            string rawTheme = File.ReadAllText(Defaults.defaultThemePath);
            string loadedTheme = themes[0].ToFileJson();
            Assert.AreEqual(rawTheme, loadedTheme);
        }

        // Metadata
        string rawMetadata = File.ReadAllText(Path.Combine(levelFolderPath, LevelComponents.Metadata.defaultFileName));
        string loadedMetadata = levelFolder.metadata.ToFileJson();
        Assert.AreEqual(rawMetadata, loadedMetadata);
    }

    [TestMethod()]
    public void LevelFolderTest_Normal()
    {
        CompareLevelFolderToSerialized(LevelPaths.output, 1);
    }

    [TestMethod()]
    public void LevelFolderTest_Blank()
    {
        CompareLevelFolderToSerialized(LevelPaths.blank, 0);
    }
}