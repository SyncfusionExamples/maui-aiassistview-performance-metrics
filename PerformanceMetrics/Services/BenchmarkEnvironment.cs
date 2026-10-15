namespace MauiAiAssistViewPerformanceMetrics.Services;

public sealed class BenchmarkEnvironment : IBenchmarkEnvironment
{
    public string Platform => DeviceInfo.Platform.ToString();

    public string DotNetVersion => $".NET 10.0.110";
}
