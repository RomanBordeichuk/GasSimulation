```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8246/25H2/2025Update/HudsonValley2)
AMD Ryzen 5 7535HS with Radeon Graphics 3.30GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.300-preview.0.26177.108
  [Host]     : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3


```
| Method             | NumAtoms | Mean       | Error       | StdDev      | Median     | Rank | Allocated |
|------------------- |--------- |-----------:|------------:|------------:|-----------:|-----:|----------:|
| **CalculateIteration** | **500**      |   **120.3 μs** |     **1.01 μs** |     **0.95 μs** |   **120.3 μs** |    **1** |         **-** |
| **CalculateIteration** | **1000**     | **8,014.1 μs** | **2,836.70 μs** | **7,860.48 μs** | **4,442.4 μs** |    **2** |         **-** |
