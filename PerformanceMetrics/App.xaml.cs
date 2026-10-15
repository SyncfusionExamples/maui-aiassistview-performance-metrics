using MauiAIAssistViewPerformanceMetrics.ViewModels;
using MauiAIAssistViewPerformanceMetrics.Views;
using Microsoft.Extensions.DependencyInjection;

namespace MauiAIAssistViewPerformanceMetrics;

public partial class App : Application
{
    private readonly IServiceProvider _services;

    public App(IServiceProvider services)
    {
        _services = services;
        InitializeComponent();
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JBaF5cXGhLflFxWGNbYl9wZFVGalxRTnZfSV9jS3hTfkdrWHpbeHRQRGhcU091Wg==");
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var mainPage = _services.GetRequiredService<MainPage>();
        mainPage.BindingContext = _services.GetRequiredService<ChatViewModel>();
        return new Window(new NavigationPage(mainPage));
    }
}