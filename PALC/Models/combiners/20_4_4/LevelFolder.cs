using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace PALC.Models.combiners._20_4_4
{
    public static class ThemeFolderOpener
    {
        private static readonly Dictionary<string, List<json_struct.Theme>> recorded_calls = [];

        public static List<json_struct.Theme> GetAllThemes(string themeFolderPath)
        {
            if (recorded_calls.TryGetValue(themeFolderPath, out var value))
            {
                return value;
            }

            List<string> themePaths =
            [
                .. Directory.GetFiles(
                    themeFolderPath, "*.lst", SearchOption.TopDirectoryOnly
                ),
            ];
            List<json_struct.Theme> themes = themePaths.Select(
                themePath => json_struct.Theme.FromFileJson(
                    File.ReadAllText(themePath)
                ) ?? throw new NullReferenceException()
            ).ToList();
            recorded_calls.Add(themeFolderPath, themes);
            return themes;
        }


        public static List<json_struct.Theme> GetThemesFromIds(List<string> eventThemeIds, string themeFolderPath)
        {
            var folderThemes = GetAllThemes(themeFolderPath);
            Dictionary<string, json_struct.Theme> folderThemeDict = [];

            foreach (var theme in folderThemes) {
                folderThemeDict.Add(theme.EventThemeId, theme);
            }

            List<json_struct.Theme> output = [];
            foreach (var themeId in eventThemeIds)
            {
                if (!folderThemeDict.TryGetValue(themeId, out json_struct.Theme? theme))
                {
                    throw new exceptions.ThemeInFolderNotFoundException(themeId);
                }

                output.Add(theme ?? throw new Exception());
            }

            return output;
        }


        public static List<string> FilterDefaultIds(List<string> ids)
        {
            // TODO verify default themes
            return ids.Where(id => int.Parse(id) > 9).ToList();
        }
    }


    public class LevelFolder
    {
        public json_struct.Level level;
        public string audioPath;
        public List<json_struct.Theme> themes;
        public json_struct.Metadata metadata;

        public LevelFolder(string levelFolderPath, string themeFolderPath)
        {
            string levelJson = File.ReadAllText(
                Path.Combine(levelFolderPath, "level.lsb")
            );

            this.level = json_struct.Level.FromFileJson(levelJson);

            this.audioPath = Path.Combine(levelFolderPath, "level.ogg");
            if (!File.Exists(audioPath))
                throw new exceptions.AudioFileNotFoundException();


            var themeIdsInLevel = this.level.events.theme.Select(data => data.x).ToList();
            this.themes = ThemeFolderOpener.GetThemesFromIds(ThemeFolderOpener.FilterDefaultIds(themeIdsInLevel), themeFolderPath);

            this.metadata = json_struct.Metadata.FromFileJson(
                File.ReadAllText(Path.Combine(levelFolderPath, "metadata.lsb"))
            );
        }
    }
}
