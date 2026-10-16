# MAUI AI AssistView Performance Metrics

This example repo maintains the demo used to measure the performance of the Syncfusion MAUI AI AssistView control for all 4 platforms with the latest metrics details for reference.


## AI AssistView Loading Performance

| Use Case | Windows | Android | macOS | iOS |
|----------|---------:|---------:|---------:|---------:|
| 100 Items Loading | 0.4372 | 5.2309 | 0.1183 | 0.8401 |


## Test Setup


| Item | Details |
|--------|---------|
| Version | Syncfusion .NET MAUI v35.1.40 |
| Android Device | Vivo T1, Android 14 |
| Windows Device | Windows 10 (10.0.17763.0) |
| iOS Device | iPhone 17e, iOS 27.0 |
| macOS Device | MacBook Air (Apple M4), macOS 27.0.1 |
| Build Configuration | Release, .NET 10.0.110 |
| Data | 100 Items |
| Method | Median of 5 runs, cold start excluded |
| Tested Date | October 15, 2026 |

## Notes

- All benchmarks were executed using the **Release** configuration on **.NET 10.0**.
- Performance measurements were collected on physical devices for Android, Windows, and macOS. iOS metrics were collected using the iOS Simulator.
- The results represent AI AssistView message loading times for a dataset containing **100 chat messages**.
- The reported value is the **median of 5 runs**, with cold-start measurements excluded.
- Actual performance may vary depending on device hardware, operating system version, message templates, and runtime conditions.
