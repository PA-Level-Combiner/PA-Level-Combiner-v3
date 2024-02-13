using System;
using System.Collections.Generic;
using System.Linq;

using PALC.Main.Models.Combiners._20_4_4.LevelComponents;

namespace PALC.Main.Models.Combiners._20_4_4;

public class IncludeOptions
{
    public bool BeatmapObjects { get; set; } = true;
    public bool Prefabs { get; set; } = true;
    public bool Markers { get; set; } = true;
    public bool Checkpoints { get; set; } = true;
    public bool EventKeyframes { get; set; } = true;
    public bool BgObjects { get; set; } = true;
}


public class _20_4_4_Combiner(LevelFolder levelFolderSource, IncludeOptions include_options) : Combiner<LevelFolder, Level>
{
    private static readonly Random rnd = new();
    private static readonly List<string> idChars = [
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
        "~", "!", "@", "#", "$", "%", "^", "&", "*", "_", "+", "{", "}", "|", ":", "<", ">", "?", ",", ".", "/", ";", "'", "[", "]",
        "▓", "▒", "░", "▐", "▆", "▉",
        "☰", "☱", "☲", "☳", "☴", "☵", "☶", "☷",
        "►", "▼", "◄",
        "▬", "▩", "▨", "▧", "▦", "▥", "▤", "▣", "▢", "□", "■", "¤",
        "ÿ", "ò", "è", "µ", "¶", "™", "ß", "Ã", "®", "¾", "ð", "¥", "œ", "⁕",
        "(", "◠", "‿", "◠", "✿", ")"
    ];


    private static readonly Version _PA_Version = Versions._20_4_4;
    public override Version GetPAVersion() => _PA_Version;

    public IncludeOptions include_options = include_options;
    public LevelFolder levelFolderSource = levelFolderSource;

    public override LevelFolder Combine(List<Level> levels)
    {
        LevelFolder combined = levelFolderSource;

        combined.level.prefabs ??= [];
        combined.level.prefab_objects ??= [];
        combined.level.ed.markers ??= [];


        static void AddRangeIgnoreFirst<T>(List<T> from, List<T> to)
        {
            from.AddRange(to.Skip(1).ToList());
        }

        foreach (var level in levels)
        {
            if (include_options.BeatmapObjects)
                combined.level.beatmap_objects.AddRange(level.beatmap_objects);
            
            if (include_options.Prefabs)
            {
                combined.level.prefabs.AddRange(level.prefabs ?? []);
                combined.level.prefab_objects.AddRange(level.prefab_objects ?? []);
            }

            if (include_options.Markers)
                combined.level.ed.markers.AddRange(level.ed.markers ?? []);

            if (include_options.Checkpoints)
                AddRangeIgnoreFirst(combined.level.checkpoints, level.checkpoints);

            if (include_options.EventKeyframes)
            {
                // TODO goodness this is horrible
                AddRangeIgnoreFirst(combined.level.events.pos, level.events.pos);
                AddRangeIgnoreFirst(combined.level.events.zoom, level.events.zoom);
                AddRangeIgnoreFirst(combined.level.events.rot, level.events.rot);
                AddRangeIgnoreFirst(combined.level.events.shake, level.events.shake);
                AddRangeIgnoreFirst(combined.level.events.theme, level.events.theme);
                AddRangeIgnoreFirst(combined.level.events.chroma, level.events.chroma);
                AddRangeIgnoreFirst(combined.level.events.bloom, level.events.bloom);
                AddRangeIgnoreFirst(combined.level.events.vignette, level.events.vignette);
                AddRangeIgnoreFirst(combined.level.events.lens, level.events.lens);
                AddRangeIgnoreFirst(combined.level.events.grain, level.events.grain);
            }
        }

        if (combined.level.prefabs.Count == 0) combined.level.prefabs = null;
        if (combined.level.prefab_objects.Count == 0) combined.level.prefab_objects = null;
        if (combined.level.ed.markers.Count == 0) combined.level.ed.markers = null;

        // regenerate ids
        foreach (var obj in combined.level.beatmap_objects)
        {
            List<string> newIdChars = [];
            for (int i = 0; i < 16; i++)
                newIdChars.Add(idChars[rnd.Next(idChars.Count)]);

            obj["id"] = string.Join("", newIdChars);
        }


        return combined;
    }
}
