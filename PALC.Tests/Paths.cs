using Microsoft.VisualStudio.TestTools.UnitTesting;
using PALC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PALC
{
    public static class Paths
    {
        public static readonly string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
        public static readonly string projectPath = exePath[..exePath.IndexOf(@"bin\Debug\net8.0-windows")];

        public static readonly string assetsPath = Path.Combine(projectPath, "assets");
        public static readonly string emptyPath = Path.Combine(assetsPath, "empty");
        public static readonly string levelsPath = Path.Combine(assetsPath, "levels");
        public static readonly string themesPath = Path.Combine(assetsPath, "themes");
    }
}