using Avalonia.Data;
using Avalonia.Data.Converters;
using System.Globalization;
using System;
using System.IO;

namespace PALC.Main.Views.Combiners._20_4_4.Converters;

public class LevelStartConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string valueStr)
        {
            DirectoryInfo? fullPath = new DirectoryInfo(valueStr).Parent?.Parent;
            if (fullPath == null) return "...";

            return fullPath.FullName + Path.DirectorySeparatorChar;
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class LevelMiddleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string valueStr)
        {
            DirectoryInfo? parent = new DirectoryInfo(valueStr).Parent;
            if (parent == null) return "??? Unknown Folder ???";

            return parent.Name + Path.DirectorySeparatorChar;
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class LevelEndConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string valueStr)
        {
            var path = new DirectoryInfo(valueStr);
            return path.Name;
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
