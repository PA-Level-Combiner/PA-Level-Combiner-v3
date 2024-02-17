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
using System.Collections.Generic;

namespace PALC.Main.Views.Combiners._20_4_4;

public partial class MainV : Window
{
    public MainVM vm;

    public MainV()
    {
        InitializeComponent();

        vm = new MainVM();
        DataContext = vm;

        vm.InvalidLevel += OnDisplayGeneralError;
        vm.InvalidSource += OnDisplayGeneralError;

        vm.advancedOptionsVM.InitialThemeLoadFailed += OnDisplayGeneralError;

        vm.MissingFields += OnMissingFields;
    }


    public async Task OnDisplayGeneralError(object? sender, DisplayGeneralErrorArgs e)
        => await MessageBoxTools.CreateErrorMsgBox(e).ShowWindowDialogAsync(this);


    public async void OnLoaded(object? sender, RoutedEventArgs e)
    {
        await vm.advancedOptionsVM.LoadDefaultThemes();
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
    public AsyncRelayCommand OnDeleteLevelCommand => _onDeleteLevelCommand ??= new AsyncRelayCommand(OnDeleteLevel);
    private async Task OnDeleteLevel()
    {
        if (vm.SelectedItems.Count == 0)
        {
            await OnDisplayGeneralError(this, new("No level selected. Wuh??", null));
            return;
        }

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


    private async Task OnMissingFields(object? sender, List<string> e)
    {
        await MessageBoxTools.CreateErrorMsgBox(
            $"The following fields aren't filled out yet:\n" +
            string.Join(", ", e)
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
