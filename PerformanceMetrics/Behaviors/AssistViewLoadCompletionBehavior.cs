using MauiAIAssistViewPerformanceMetrics.Control;

namespace MauiAIAssistViewPerformanceMetrics.Behaviors;

/// <summary>
/// Forwards the <see cref="CustomAIAssistView.LayoutArranged"/> event to the
/// attached view's <see cref="BindableObject.BindingContext"/> when it
/// implements <see cref="IAssistViewLoadAware"/>.
/// </summary>
public sealed class AssistViewLoadCompletionBehavior : Behavior<CustomAIAssistView>
{
    protected override void OnAttachedTo(CustomAIAssistView bindable)
    {
        base.OnAttachedTo(bindable);
        BindingContext = bindable.BindingContext;
        bindable.BindingContextChanged += OnBindingContextChanged;
        bindable.LayoutArranged += OnLayoutArranged;
    }

    protected override void OnDetachingFrom(CustomAIAssistView bindable)
    {
        bindable.LayoutArranged -= OnLayoutArranged;
        bindable.BindingContextChanged -= OnBindingContextChanged;
        BindingContext = null;
        base.OnDetachingFrom(bindable);
    }

    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        if (sender is CustomAIAssistView assistView)
        {
            BindingContext = assistView.BindingContext;
        }
    }

    private void OnLayoutArranged(object? sender, EventArgs e)
    {
        if (BindingContext is IAssistViewLoadAware loadAware)
        {
            loadAware.OnAssistViewLayoutArranged();
        }
    }
}
