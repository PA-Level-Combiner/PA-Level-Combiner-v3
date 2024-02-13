using System;
using System.Diagnostics;
using System.Reflection;

namespace PALC.Main;


public static class Globals
{
    public static readonly string programName = "PA Level Combiner Main";

    public static readonly string githubLink = @"https://github.com/PA-Level-Combiner/PA-Level-Combiner-v3";
    public static readonly string githubIssuesLink = $"{githubLink}/issues";
    public static readonly string githubReleasesLink = "${githubLink}/releases";

    public static readonly string logsPath = $"{AppDomain.CurrentDomain.BaseDirectory}logs";
}
