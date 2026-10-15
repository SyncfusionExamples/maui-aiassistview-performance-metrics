using MauiAiAssistViewPerformanceMetrics.ViewModels;
using MauiAiAssistViewPerformanceMetrics.Views;
using Microsoft.Extensions.DependencyInjection;

namespace MauiAiAssistViewPerformanceMetrics.Services;

public sealed class AppNavigationService(IServiceProvider services) : IAppNavigationService
{
    public Task NavigateToAssistViewAsync()
    {
        var navigation = Application.Current?.Windows.FirstOrDefault()?.Page?.Navigation
            ?? throw new InvalidOperationException("The app navigation stack is unavailable.");
        var page = services.GetRequiredService<AssistViewPage>();
        page.BindingContext = services.GetRequiredService<ChatViewModel>();

        return navigation.PushAsync(page);
    }
}
