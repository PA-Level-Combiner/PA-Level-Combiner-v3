using Avalonia.Controls;
using PALC.Updater.ViewModels;

namespace PALC.Updater.Views;

public partial class MainV : Window
{
    private MainVM vm;
    public MainV()
    {
        InitializeComponent();

        vm = new();
        DataContext = vm;
    }
}
