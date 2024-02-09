using System.Reflection;

namespace PALC
{
    public static class Paths
    {
        public static readonly string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
        public static readonly string projectPath = exePath[..exePath.IndexOf(@"bin\Debug\net7.0")];

        public static readonly string assetsPath = Path.Combine(projectPath, "assets");
        public static readonly string emptyPath = Path.Combine(assetsPath, "empty");
        public static readonly string levelsPath = Path.Combine(assetsPath, "levels");
        public static readonly string themesPath = Path.Combine(assetsPath, "themes");
    }
}