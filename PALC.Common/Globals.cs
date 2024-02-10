using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace PALC;

public delegate Task AsyncEventHandler<TEventArgs>(object? sender, TEventArgs e);


public static class Globals
{
    public static readonly string PALCVersion =
        FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion?.Split("+")[0]
        ?? "Unknown version?? wtf";

    public static readonly string githubLink = @"https://github.com/TNTzx/PA-Level-Combiner-v3";
    public static readonly string githubIssuesLink = $"{githubLink}/issues";

    public static readonly string logsPath = $"{AppDomain.CurrentDomain.BaseDirectory}logs";
}
