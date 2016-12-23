``` ini

BenchmarkDotNet=v0.10.1, OS=OSX
Processor=?, ProcessorCount=4
Frequency=1000000000 Hz, Resolution=1.0000 ns, Timer=UNKNOWN
dotnet cli version=1.0.0-preview2-1-003177
  [Host]     : .NET Core 4.6.24628.01, 64bit RyuJIT
  DefaultJob : .NET Core 4.6.24628.01, 64bit RyuJIT


```
      Method |      Mean |    StdErr |    StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
------------ |---------- |---------- |---------- |--------- |--------- |--------- |---------- |
    WithLinq | 6.8006 ms | 0.0363 ms | 0.1407 ms | 841.6667 | 712.5000 | 702.0833 |   4.11 MB |
 WithoutLinq | 6.1493 ms | 0.0904 ms | 0.8766 ms | 355.6985 | 355.6985 | 355.6985 |   1.74 MB |
