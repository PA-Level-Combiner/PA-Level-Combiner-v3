using Avalonia;
using Avalonia.Controls.Primitives;

namespace PALC.Main.Views.Templates;

public class CheckBoxSizeable : TemplatedControl
{
    public static readonly StyledProperty<double> CheckBoxSizeProperty =
        AvaloniaProperty.Register<CheckBoxSizeable, double>(nameof(CheckBoxSize), 60);

    public double CheckBoxSize
    {
        get => GetValue(CheckBoxSizeProperty);
        set => SetValue(CheckBoxSizeProperty, value);
    }


    public static readonly StyledProperty<bool> IsCheckedProperty =
         AvaloniaProperty.Register<CheckBoxSizeable, bool>(nameof(IsChecked));

    public bool IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }


    public static readonly StyledProperty<string> TextProperty =
         AvaloniaProperty.Register<CheckBoxSizeable, string>(nameof(Text));

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

}
