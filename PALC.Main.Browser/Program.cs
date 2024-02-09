using System.Runtime.Versioning;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Browser;

using PALC.Main;

[assembly: SupportedOSPlatform("browser")]

internal partial class Program
{
#pragma warning disable IDE0060 // Remove unused parameter
    private static async Task Main(string[] args) => await BuildAvaloniaApp()
            .WithInterFont()
            .StartBrowserAppAsync("out");
#pragma warning restore IDE0060 // Remove unused parameter

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>();
}
