using Avalonia.Controls.Primitives;
using PALC.Main.Models.Combiners._20_4_4.LevelComponents;
using System;

namespace PALC.Main.Models.Combiners._20_4_4.Exceptions
{
    public class ThemeInFolderNotFoundException(string themeId, Exception? inner = null)
        : Exception($"The theme ID {themeId} cannot be found.", inner)
    { }

    public class ThemeDuplicateException(string path1, string path2, string themeId, Exception? inner = null)
        : Exception($"Theme {path1} and theme with path {path2} shares a duplicate ID {themeId}.", inner)
    {
        public readonly string path1 = path1;
        public readonly string path2 = path2;
        public readonly string themeId = themeId;
    }

    public class JsonLoadException(string path, Exception? inner = null)
        : Exception($"JSON failed to load for path \"{path}\".{(inner != null ? $" {inner.Message}" : "")}", inner)
    {
        public readonly string path = path;
    }
}
