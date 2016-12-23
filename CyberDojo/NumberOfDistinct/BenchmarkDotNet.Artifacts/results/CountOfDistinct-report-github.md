``` ini

BenchmarkDotNet=v0.10.1, OS=OSX
Processor=?, ProcessorCount=4
Frequency=1000000000 Hz, Resolution=1.0000 ns, Timer=UNKNOWN
dotnet cli version=1.0.0-preview2-1-003177
  [Host]     : .NET Core 4.6.24628.01, 64bit RyuJITDEBUG
  DefaultJob : .NET Core 4.6.24628.01, 64bit RyuJIT


```
      Method |      Mean |    StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
------------ |---------- |---------- |--------- |--------- |--------- |---------- |
    WithLinq | 6.8079 ms | 0.1640 ms | 837.5000 | 706.2500 | 702.0833 |   4.11 MB |
 WithoutLinq | 5.1323 ms | 0.1201 ms | 351.0417 | 351.0417 | 351.0417 |   1.74 MB |
