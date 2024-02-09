using Avalonia;
using Avalonia.Controls.Primitives;

namespace PALC.Main.Views.Templates;

public class IncludeOptions : TemplatedControl
{
    public static readonly StyledProperty<bool> BeatmapObjectsProperty =
        AvaloniaProperty.Register<IncludeOptions, bool>(nameof(BeatmapObjects), true);

    public bool BeatmapObjects
    {
        get => GetValue(BeatmapObjectsProperty);
        set => SetValue(BeatmapObjectsProperty, value);
    }


    public static readonly StyledProperty<bool> PrefabsProperty =
        AvaloniaProperty.Register<IncludeOptions, bool>(nameof(Prefabs), true);

    public bool Prefabs
    {
        get => GetValue(PrefabsProperty);
        set => SetValue(PrefabsProperty, value);
    }


    public static readonly StyledProperty<bool> MarkersProperty =
        AvaloniaProperty.Register<IncludeOptions, bool>(nameof(Markers), true);

    public bool Markers
    {
        get => GetValue(MarkersProperty);
        set => SetValue(MarkersProperty, value);
    }


    public static readonly StyledProperty<bool> CheckpointsProperty =
        AvaloniaProperty.Register<IncludeOptions, bool>(nameof(Checkpoints), true);

    public bool Checkpoints
    {
        get => GetValue(CheckpointsProperty);
        set => SetValue(CheckpointsProperty, value);
    }


    public static readonly StyledProperty<bool> EventKFsProperty =
        AvaloniaProperty.Register<IncludeOptions, bool>(nameof(EventKFs), true);

    public bool EventKFs
    {
        get => GetValue(EventKFsProperty);
        set => SetValue(EventKFsProperty, value);
    }


    public static readonly StyledProperty<bool> BGObjectsProperty =
        AvaloniaProperty.Register<IncludeOptions, bool>(nameof(BGObjects), true);

    public bool BGObjects
    {
        get => GetValue(BGObjectsProperty);
        set => SetValue(BGObjectsProperty, value);
    }
}
