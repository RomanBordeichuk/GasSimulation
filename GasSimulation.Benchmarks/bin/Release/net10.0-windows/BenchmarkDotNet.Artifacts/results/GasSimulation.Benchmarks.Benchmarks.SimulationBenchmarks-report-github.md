```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8457/25H2/2025Update/HudsonValley2)
AMD Ryzen 5 7535HS with Radeon Graphics 3.30GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.300
  [Host]     : .NET 10.0.8 (10.0.8, 10.0.826.23019), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.8 (10.0.8, 10.0.826.23019), X64 RyuJIT x86-64-v3


```
| Method             | NumAtoms | Mean         | Error         | StdDev        | Median       | Rank | Gen0       | Gen1       | Gen2      | Allocated    |
|------------------- |--------- |-------------:|--------------:|--------------:|-------------:|-----:|-----------:|-----------:|----------:|-------------:|
| **CalculateIteration** | **1000**     |     **443.2 μs** |       **8.74 μs** |      **20.60 μs** |     **445.4 μs** |    **1** |   **218.2617** |   **218.2617** |   **62.0117** |    **764.51 KB** |
| **CalculateIteration** | **5000**     | **592,418.3 μs** | **186,806.04 μs** | **523,824.21 μs** | **359,146.5 μs** |    **2** | **21000.0000** | **10000.0000** | **4000.0000** | **176108.24 KB** |
