using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace PA_Level_Combiner.structs.combiners._20_4_4.Tests
{
    public static class Defaults
    {
        public static readonly json_struct.Theme cycleHitTheme = JsonSerializer.Deserialize<json_struct.Theme>(
            File.ReadAllText(Path.Combine(Paths.themesPath, "cycle_h.lst"))
        ) ?? throw new JsonException();
    }
}
