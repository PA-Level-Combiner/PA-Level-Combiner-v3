using System;

namespace PALC.Main.Models.Combiners._20_4_4.exceptions
{
    public class ThemeInFolderNotFoundException(string themeId, Exception? inner) : Exception($"The theme bearing ID {themeId} cannot be found.", inner)
    {
        public ThemeInFolderNotFoundException(string themeId) : this(themeId, null) { }
    }
}
