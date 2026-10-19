using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;
using MauiAIAssistViewPerformanceMetrics.Behaviors;
using MauiAIAssistViewPerformanceMetrics.Models;
using Syncfusion.Maui.AIAssistView;

namespace MauiAIAssistViewPerformanceMetrics.ViewModels;

public sealed class AssistViewModel : INotifyPropertyChanged, IAssistViewLoadAware
{
    private const int TotalItemsPerRun = 100;
    private const int WarmupRunCount = 1;
    private const int MeasuredRunCount = 5;
    private readonly List<double> _runTimes = [];
    private Stopwatch? _loadStopwatch;
    private BenchmarkInfo _benchmarkInfo = new();

    public AssistViewModel()
    {
        this.GenerateAssistItems();
        LoadItemsCommand = new Command(StartLoad);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<IAssistItem> AssistItems { get; set; } = [];

    public BenchmarkInfo BenchmarkInfo
    {
        get => _benchmarkInfo;
        private set
        {
            if (_benchmarkInfo == value)
            {
                return;
            }

            _benchmarkInfo = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(BenchmarkDetails));
        }
    }

    public ICommand LoadItemsCommand { get; }

    public string BenchmarkDetails
    {
        get
        {
            var method = BenchmarkInfo.RunCount < MeasuredRunCount
                ? $"Collecting AssistView runs ({BenchmarkInfo.RunCount}/{MeasuredRunCount}); first run is warmup"
                : "Median AssistView load of 5 runs; first run excluded";
            var totalTime = BenchmarkInfo.RunCount < MeasuredRunCount
                ? BenchmarkInfo.LoadTimeSeconds
                : BenchmarkInfo.MedianLoadTime;

            return string.Join(
                Environment.NewLine,
                $"Version      : Syncfusion .NET MAUI v{BenchmarkInfo.SyncfusionVersion}",
                $"Build        : {BenchmarkInfo.BuildConfiguration}, {BenchmarkInfo.DotNetVersion}",
                $"Data         : {BenchmarkInfo.TotalItems} items",
                $"Method       : {method}",
                $"Tested       : {BenchmarkInfo.TestedTime.ToString("dd-MMM-yyyy HH:mm:ss")}",
                $"Total Seconds: {totalTime.ToString("F4")} seconds");
        }
    }

    private void StartLoad()
    {
        _loadStopwatch?.Stop();
        _loadStopwatch = Stopwatch.StartNew();
    }

    public void OnAssistViewLayoutArranged()
    {
        if (_loadStopwatch is not { IsRunning: true } stopwatch ||
            AssistItems.Count != TotalItemsPerRun)
        {
            return;
        }

        stopwatch.Stop();
        var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
        _loadStopwatch = null;
        _runTimes.Add(elapsedSeconds);
        UpdateBenchmarkInfo(elapsedSeconds);
    }

    private void GenerateAssistItems()
    {
        AssistItems.Clear();

        var baseTime = DateTime.Now;
        for (var index = 1; index <= TotalItemsPerRun / 2; index++)
        {
            var request = new AssistItem
            {
                Text = $"Request {index}: Explain MAUI feature {index}",
                IsRequested = true,
                DateTime = baseTime.AddMinutes(index)
            };

            var response = new AssistItem
            {
                Text = $"Response {index} .NET MAUI provides a single framework for building Android, iOS, macOS, and Windows applications,Supports native performance and UI rendering.",
                IsRequested = false,
                RequestItem = request,
                DateTime = baseTime.AddMinutes(index + 1)
            };

            AssistItems.Add(request);
            AssistItems.Add(response);
        }
    }

    private void UpdateBenchmarkInfo(double lastRunSeconds)
    {
        var completedMeasuredRuns = Math.Min(
            Math.Max(_runTimes.Count - WarmupRunCount, 0),
            MeasuredRunCount);

        BenchmarkInfo = new BenchmarkInfo
        {
            SyncfusionVersion = typeof(SfAIAssistView).Assembly.GetName().Version?.ToString() ?? "Unknown",
            DotNetVersion = RuntimeInformation.FrameworkDescription,
#if DEBUG
            BuildConfiguration = "Debug",
#else
            BuildConfiguration = "Release",
#endif
            TotalItems = AssistItems.Count,
            LoadTimeSeconds = lastRunSeconds,
            MedianLoadTime = CalculateMedian(),
            RunCount = completedMeasuredRuns,
            TestedTime = DateTime.Now
        };
    }

    private double CalculateMedian()
    {
        if (_runTimes.Count < WarmupRunCount + MeasuredRunCount)
        {
            return 0;
        }

        return _runTimes
            .Skip(WarmupRunCount)
            .Take(MeasuredRunCount)
            .Order()
            .ElementAt(MeasuredRunCount / 2);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
