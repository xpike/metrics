# Change Log

## [2.4.0]

- Improvements to `OperationTracker`
- Timings now properly record fractional milliseconds
- Added .NET Core 2.2 example project
- Renamed `MetricsSettings` to `MetricsConfig`
- Renamed `DataDogProviderSettings` to `DataDogProviderConfig`
- Added the concept of a `MetricsContext` similar to `LogContext` to allow tags to be applied across all metrics recorded within an async scope
- Added `ConsoleMetricsProvider`
- Added option to stop the clock when calling `IOperationTracker.SetSuccess()` and `IOperationTracker.SetFailure()`
- Improved `OperationTracker` to allow recording attempts/results independently of timings (for when using a provider other than DataDog)
- Improved handling of Global Tags aka Constant Tags
- Global tags now automatically populated with key information such as host name and IP addresses

## [2.3.1]

- Renamed some registration extension methods

## [2.3.0]

- Added Azure AppInsights provider

## [2.2.0]

- Improved `MetricsTimer`, added `OperationTracker`

## [2.1.0]

Updated to use `IConfig<>` instead of `ISettings<>`.

## [2.0.0]

### Breaking Changes

- Changed interfaces to use `IEnumerable<string>` for tags instead of `string[]`. This elimiates the overhead of copying
  arrays in consuming code when building out tags.

## [1.0.0]

Inital release.

Basic support for:

- DataDog
- Amazon CloudWatch
