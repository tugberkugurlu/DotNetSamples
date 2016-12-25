``` ini

BenchmarkDotNet=v0.10.1, OS=OSX
Processor=?, ProcessorCount=4
Frequency=1000000000 Hz, Resolution=1.0000 ns, Timer=UNKNOWN
dotnet cli version=1.0.0-preview2-1-003177
  [Host]     : .NET Core 4.6.24628.01, 64bit RyuJIT
  DefaultJob : .NET Core 4.6.24628.01, 64bit RyuJIT

Allocated=0 B  

```
                   Method |        Mean |    StdDev |
------------------------- |------------ |---------- |
 OptimizedWith100000Input | 478.1712 us | 8.9126 us |
