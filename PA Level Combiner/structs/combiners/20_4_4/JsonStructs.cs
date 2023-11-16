using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable IDE1006
namespace PA_Level_Combiner.structs.combiners._20_4_4.json_struct
{
    using ObjectDict = Dictionary<string, object>;

    namespace level
    {
        public class Root
        {
            public required Ed ed;
            public required ObjectDict level_data;
            public required List<ObjectDict>
                themes,
                checkpoints,
                beatmap_objects,
                bg_objects;

            public required Events events;
        }

        public class Ed
        {
            public required string timeline_pos;
            public List<ObjectDict>? markers;
        }

        public class Events
        {
            public required List<ObjectDict>
                pos, zoom, rot, shake,
                chroma, bloom,
                vignette, lens, grain;

            public required List<EventTheme> theme;
        }

        public class EventTheme
        {
            public required string t, x;
        }
    }
    public class Level : level.Root { }

    namespace theme
    {
        public class Rootobject
        {
            public required string id { get; set; }
            public required string name { get; set; }
            public required string gui { get; set; }
            public required string bg { get; set; }
            public required List<string> players { get; set; }
            public required List<string> objs { get; set; }
            public required List<string> bgs { get; set; }
        }

    }
    public class Theme : theme.Rootobject { }

    namespace metadata
    {
        public class Rootobject
        {
            public required Artist artist { get; set; }
            public required Creator creator { get; set; }
            public required Song song { get; set; }
            public required Beatmap beatmap { get; set; }
        }

        public class Artist
        {
            public required string name { get; set; }
            public required string link { get; set; }
            public required string linkType { get; set; }
        }

        public class Creator
        {
            public required string steam_name { get; set; }
            public required string steam_id { get; set; }
        }

        public class Song
        {
            public required string title { get; set; }
            public required string difficulty { get; set; }
            public required string description { get; set; }
            public required string bpm { get; set; }
            public required string t { get; set; }
            public required string preview_start { get; set; }
            public required string preview_length { get; set; }
        }

        public class Beatmap
        {
            public required string date_edited { get; set; }
            public required string version_number { get; set; }
            public required string game_version { get; set; }
            public required string workshop_id { get; set; }
        }

    }
    public class Metadata : metadata.Rootobject { }
}
