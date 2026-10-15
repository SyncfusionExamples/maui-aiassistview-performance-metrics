using MauiAIAssistViewPerformanceMetrics.ViewModels;
using MauiAIAssistViewPerformanceMetrics.Views;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
#if MAUI_DEVFLOW
using Microsoft.Maui.DevFlow.Agent;
#endif

namespace MauiAIAssistViewPerformanceMetrics;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
    #if MAUI_DEVFLOW
        builder.AddMauiDevFlowAgent();
    #endif
        builder
            .UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<ChatViewModel>();
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
    }
}
