using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace PALC.Main;

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
