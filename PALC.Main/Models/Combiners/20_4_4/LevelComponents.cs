using System.Collections.Generic;
using System.Text;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;
using System.Linq;
using System;


#pragma warning disable IDE1006
namespace PALC.Main.Models.Combiners._20_4_4.LevelComponents
{
    using ObjectDict = Dictionary<string, object>;


    internal interface IJsonConverter<T>
    {
        public abstract static T FromFileJson(string json);
        public abstract static T FromFile(string path);

        public abstract string ToFileJson();
    }


    public static class ThemeFolderOpener
    {
        public static List<ThemeFileInfo> GetThemeFilesFromFolder(string themeFolderPath)
        {
            List<string> themePaths = [
                .. Directory.GetFiles(
                    themeFolderPath, "*.lst", SearchOption.TopDirectoryOnly
                ),
            ];

            Dictionary<string, ThemeFileInfo> themeFileInfos = [];
            foreach (string themePath in themePaths)
            {
                var theme = Theme.FromFile(themePath);
                if (themeFileInfos.TryGetValue(theme.EventThemeId, out ThemeFileInfo? duplicate))
                    throw new ArgumentException($"Theme {themePath} and theme with path {duplicate.Path} shares a duplicate ID {theme.id}.");

                themeFileInfos.Add(theme.EventThemeId, new ThemeFileInfo { ThemeProp = theme, Path = themePath });
            }

            return [.. themeFileInfos.Values];
        }


        public static List<Theme> GetThemesFromIds(List<string> eventThemeIds, List<Theme> themes)
        {
            Dictionary<string, Theme> folderThemeDict = [];

            foreach (var theme in themes)
            {
                folderThemeDict.Add(theme.EventThemeId, theme);
            }

            List<Theme> output = [];
            foreach (var themeId in eventThemeIds)
            {
                if (!folderThemeDict.TryGetValue(themeId, out Theme? theme))
                {
                    throw new exceptions.ThemeInFolderNotFoundException(themeId);
                }

                output.Add(theme ?? throw new Exception());
            }

            return output;
        }


        public static List<Theme> GetThemesFromIds(List<string> eventThemeIds, string themeFolderPath)
        {
            var folderThemes = GetThemeFilesFromFolder(themeFolderPath);
            return GetThemesFromIds(eventThemeIds, folderThemes.Select(x => x.ThemeProp).ToList());
        }


        public static List<string> FilterDefaultIds(List<string> ids)
        {
            // TODO verify default themes
            return ids.Where(id => int.Parse(id) > 9).ToList();
        }
    }



    namespace level
    {
        public class Root
        {
            [JsonProperty(Order = 0)]
            public required Ed ed { get; set; }

            [JsonProperty(Order = 1)]
            public List<ObjectDict>? prefab_objects { get; set; }

            [JsonProperty(Order = 2)]
            public required ObjectDict level_data { get; set; }

            [JsonProperty(Order = 3)]
            public List<ObjectDict>? prefabs { get; set; }

            [JsonProperty(Order = 4)]
            public required List<ObjectDict> themes { get; set; }

            [JsonProperty(Order = 5)]
            public required List<ObjectDict> checkpoints { get; set; }

            [JsonProperty(Order = 6)]
            public required List<ObjectDict> beatmap_objects { get; set; }

            [JsonProperty(Order = 7)]
            public required List<ObjectDict> bg_objects { get; set; }

            [JsonProperty(Order = 8)]
            public required Events events { get; set; }
        }

        public class Ed
        {
            [JsonProperty(Order = 0)]
            public required string timeline_pos { get; set; }

            [JsonProperty(Order = 1)]
            public List<ObjectDict>? markers { get; set; }
        }

        public class Events
        {
            [JsonProperty(Order = 0)]
            public required List<ObjectDict> pos { get; set; }

            [JsonProperty(Order = 1)]
            public required List<ObjectDict> zoom { get; set; }

            [JsonProperty(Order = 2)]
            public required List<ObjectDict> rot { get; set; }

            [JsonProperty(Order = 3)]
            public required List<ObjectDict> shake { get; set; }

            [JsonProperty(Order = 4)]
            public required List<EventTheme> theme { get; set; }

            [JsonProperty(Order = 5)]
            public required List<ObjectDict> chroma { get; set; }

            [JsonProperty(Order = 6)]
            public required List<ObjectDict> bloom { get; set; }

            [JsonProperty(Order = 7)]
            public required List<ObjectDict> vignette { get; set; }

            [JsonProperty(Order = 8)]
            public required List<ObjectDict> lens { get; set; }

            [JsonProperty(Order = 9)]
            public required List<ObjectDict> grain { get; set; }
        }

        public class EventTheme
        {
            [JsonProperty(Order = 0)]
            public required string t { get; set; }

            [JsonProperty(Order = 1)]
            public required string x { get; set; }
        }
    }
    public class Level : level.Root, IJsonConverter<Level> {
        public static readonly string defaultFileName = "level.lsb";


