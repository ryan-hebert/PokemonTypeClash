using System;
using BenchmarkDotNet.Running;
using PokemonTypeClash.Performance.Tests.Benchmarks;

namespace PokemonTypeClash.Performance.Tests;

public class BenchmarkRunner
{
    public static void RunBenchmarks()
    {
        System.Console.WriteLine("Starting PokemonTypeClash Performance Benchmarks...");
        System.Console.WriteLine("==================================================");
        
        // Run type effectiveness benchmarks
        var summary = BenchmarkDotNet.Running.BenchmarkRunner.Run<TypeEffectivenessBenchmarks>();
        
        System.Console.WriteLine("\nBenchmark Summary:");
        System.Console.WriteLine($"Total time: {summary.TotalTime}");
        System.Console.WriteLine($"Benchmarks run: {summary.Reports.Count()}");
        
        // Print results
        foreach (var report in summary.Reports)
        {
            System.Console.WriteLine($"\n{report.BenchmarkCase.Descriptor.DisplayInfo}:");
            System.Console.WriteLine($"  Mean: {report.ResultStatistics?.Mean:F2} ns");
            System.Console.WriteLine($"  Median: {report.ResultStatistics?.Median:F2} ns");
            System.Console.WriteLine($"  StdDev: {report.ResultStatistics?.StandardDeviation:F2} ns");
            if (report.GcStats.TotalOperations > 0)
            {
                System.Console.WriteLine($"  Memory: {report.GcStats.TotalOperations:F0} bytes allocated");
            }
        }
        
        System.Console.WriteLine("\nBenchmarks completed successfully!");
    }
}
