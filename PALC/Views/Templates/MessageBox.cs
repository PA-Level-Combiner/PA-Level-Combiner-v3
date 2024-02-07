using System;

using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

namespace PALC.Views.Templates;

public static class MessageBoxTools
{
    public static IMsBox<ButtonResult> CreateErrorMsgBox(string message, Exception? ex = null)
        => MessageBoxManager.GetMessageBoxStandard(
            "Error!",
            message + "\n\n" + (ex?.Message ?? string.Empty)
        );
}
