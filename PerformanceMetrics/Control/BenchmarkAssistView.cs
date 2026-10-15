using Syncfusion.Maui.AIAssistView;

namespace MauiAiAssistViewPerformanceMetrics.Control;

public class BenchmarkAssistView : SfAIAssistView
{
    public event EventHandler? LayoutArranged;

    protected override Size ArrangeOverride(Rect bounds)
    {
        var size = base.ArrangeOverride(bounds);
        LayoutArranged?.Invoke(this, EventArgs.Empty);
        return size;
    }
}