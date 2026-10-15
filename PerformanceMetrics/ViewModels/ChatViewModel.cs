using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MauiAiAssistViewPerformanceMetrics.Models;
using MauiAiAssistViewPerformanceMetrics.Services;
using Syncfusion.Maui.AIAssistView;

namespace MauiAiAssistViewPerformanceMetrics.ViewModels;

public sealed class ChatViewModel : INotifyPropertyChanged
{
    private const int TotalItemsPerRun = 100;
    private const int WarmupRunCount = 1;
    private const int MeasuredRunCount = 5;

    private readonly List<double> _runTimes = [];
    private readonly IAppNavigationService _navigationService;
    private readonly IAlertService _alertService;
    private readonly IBenchmarkEnvironment _benchmarkEnvironment;
    private Stopwatch? _loadStopwatch;
    private double _dataPreparationTimeSeconds;
    private BenchmarkInfo _benchmarkInfo = new();

    public ChatViewModel(
        IAppNavigationService navigationService,
        IAlertService alertService,
        IBenchmarkEnvironment benchmarkEnvironment)
    {
        _navigationService = navigationService;
        _alertService = alertService;
        _benchmarkEnvironment = benchmarkEnvironment;
        LoadItemsCommand = new Command(async () => await LoadItemsAsync());
        GetDetailsCommand = new Command(async () => await ShowBenchmarkDetailsAsync());
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<IAssistItem> AssistItems { get; } = [];

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

    public ICommand GetDetailsCommand { get; }

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

    private async Task LoadItemsAsync()
    {
        _loadStopwatch = Stopwatch.StartNew();
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
                Text = @$"## Response {index}
- .NET MAUI provides a single framework for building Android, iOS, macOS, and Windows applications,Supports native performance and UI rendering.",
                //Text = $"Response {index}: .NET MAUI provides a single framework for building Android, iOS, macOS and Windows applications. It supports native performance and UI rendering.",
                IsRequested = false,
                RequestItem = request,
                DateTime = baseTime.AddMinutes(index + 1)
            };

            AssistItems.Add(request);
            AssistItems.Add(response);
        }

        try
        {
            await _navigationService.NavigateToAssistViewAsync();
        }
        catch
        {
            _loadStopwatch.Stop();
            _loadStopwatch = null;
            throw;
        }
    }

    public void CompleteLoadAfterArrange()
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

    private async Task ShowBenchmarkDetailsAsync() =>
        await _alertService.ShowAlertAsync("Benchmark Details", BenchmarkDetails, "OK");

    private void UpdateBenchmarkInfo(double lastRunSeconds)
    {
        var completedMeasuredRuns = Math.Min(
            Math.Max(_runTimes.Count - WarmupRunCount, 0),
            MeasuredRunCount);

        BenchmarkInfo = new BenchmarkInfo
        {
            SyncfusionVersion = typeof(SfAIAssistView).Assembly.GetName().Version?.ToString() ?? "Unknown",
            DotNetVersion = _benchmarkEnvironment.DotNetVersion,
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
