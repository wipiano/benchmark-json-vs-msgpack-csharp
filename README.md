BenchmarkDotNet=v0.12.0, OS=ubuntu 19.04
Intel Core i7-8700 CPU 3.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.0.100
  [Host]   : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT
  ShortRun : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

|                 Method |       Mean |     Error |   StdDev | Ratio | RatioSD |     Gen 0 |     Gen 1 |     Gen 2 | Allocated | Completed Work Items | Lock Contentions |
|----------------------- |-----------:|----------:|---------:|------:|--------:|----------:|----------:|----------:|----------:|---------------------:|-----------------:|
|       Utf8JsonWithType | 1,001.4 ms | 337.02 ms | 18.47 ms |  1.00 |    0.00 | 1000.0000 | 1000.0000 |         - | 642.99 MB |               3.0000 |                - |
|     Utf8JsonWithTypeGz | 4,134.0 ms | 987.83 ms | 54.15 ms |  4.13 |    0.13 | 7000.0000 | 7000.0000 | 2000.0000 | 814.09 MB |               3.0000 |                - |
|       Utf8JsonTypeless |   984.2 ms |  50.38 ms |  2.76 ms |  0.98 |    0.02 | 1000.0000 | 1000.0000 |         - | 642.99 MB |               2.0000 |                - |
|     Utf8JsonTypelessGz | 4,194.6 ms | 777.21 ms | 42.60 ms |  4.19 |    0.04 | 7000.0000 | 7000.0000 | 2000.0000 | 814.08 MB |               3.0000 |                - |
|    MessagePackWithType |   390.3 ms | 231.84 ms | 12.71 ms |  0.39 |    0.02 |         - |         - |         - |  336.9 MB |               2.0000 |                - |
| MessagePackWithTypeLz4 |   537.4 ms |  66.94 ms |  3.67 ms |  0.54 |    0.01 | 2000.0000 | 2000.0000 |         - | 395.79 MB |               2.0000 |                - |
|    MessagePackTypeless |   368.1 ms |   2.59 ms |  0.14 ms |  0.37 |    0.01 |         - |         - |         - | 335.94 MB |               2.0000 |                - |
| MessagePackTypelessLz4 |   515.2 ms |  75.83 ms |  4.16 ms |  0.51 |    0.01 | 2000.0000 | 2000.0000 |         - | 394.22 MB |               2.0000 |                - |

// * Legends *
  Mean                 : Arithmetic mean of all measurements
  Error                : Half of 99.9% confidence interval
  StdDev               : Standard deviation of all measurements
  Ratio                : Mean of the ratio distribution ([Current]/[Baseline])
  RatioSD              : Standard deviation of the ratio distribution ([Current]/[Baseline])
  Gen 0                : GC Generation 0 collects per 1000 operations
  Gen 1                : GC Generation 1 collects per 1000 operations
  Gen 2                : GC Generation 2 collects per 1000 operations
  Allocated            : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  Completed Work Items : The number of work items that have been processed in ThreadPool (per single operation)
  Lock Contentions     : The number of times there was contention upon trying to take a Monitor's lock (per single operation)
  1 ms                 : 1 Millisecond (0.001 sec)

