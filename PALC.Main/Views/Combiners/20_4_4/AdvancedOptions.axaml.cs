using Avalonia.Controls;
using System.Threading.Tasks;
using PALC.Main.ViewModels.Combiners._20_4_4;
using CommunityToolkit.Mvvm.Input;
using PALC.Common.Views.Templates;

namespace PALC.Main.Views.Combiners._20_4_4;

public partial class AdvancedOptionsV : Window
{
    public AdvancedOptionsVM vm;

    public AdvancedOptionsV() : this(new AdvancedOptionsVM()) { }
    public AdvancedOptionsV(AdvancedOptionsVM viewModel)
    {
        InitializeComponent();

        vm = viewModel;
        DataContext = viewModel;

        vm.InvalidThemesFolder += OnDisplayGeneralError;
    }


    public async Task OnDisplayGeneralError(object? sender, DisplayGeneralErrorArgs e)
        => await MessageBoxTools.CreateErrorMsgBox(e).ShowWindowDialogAsync(this);


    private AsyncRelayCommand? _onSetThemesFolderCommand;
    public AsyncRelayCommand OnSetThemesFolderCommand => _onSetThemesFolderCommand ??= new(OnSetThemesFolder);
    public async Task OnSetThemesFolder()
    {
        var folders = await BrowseTools.OpenFolder(this, "Select Theme Folder");
        if (folders.Count == 0) return;

        var path = folders[0].Path.LocalPath;
        await vm.SetThemesFolderLoggedCommand.ExecuteAsync(path);
    }


    [RelayCommand]
    public void Confirm()
        => Close();
}
