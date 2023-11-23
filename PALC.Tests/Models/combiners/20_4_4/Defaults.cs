using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace PALC.Models.combiners._20_4_4.Tests
{
    public static class Defaults
    {
        public static readonly string defaultThemePath = Path.Combine(Paths.themesPath, "cycle_h.lst");
        public static readonly json_struct.Theme defaultTheme = JsonSerializer.Deserialize<json_struct.Theme>(
            File.ReadAllText(defaultThemePath)
        ) ?? throw new JsonException();
    }
}
