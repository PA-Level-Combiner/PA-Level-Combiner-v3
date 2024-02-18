using Microsoft.VisualStudio.TestTools.UnitTesting;

using PALC.Main.Models.Combiners._20_4_4.LevelComponents;


namespace PALC.Main.Models.Combiners._20_4_4.Tests;

[TestClass()]
public class CombinerTests
{
    [TestMethod()]
    public void CombineTest_Normal()
    {
        var level1 = new LevelFolder(LevelPaths.input_first);
        var level2 = Level.FromFile(Path.Combine(LevelPaths.input_second, "level.lsb"));

        var expectedOutput = new LevelFolder(LevelPaths.output);

        LevelFolder output = new _20_4_4_Combiner(level1, new IncludeOptions()).Combine(new List<Level> { level2 });

        Assert.AreEqual(level1.audioPath, output.audioPath);


        // This unit test only checks for length, not the objects itself.
        // Should probably fix this
        // TODO
        void AssertList<T>(Func<LevelFolder, List<T>?> listFunc)
        {
            Assert.AreEqual(listFunc(output)?.Count, listFunc(expectedOutput)?.Count);
        }

        void AssertOptionalList<T>(Func<LevelFolder, List<T>?> listFunc)
        {
            Assert.IsNotNull(listFunc(output));
            AssertList(listFunc);
        }

        AssertOptionalList(x => x.level.prefabs);
        AssertOptionalList(x => x.level.prefab_objects);
        AssertOptionalList(x => x.level.ed.markers);
        AssertList(x => x.level.beatmap_objects);
        AssertList(x => x.level.checkpoints);

        var events = output.level.events;

        AssertList(x => x.level.events.pos);
        AssertList(x => x.level.events.zoom);
        AssertList(x => x.level.events.rot);
        AssertList(x => x.level.events.shake);
        AssertList(x => x.level.events.theme);
        AssertList(x => x.level.events.chroma);
        AssertList(x => x.level.events.bloom);
        AssertList(x => x.level.events.vignette);
        AssertList(x => x.level.events.lens);
        AssertList(x => x.level.events.grain);



        var outputThemes = output.level.GetThemes(ThemePaths.exists);


        Assert.AreEqual(1, outputThemes.Count);

        string rawTheme = File.ReadAllText(Defaults.defaultThemePath);
        string loadedTheme = outputThemes[0].ToFileJson();
        Assert.AreEqual(rawTheme, loadedTheme);
    }
}