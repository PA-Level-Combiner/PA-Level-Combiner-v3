using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PALC.Models.Combiners._20_4_4.Tests
{
    public static class LevelPaths
    {
        public static string GetLevelPath(string levelFolderName)
            => Path.Combine(Paths.levelsPath, "20_4_4_combiner_" + levelFolderName);

        public static readonly string normal_input_first = GetLevelPath("normal_input_first");
        public static readonly string normal_input_second = GetLevelPath("normal_input_second");
        public static readonly string normal_output = GetLevelPath("normal_output");
        public static readonly string noprefab_input_first = GetLevelPath("noprefab_input_first");
        public static readonly string noprefab_input_second = GetLevelPath("noprefab_input_second");
        public static readonly string noprefab_output = GetLevelPath("noprefab_output");

        public static readonly string blank = GetLevelPath("blank");
    }



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
                Path.Combine(levelFolderPath, "level.ogg"),
                levelFolder.audioPath
            );

            // Themes
            var themes = levelFolder.level.GetThemes(Paths.themesPath);
            Assert.AreEqual(expectedThemeCount, themes.Count);

            if (expectedThemeCount > 0)
            {
                string rawTheme = File.ReadAllText(Defaults.defaultThemePath);
                string loadedTheme = themes[0].ToFileJson();
                Assert.AreEqual(rawTheme, loadedTheme);
            }

            // Metadata
            string rawMetadata = File.ReadAllText(Path.Combine(levelFolderPath, "metadata.lsb"));
            string loadedMetadata = levelFolder.metadata.ToFileJson();
            Assert.AreEqual(rawMetadata, loadedMetadata);
        }

        [TestMethod()]
        public void LevelFolderTest_Normal()
        {
            CompareLevelFolderToSerialized(LevelPaths.normal_output, 1);
        }

        [TestMethod()]
        public void LevelFolderTest_NoPrefab()
        {
            CompareLevelFolderToSerialized(LevelPaths.noprefab_output, 1);
        }

        [TestMethod()]
        public void LevelFolderTest_Blank()
        {
            CompareLevelFolderToSerialized(LevelPaths.blank, 0);
        }
    }
}