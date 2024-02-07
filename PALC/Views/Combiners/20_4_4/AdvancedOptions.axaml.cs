using Avalonia.Controls;
using System.Threading.Tasks;

using PALC.Models.Combiners._20_4_4;
using PALC.ViewModels.Combiners._20_4_4;
using PALC.Views.Templates;
using static PALC.ViewModels.Combiners._20_4_4.MainVM;
using static PALC.ViewModels.Combiners._20_4_4.AdvancedOptionsVM;
using CommunityToolkit.Mvvm.Input;

namespace PALC.Views.Combiners._20_4_4;

public partial class AdvancedOptionsV : Window
{
    public AdvancedOptionsVM vm;

    public AdvancedOptionsV() : this(new AdvancedOptionsVM()) { }
    public AdvancedOptionsV(AdvancedOptionsVM viewModel)
    {
        InitializeComponent();

        vm = viewModel;
        DataContext = viewModel;

        vm.InvalidThemesFolder += OnInvalidThemesFolder;
        vm.NoThemes += OnNoThemes;
        vm.ResetThemeLoadFailed += OnResetThemeLoadFailed;
    }
    

    private async Task OnResetThemeLoadFailed(object? sender, ThemesLoadFailedArgs e)
    {
        var msg = MessageBoxTools.CreateErrorMsgBox(
            $"The default theme path ({vm.defaultThemesPath}) doesn't work. Please set a theme path.",
            e.ex
        );
        await msg.ShowWindowDialogAsync(this);
    }

    private async Task OnNoThemes(object? sender, NoThemesArgs e)
    {
        var msg = MessageBoxTools.CreateErrorMsgBox(
            $"There are no themes inside that folder.", null
        );
        await msg.ShowWindowDialogAsync(this);

        await OnSetThemesFolderCommand.ExecuteAsync(null);
    }

    public async Task OnInvalidThemesFolder(object? sender, InvalidThemesFolderArgs e)
    {
        var msg = MessageBoxTools.CreateErrorMsgBox(
            $"The theme folder either contains no themes or a theme failed to load.",
            e.ex
        );
        await msg.ShowWindowDialogAsync(this);

        await OnSetThemesFolderCommand.ExecuteAsync(null);
    }

    private AsyncRelayCommand? _onSetThemesFolderCommand;
    public AsyncRelayCommand OnSetThemesFolderCommand => _onSetThemesFolderCommand ??= new(OnSetThemesFolder);
    public async Task OnSetThemesFolder()
    {
        var folders = await BrowseTools.OpenFolder(this, "Select Theme Folder");
        if (folders.Count == 0) return;

        var path = folders[0].Path.LocalPath;
        await vm.SetThemesFolderCommand.ExecuteAsync(path);
    }


    [RelayCommand]
    public void Confirm()
        => Close();
}
