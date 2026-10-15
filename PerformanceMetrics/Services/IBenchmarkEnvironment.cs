namespace MauiAiAssistViewPerformanceMetrics.Services;

public interface IBenchmarkEnvironment
{
    string Platform { get; }

    string DotNetVersion { get; }
}
