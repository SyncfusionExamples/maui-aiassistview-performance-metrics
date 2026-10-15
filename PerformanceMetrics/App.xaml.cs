using MauiAiAssistViewPerformanceMetrics.ViewModels;
using MauiAiAssistViewPerformanceMetrics.Views;
using Microsoft.Extensions.DependencyInjection;

namespace MauiAiAssistViewPerformanceMetrics;

public partial class App : Application
{
    private readonly IServiceProvider _services;

    public App(IServiceProvider services)
    {
        _services = services;
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var mainPage = _services.GetRequiredService<MainPage>();
        mainPage.BindingContext = _services.GetRequiredService<ChatViewModel>();

        return new Window(new NavigationPage(mainPage));
    }
}