using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

using Avalonia.Controls;
using Avalonia.Platform.Storage;
using PALC.Main.ViewModels.Combiners._20_4_4;
using static PALC.Main.ViewModels.Combiners._20_4_4.MainVM;
using static PALC.Main.ViewModels.Combiners._20_4_4.AdvancedOptionsVM;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using PALC.Common.Views.Templates;

namespace PALC.Main.Views.Combiners._20_4_4;

public partial class MainV : Window
{
    public MainVM vm;

    public MainV()
    {
        InitializeComponent();

        vm = new MainVM();
        DataContext = vm;

        vm.InvalidLevel += OnInvalidLevel;
        vm.InvalidSource += OnInvalidSourceLevelFolder;

        vm.advancedOptionsVM.InitialThemeLoadFailed += OnDefaultThemesLoadFailed;

        vm.MissingFields += OnMissingFields;
    }


    public async Task OnDefaultThemesLoadFailed(object? sender, ThemesLoadFailedArgs e)
    {
        var msg = MessageBoxTools.CreateErrorMsgBox(
            $"An error occurred while automatically loading the default theme path ({vm.advancedOptionsVM.defaultThemesPath}). " +
            $"Please set a theme path in the \"Advanced Options\" button.",
            e.ex
        );
        await msg.ShowWindowDialogAsync(this);
    }

    public async void OnLoaded(object? sender, RoutedEventArgs e)
    {
        await vm.advancedOptionsVM.LoadDefaultThemes();
    }
    


    public async Task OnInvalidSourceLevelFolder(object? sender, InvalidSourceArgs e)
    {
        var msg = MessageBoxTools.CreateErrorMsgBox(
            $"The selected source folder is invalid.",
            e.ex
        );
        await msg.ShowWindowDialogAsync(this);

        await OnSetSourceCommand.ExecuteAsync(null);
    }

    // TODO source generators are unreliable in visual studio so if anyone can help me out to use [RelayCommand] that'd be great
    private AsyncRelayCommand? _onSetSourceCommand;
    public AsyncRelayCommand OnSetSourceCommand => _onSetSourceCommand ??= new(OnSetSource);
    public async Task OnSetSource()
    {
        var folders = await BrowseTools.OpenFolder(this, "Select Folder...");
        if (folders.Count == 0) return;

        var result = folders[0].Path.LocalPath;

        await vm.SetSourceCommand.ExecuteAsync(result);
    }



    public async Task OnInvalidLevel(object? sender, InvalidLevelArgs e)
    {
        await MessageBoxTools.CreateErrorMsgBox(
            $"The level \"{e.failedLevelPath}\" cannot be loaded.\n\n", e.ex)
        .ShowWindowDialogAsync(this);

        await OnAddLevelCommand.ExecuteAsync(null);
    }

    private AsyncRelayCommand? _onAddLevelCommand;
    public AsyncRelayCommand OnAddLevelCommand => _onAddLevelCommand ??= new(OnAddLevel);
    private async Task OnAddLevel()
    {
        var topLevel = GetTopLevel(this) ?? throw new Exception();

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                FileTypeFilter =
                [
                    new FilePickerFileType("Standard PA Level") { Patterns = ["level.lsb"] },
                    new FilePickerFileType("PA File") { Patterns = ["*.lsb"] }
                ],
                AllowMultiple = true
            }
        );

        if (files.Count == 0) return;

        vm.AddLevelsCommand.Execute(files);
    }



    private AsyncRelayCommand? _onDeleteLevelCommand;
    public AsyncRelayCommand OnDeleteLevelCommand => _onDeleteLevelCommand ??= new(OnDeleteLevel);
    private async Task OnDeleteLevel()
    {
        var msg = MessageBoxManager.GetMessageBoxStandard(
            "Delete?",
            "Are you sure you want to delete the selected level/s?",
            MsBox.Avalonia.Enums.ButtonEnum.YesNo,
            MsBox.Avalonia.Enums.Icon.Info,
            WindowStartupLocation.CenterScreen
        );

        var result = await msg.ShowWindowDialogAsync(this);
        if (result == MsBox.Avalonia.Enums.ButtonResult.No) return;

        vm.DeleteLevelsCommand.Execute(null);
    }



    public async Task OnInvalidOutputFolder(object? sender, InvalidSourceArgs e)
    {
        var msg = MessageBoxTools.CreateErrorMsgBox(
            $"The selected output folder is invalid.",
            e.ex
        );
        await msg.ShowWindowDialogAsync(this);

        await OnSetOutputFolderPathCommand.ExecuteAsync(null);
    }

    private AsyncRelayCommand? _onSetOutputFolderPathCommand;
    public AsyncRelayCommand OnSetOutputFolderPathCommand => _onSetOutputFolderPathCommand ??= new(OnSetOutputFolderPath);
    public async Task OnSetOutputFolderPath()
    {
        var folders = await BrowseTools.OpenFolder(this, "Select Folder...");
        if (folders.Count == 0) return;

        var result = folders[0].Path.LocalPath;

        vm.SetOutputFolderPathCommand.Execute(result);
    }




    public async Task OnAdvancedOptions()
    {
        AdvancedOptionsV advancedOptionsV = new(vm.advancedOptionsVM);
        await advancedOptionsV.ShowDialog(this);
    }


    private async Task OnMissingFields(object? sender, MissingFieldsArgs e)
    {
        await MessageBoxTools.CreateErrorMsgBox(
            $"The following fields aren't filled out yet:\n" +
            string.Join(", ", e.fields)
        ).ShowWindowDialogAsync(this);
    }

    // TODO do you even need relaycommands here
    private AsyncRelayCommand? _onCombineCommand;
    public AsyncRelayCommand OnCombineCommand => _onCombineCommand ??= new(OnCombine);
    public async Task OnCombine()
    {
        var areFieldsFilled = await vm.AreFieldsFilled();
        if (!areFieldsFilled) return;

        var msg = MessageBoxManager.GetMessageBoxStandard(
            "Combine?",
            "Are you sure you want to start combining these levels?",
            MsBox.Avalonia.Enums.ButtonEnum.YesNo,
            MsBox.Avalonia.Enums.Icon.Info,
            WindowStartupLocation.CenterScreen
        );

        var result = await msg.ShowWindowDialogAsync(this);

        if (result == MsBox.Avalonia.Enums.ButtonResult.No) return;

        CombiningVM combiningVM = new(
            vm.SourceLevelFolder,
            [.. vm.LevelList],
            vm.advancedOptionsVM.IncludeOptionSettings,
            [.. vm.advancedOptionsVM.ThemeFiles],
            vm.OutputFolderPath
        );
        Combining combiningV = new(combiningVM);
        await combiningV.ShowDialog(this);
    }
}
