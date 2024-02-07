﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

using PALC.Models.Combiners._20_4_4.LevelComponents;

namespace PALC.Models.Combiners._20_4_4.Tests
{
    [TestClass()]
    public class ThemeFolderOpenerTests
    {
        [TestMethod()]
        public void GetThemesTest_Normal()
        {
            List<Theme> themes = ThemeFolderOpener.GetThemeFilesFromFolder(Paths.themesPath).Select(x => x.ThemeProp).ToList();

            Assert.AreEqual(themes.Count, 1);

            Assert.AreEqual(
                JsonSerializer.Serialize(themes[0]),
                JsonSerializer.Serialize(Defaults.defaultTheme)
            );
        }

        [TestMethod()]
        public void GetThemesTest_EmptyGet()
        {
            List<Theme> themes = ThemeFolderOpener.GetThemeFilesFromFolder(Paths.emptyPath).Select(x => x.ThemeProp).ToList();

            Assert.AreEqual(themes.Count, 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void GetThemesTest_InvalidPath()
        {
            ThemeFolderOpener.GetThemeFilesFromFolder(@"wawa");
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