namespace MauiAiAssistViewPerformanceMetrics.Models;

public sealed record BenchmarkInfo
{
    public string SyncfusionVersion { get; init; } = "Unknown";

    public string DotNetVersion { get; init; } = string.Empty;

    public string BuildConfiguration { get; init; } = string.Empty;

    public int TotalItems { get; init; }

    public double LoadTimeSeconds { get; init; }

    public double MedianLoadTime { get; init; }

    public int RunCount { get; init; }

    public DateTime TestedTime { get; init; }
}
