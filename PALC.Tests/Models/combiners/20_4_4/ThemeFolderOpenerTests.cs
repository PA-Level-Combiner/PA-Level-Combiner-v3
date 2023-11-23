using PALC.Models.combiners._20_4_4;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace PALC.Models.combiners._20_4_4.Tests
{
    [TestClass()]
    public class ThemeFolderOpenerTests
    {
        [TestMethod()]
        public void GetThemesTest_Normal()
        {
            List<json_struct.Theme> themes = ThemeFolderOpener.GetAllThemes(Paths.themesPath);

            Assert.AreEqual(themes.Count, 1);

            Assert.AreEqual(
                JsonSerializer.Serialize(themes[0]),
                JsonSerializer.Serialize(Defaults.defaultTheme)
            );
        }

        [TestMethod()]
        public void GetThemesTest_EmptyGet()
        {
            List<json_struct.Theme> themes = ThemeFolderOpener.GetAllThemes(Paths.emptyPath);

            Assert.AreEqual(themes.Count, 0);
        }

        [TestMethod()]
        public void GetThemesTest_Caching()
        {
            List<json_struct.Theme> themes1 = ThemeFolderOpener.GetAllThemes(Paths.themesPath);
            List<json_struct.Theme> themes2 = ThemeFolderOpener.GetAllThemes(Paths.themesPath);
            Assert.AreEqual(themes1, themes2);
        }

        [TestMethod()]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void GetThemesTest_InvalidPath()
        {
            ThemeFolderOpener.GetAllThemes(@"wawa");
        }



        [TestMethod()]
        public void GetThemesFromIdTest_Normal()
        {
            var themes = ThemeFolderOpener.GetThemesFromIds(new List<string>{ "80588" }, Paths.themesPath);

            Assert.IsTrue(themes.Count == 1);

            string rawTheme = File.ReadAllText(Defaults.defaultThemePath);
            string loadedTheme = themes[0].ToFileJson();
            Assert.AreEqual(rawTheme, loadedTheme);
        }
    }
}