        public static Level FromFileJson(string json)
            => JsonConvert.DeserializeObject<Level>(json)
                ?? throw new JsonException();

        public static Level FromFile(string path)
            => FromFileJson(File.ReadAllText(path));

        public string ToFileJson()
            => JsonConvert.SerializeObject(this, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

        public List<string> GetThemeIdsUsed() => events.theme.Select(data => data.x).ToList();
        public List<string> GetCustomThemeIdsUsed() => ThemeFolderOpener.FilterDefaultIds(GetThemeIdsUsed());

        public List<Theme> GetThemes(string themeFolderPath)
            => ThemeFolderOpener.GetThemesFromIds(GetCustomThemeIdsUsed(), themeFolderPath);

        public List<Theme> FilterThemes(List<Theme> themes)
            => ThemeFolderOpener.GetThemesFromIds(GetCustomThemeIdsUsed(), themes);
    }

    namespace theme
    {
        public class Rootobject
        {
            [JsonProperty(Order = 0)]
            public required string id { get; set; }

            [JsonProperty(Order = 1)]
            public required string name { get; set; }

            [JsonProperty(Order = 2)]
            public required string gui { get; set; }

            [JsonProperty(Order = 3)]
            public required string bg { get; set; }

            [JsonProperty(Order = 4)]
            public required List<string> players { get; set; }

            [JsonProperty(Order = 5)]
            public required List<string> objs { get; set; }

            [JsonProperty(Order = 6)]
            public required List<string> bgs { get; set; }
        }

    }
    public partial class Theme : theme.Rootobject, IJsonConverter<Theme> {
        [GeneratedRegex(@"( +)(""[a-z]+"")(:) (.+)")]
        private static partial Regex noSpaceBeforeColonRegex();


        public static Theme FromFileJson(string json)
            => JsonConvert.DeserializeObject<Theme>(json) ?? throw new JsonException();

        public static Theme FromFile(string path)
            => FromFileJson(File.ReadAllText(path));

        public string ToFileJson()
        {
            string output;

            var sb = new StringBuilder();
            using (var ss = new StringWriter(sb))
            using (var writer = new JsonTextWriter(ss)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 3,
                    IndentChar = ' ',
                }
            )
            {
                new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore }.Serialize(writer, this);
                output = ss.ToString();
            }

            output = output.Replace("\r", "");

            foreach (string toReplace in new List<string> { "{\n", "[\n", ",\n" })
            {
                output = output.Replace(toReplace, toReplace[0] + " " + toReplace[1..]);
            }

            output = noSpaceBeforeColonRegex().Replace(output, "$1$2 $3 $4");

            return output;
        }

        [JsonIgnore]
        public string EventThemeId { get => int.Parse(id).ToString(); }
    }

    namespace metadata
    {
        public class Rootobject
        {
            [JsonProperty(Order = 0)]
            public required Artist artist { get; set; }

            [JsonProperty(Order = 1)]
            public required Creator creator { get; set; }

            [JsonProperty(Order = 2)]
            public required Song song { get; set; }

            [JsonProperty(Order = 3)]
            public required Beatmap beatmap { get; set; }
        }

        public class Artist
        {
            [JsonProperty(Order = 0)]
            public required string name { get; set; }

            [JsonProperty(Order = 1)]
            public required string link { get; set; }

            [JsonProperty(Order = 2)]
            public required string linkType { get; set; }
        }

        public class Creator
        {
            [JsonProperty(Order = 0)]
            public required string steam_name { get; set; }

            [JsonProperty(Order = 1)]
            public required string steam_id { get; set; }
        }

        public class Song
        {
            [JsonProperty(Order = 0)]
            public required string title { get; set; }

            [JsonProperty(Order = 1)]
            public required string difficulty { get; set; }

            [JsonProperty(Order = 2)]
            public required string description { get; set; }

            [JsonProperty(Order = 3)]
            public required string bpm { get; set; }

            [JsonProperty(Order = 4)]
            public required string t { get; set; }

            [JsonProperty(Order = 5)]
            public required string preview_start { get; set; }

            [JsonProperty(Order = 6)]
            public required string preview_length { get; set; }
        }

        public class Beatmap
        {
            [JsonProperty(Order = 0)]
            public required string date_edited { get; set; }

            [JsonProperty(Order = 1)]
            public required string version_number { get; set; }

            [JsonProperty(Order = 2)]
            public required string game_version { get; set; }

            [JsonProperty(Order = 3)]
            public required string workshop_id { get; set; }
        }

    }
    public class Metadata : metadata.Rootobject, IJsonConverter<Metadata> {
        public static readonly string defaultFileName = "metadata.lsb";


        public static Metadata FromFileJson(string json)
            => JsonConvert.DeserializeObject<Metadata>(json) ?? throw new JsonException();

        public static Metadata FromFile(string path)
            => FromFileJson(File.ReadAllText(path));

        public string ToFileJson()
            => JsonConvert.SerializeObject(this);
    }


    public static class Audio
    {
        public static readonly string defaultFileName = "level.ogg";
    }
}
