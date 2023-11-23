using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PALC.Models.combiners._20_4_4.Tests
{
    [TestClass()]
    public class CombinerTests
    {
        [TestMethod()]
        public void CombineTest_Normal()
        {
            var level1 = new LevelFolder(LevelPaths.normal_input_first, Paths.themesPath);
            var level2 = new LevelFolder(LevelPaths.normal_input_second, Paths.themesPath);

            var output = new _20_4_4_Combiner(new IncludeOptions()).Combine(new List<LevelFolder> { level1, level2 });

            Assert.AreEqual(level1.audioPath, output.audioPath);


            // This unit test only checks for length, not the objects itself.
            // Should probably fix this
            // TODO
            {
                static void AssertList<T>(List<T> list, int expectedCount)
                {
                    Assert.AreEqual(expectedCount, list.Count);
                }

                static void AssertOptionalList<T>(List<T>? list, int expectedCount)
                {
                    Assert.IsNotNull(list);
                    AssertList(list, expectedCount);
                }

                AssertOptionalList(output.level.prefabs, 2);
                AssertOptionalList(output.level.prefab_objects, 2);
                AssertOptionalList(output.level.ed.markers, 2);
                AssertList(output.level.beatmap_objects, 2);
                AssertList(output.level.checkpoints, 3);

                var events = output.level.events;

                List<List<Dictionary<string, object>>> eventKfsNoTheme = new()
                {
                    events.pos, events.zoom, events.rot, events.shake,
                    events.chroma, events.bloom, events.vignette, events.lens, events.grain
                };

                foreach (var eventKfs in eventKfsNoTheme)
                    AssertList(eventKfs, 3);

                AssertList(events.theme, 3);
            }


            Assert.AreEqual(1, output.themes.Count);

            string rawTheme = File.ReadAllText(Defaults.defaultThemePath);
            string loadedTheme = output.themes[0].ToFileJson();
            Assert.AreEqual(rawTheme, loadedTheme);
        }
    }
}