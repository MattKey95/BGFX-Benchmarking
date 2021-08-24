using BenchmarkDotNet.Attributes;
using CSharp;

namespace CSharpBenchmarking
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class RendererBenchmarks
    {
        private Renderer _renderer;

        public RendererBenchmarks()
        {
            var startup = new Startup();
            startup.CreateWindow();

            _renderer = new Renderer();
            _renderer.Init();
        }

        [Benchmark]
        public void Render()
        {
            _renderer.Render();
        }
    }
}
