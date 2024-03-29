using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;
using PALC.Main.ViewModels.Splash.Inner;
using System.Diagnostics;

namespace PALC.Main.Views.Splash.Inner;


public partial class TableItem : TemplatedControl
{
    public static readonly StyledProperty<string> LeftProperty =
        AvaloniaProperty.Register<TableItem, string>(nameof(Left));

    public string Left
    {
        get => GetValue(LeftProperty);
        set => SetValue(LeftProperty, value);
    }


    public static readonly StyledProperty<string> RightProperty =
        AvaloniaProperty.Register<TableItem, string>(nameof(Right));

    public string Right
    {
        get => GetValue(RightProperty);
        set => SetValue(RightProperty, value);
    }
}


public partial class LinkButton : TemplatedControl
{
    public static readonly StyledProperty<string> ButtonClassesProperty =
        AvaloniaProperty.Register<LinkButton, string>(nameof(ButtonClasses));

    public string ButtonClasses
    {
        get => GetValue(ButtonClassesProperty);
        set => SetValue(ButtonClassesProperty, value);
    }


    public static readonly StyledProperty<string> TextBlockClassesProperty =
        AvaloniaProperty.Register<LinkButton, string>(nameof(TextBlockClasses));

    public string TextBlockClasses
    {
        get => GetValue(TextBlockClassesProperty);
        set => SetValue(TextBlockClassesProperty, value);
    }



    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<LinkButton, string>(nameof(Title));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<string> LinkProperty =
        AvaloniaProperty.Register<LinkButton, string>(nameof(Link));

    public string Link
    {
        get => GetValue(LinkProperty);
        set => SetValue(LinkProperty, value);
    }


    [RelayCommand]
    private void LaunchLink()
    {
        Process.Start(new ProcessStartInfo(Link) { UseShellExecute = true });
    }
}


public partial class About : Window
{
    public About()
    {
        InitializeComponent();

        DataContext = new AboutVM();
    }
}
