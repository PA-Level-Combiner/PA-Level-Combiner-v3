using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace PALC.Main.Views.Templates;


public partial class LevelList : ContentControl
{
    public static readonly StyledProperty<object> ListBoxProperty =
        AvaloniaProperty.Register<LevelList, object>(nameof(ListBox));
    public object ListBox
    {
        get => GetValue(ListBoxProperty);
        set => SetValue(ListBoxProperty, value);
    }


    public static readonly StyledProperty<ICommand> AddItemCommandProperty =
        AvaloniaProperty.Register<LevelList, ICommand>(nameof(AddItemCommand));
    public ICommand AddItemCommand
    {
        get => GetValue(AddItemCommandProperty);
        set => SetValue(AddItemCommandProperty, value);
    }


    public static readonly StyledProperty<ICommand> DeleteItemCommandProperty =
        AvaloniaProperty.Register<LevelList, ICommand>(nameof(DeleteItemCommand));
    public ICommand DeleteItemCommand
    {
        get => GetValue(DeleteItemCommandProperty);
        set => SetValue(DeleteItemCommandProperty, value);
    }
}
