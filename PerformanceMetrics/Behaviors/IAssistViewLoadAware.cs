namespace MauiAIAssistViewPerformanceMetrics.Behaviors;

/// <summary>
/// Marker interface for view models that want to be notified when the
/// attached AssistView completes its layout pass.
/// </summary>
public interface IAssistViewLoadAware
{
    void OnAssistViewLayoutArranged();
}
