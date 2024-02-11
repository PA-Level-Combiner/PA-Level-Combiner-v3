using System;
using System.Diagnostics;
using System.Reflection;

namespace PALC.Main;


public static class Globals
{
    public static readonly string PALCVersion =
        FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion?.Split("+")[0]
        ?? "Unknown version?? wtf";

    public static readonly string githubLink = @"https://github.com/PA-Level-Combiner/PA-Level-Combiner-v3";
    public static readonly string githubIssuesLink = $"{githubLink}/issues";
    public static readonly string githubReleasesLink = "${githubLink}/releases";
    public static readonly long githubID = 718512825;

    public static readonly string logsPath = $"{AppDomain.CurrentDomain.BaseDirectory}logs";
}
