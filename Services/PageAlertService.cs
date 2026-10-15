namespace MauiAiAssistViewPerformanceMetrics.Services;

public sealed class PageAlertService : IAlertService
{
    public Task ShowAlertAsync(string title, string message, string cancel)
    {
        var rootPage = Application.Current?.Windows.FirstOrDefault()?.Page
            ?? throw new InvalidOperationException("The active page is unavailable.");
        var page = rootPage is NavigationPage navigationPage
            ? navigationPage.CurrentPage
            : rootPage;

        return page.DisplayAlertAsync(title, message, cancel);
    }
}
