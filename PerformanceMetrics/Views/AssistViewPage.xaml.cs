using MauiAIAssistViewPerformanceMetrics.ViewModels;

namespace MauiAIAssistViewPerformanceMetrics.Views;

public partial class AssistViewPage : ContentPage
{
    public AssistViewPage()
    {
        InitializeComponent();
        AssistView.LayoutArranged += OnAssistViewLayoutArranged;
    }

    private void OnAssistViewLayoutArranged(object? sender, EventArgs e)
    {
        if (BindingContext is ChatViewModel viewModel)
        {
            viewModel.CompleteLoadAfterArrange();
        }
    }
}
