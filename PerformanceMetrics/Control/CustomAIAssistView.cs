using Syncfusion.Maui.AIAssistView;

namespace MauiAIAssistViewPerformanceMetrics.Control;

public class CustomAIAssistView : SfAIAssistView
{
    public event EventHandler? LayoutArranged;

    protected override Size ArrangeOverride(Rect bounds)
    {
        var size = base.ArrangeOverride(bounds);
        LayoutArranged?.Invoke(this, EventArgs.Empty);
        return size;
    }
}
