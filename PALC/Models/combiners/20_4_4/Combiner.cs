using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Drawing;
using System.IO;

namespace PALC.Models.combiners._20_4_4
{
    public class IncludeOptions(
        bool beatmap_objects = true,
        bool prefabs = true,
        bool markers = true,
        bool checkpoints = true,
        bool event_keyframes = true,
        bool bg_objects = true
    )
    {
        public bool beatmap_objects = beatmap_objects;
        public bool prefabs = prefabs;
        public bool markers = markers;
        public bool checkpoints = checkpoints;
        public bool event_keyframes = event_keyframes;
        public bool bg_objects = bg_objects;
    }


    public class _20_4_4_Combiner(IncludeOptions include_options) : Combiner<LevelFolder>
    {
        private static readonly Version _PA_Version = Versions._20_4_4;
        public override Version GetPAVersion() =>_PA_Version;

        public IncludeOptions include_options = include_options;

        public override LevelFolder Combine(List<LevelFolder> levelFolders)
        {
            LevelFolder combined = levelFolders[0];

            combined.level.prefabs ??= [];
            combined.level.prefab_objects ??= [];
            combined.level.ed.markers ??= [];


            static void AddRangeIgnoreFirst<T>(List<T> from, List<T> to)
            {
                from.AddRange(to.Skip(1).ToList());
            }

            foreach (LevelFolder levelFolder in levelFolders.Skip(1).ToList())
            {
                if (this.include_options.beatmap_objects)
                    combined.level.beatmap_objects.AddRange(levelFolder.level.beatmap_objects);
                
                if (this.include_options.prefabs)
                {
                    combined.level.prefabs.AddRange(levelFolder.level.prefabs ?? []);
                    combined.level.prefab_objects.AddRange(levelFolder.level.prefab_objects ?? []);
                }

                if (this.include_options.markers)
                    combined.level.ed.markers.AddRange(levelFolder.level.ed.markers ?? []);

                if (this.include_options.checkpoints)
                    AddRangeIgnoreFirst(combined.level.checkpoints, levelFolder.level.checkpoints);

                if (this.include_options.event_keyframes)
                {
                    // TODO goodness this is horrible
                    AddRangeIgnoreFirst(combined.level.events.pos, levelFolder.level.events.pos);
                    AddRangeIgnoreFirst(combined.level.events.zoom, levelFolder.level.events.zoom);
                    AddRangeIgnoreFirst(combined.level.events.rot, levelFolder.level.events.rot);
                    AddRangeIgnoreFirst(combined.level.events.shake, levelFolder.level.events.shake);
                    AddRangeIgnoreFirst(combined.level.events.theme, levelFolder.level.events.theme);
                    AddRangeIgnoreFirst(combined.level.events.chroma, levelFolder.level.events.chroma);
                    AddRangeIgnoreFirst(combined.level.events.bloom, levelFolder.level.events.bloom);
                    AddRangeIgnoreFirst(combined.level.events.vignette, levelFolder.level.events.vignette);
                    AddRangeIgnoreFirst(combined.level.events.lens, levelFolder.level.events.lens);
                    AddRangeIgnoreFirst(combined.level.events.grain, levelFolder.level.events.grain);
                }
                
                foreach (json_struct.Theme theme in levelFolder.themes)
                {
                    if (!combined.themes.Contains(theme)) combined.themes.Add(theme);
                }
            }

            if (combined.level.prefabs.Count == 0) combined.level.prefabs = null;
            if (combined.level.prefab_objects.Count == 0) combined.level.prefab_objects = null;
            if (combined.level.ed.markers.Count == 0) combined.level.ed.markers = null;


            return combined;
        }
    }
}
