```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8246/25H2/2025Update/HudsonValley2)
AMD Ryzen 5 7535HS with Radeon Graphics 3.30GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.300-preview.0.26177.108
  [Host]     : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3


```
| Method             | NumAtoms | Mean       | Error       | StdDev      | Median     | Rank | Allocated |
|------------------- |--------- |-----------:|------------:|------------:|-----------:|-----:|----------:|
| **CalculateIteration** | **500**      |   **189.8 μs** |     **2.38 μs** |     **2.11 μs** |   **189.8 μs** |    **1** |         **-** |
| **CalculateIteration** | **1000**     | **8,139.6 μs** | **2,378.97 μs** | **6,709.93 μs** | **5,554.9 μs** |    **2** |      **48 B** |
