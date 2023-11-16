using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PA_Level_Combiner.structs.combiners._20_4_4
{
    public static class ThemeFolderOpener
    {
        private static readonly Dictionary<string, List<json_struct.Theme>> recorded_calls = [];

        public static List<json_struct.Theme> GetThemes(string themeFolderPath)
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
                themePath => JsonSerializer.Deserialize<json_struct.Theme>(
                    File.ReadAllText(themePath)
                ) ?? throw new NullReferenceException()
            ).ToList();
            recorded_calls.Add(themeFolderPath, themes);
            return themes;
        }
    }


    public class LevelFolder
    {
        public static readonly JsonSerializerOptions noNullJsonOption = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };


        public json_struct.Level level;
        public string audio_path;
        public List<json_struct.Theme> themes;

        public LevelFolder(string level_folder_path, string theme_folder_path)
        {
            string level_data_str = File.ReadAllText(
                Path.Combine(level_folder_path, "level.lsb")
            );

            this.level = JsonSerializer.Deserialize<json_struct.Level>(level_data_str)
                ?? throw new JsonException("Level data is not shaped correctly.");

            this.audio_path = Path.Combine(level_folder_path, "level.ogg");
            if (!File.Exists(audio_path))
                throw new exceptions.AudioFileNotFoundException();


            var theme_ids_in_data = this.level.events.theme.Select(data => data.x).ToList();
            var unverified_theme_paths = ThemeFolderOpener.GetThemes(theme_folder_path);

            this.themes = [];
            foreach (var theme in unverified_theme_paths)
            {
                if (theme_ids_in_data.Contains(theme.id))
                    this.themes.Add(theme);
            }
        }


        public string SerializeLevelJson()
            => JsonSerializer.Serialize(this.level, noNullJsonOption);
    }
}
