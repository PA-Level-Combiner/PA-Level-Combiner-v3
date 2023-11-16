using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace PA_Level_Combiner.structs.combiners._20_4_4.Tests
{
    [TestClass()]
    public class ThemeFolderOpenerTests
    {
        [TestMethod()]
        public void GetThemesTest_Normal()
        {
            List<json_struct.Theme> themes = ThemeFolderOpener.GetThemes(Paths.themesPath);

            Assert.AreEqual(themes.Count, 1);
            
            Assert.AreEqual(
                JsonSerializer.Serialize(themes[0]),
                JsonSerializer.Serialize(Defaults.cycleHitTheme)
            );
        }

        [TestMethod()]
        public void GetThemesTest_EmptyGet()
        {
            List<json_struct.Theme> themes = ThemeFolderOpener.GetThemes(Paths.emptyPath);

            Assert.AreEqual(themes.Count, 0);
        }

        [TestMethod()]
        public void GetThemesTest_Caching()
        {
            List<json_struct.Theme> themes1 = ThemeFolderOpener.GetThemes(Paths.themesPath);
            List<json_struct.Theme> themes2 = ThemeFolderOpener.GetThemes(Paths.themesPath);
            Assert.AreEqual(themes1, themes2);
        }

        [TestMethod()]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void GetThemesTest_InvalidPath()
        {
            ThemeFolderOpener.GetThemes(@"wawa");
        }
    }
}