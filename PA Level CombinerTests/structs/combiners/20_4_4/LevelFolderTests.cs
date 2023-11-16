using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA_Level_Combiner.structs.combiners._20_4_4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PA_Level_Combiner.structs.combiners._20_4_4.Tests
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
    }



    [TestClass()]
    public class LevelFolderTests
    {
        [TestMethod()]
        public void LevelFolderTest_Normal()
        {
            LevelFolder levelFolder = new(LevelPaths.normal_input_first, Paths.themesPath);

            Assert.AreEqual(
                File.ReadAllText(Path.Combine(LevelPaths.normal_input_first, "level.lsb")),
                levelFolder.SerializeLevelJson()
            );

            Assert.AreEqual(
                Path.Combine(LevelPaths.normal_input_first, "level.ogg"),
                levelFolder.audio_path
            );

            Assert.AreEqual(
                JsonSerializer.Serialize<List<json_struct.Theme>>([Defaults.cycleHitTheme]),
                JsonSerializer.Serialize(levelFolder.themes)
            );
        }
    }
}