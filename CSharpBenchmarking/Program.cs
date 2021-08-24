using BenchmarkDotNet.Running;
using System;

namespace CSharpBenchmarking
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<RendererBenchmarks>();
        }
    }
}
