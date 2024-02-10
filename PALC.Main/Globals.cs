using System.Diagnostics;
using System.Reflection;

namespace PALC.Main;


public static class Globals
{
    public static readonly string PALCVersion =
        FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion?.Split("+")[0]
        ?? "Unknown version?? wtf";
}
