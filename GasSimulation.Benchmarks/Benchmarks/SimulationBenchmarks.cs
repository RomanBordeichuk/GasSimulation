using BenchmarkDotNet.Attributes;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Mappers;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.GasGenerator;
using GasSimulation.Simulation.IterationCalculator;
using Microsoft.Extensions.DependencyInjection;

namespace GasSimulation.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    public class SimulationBenchmarks : BenchmarksBase
    {
        private AllStates _allStates = null!;
        private IterationCalculator _iterationCalculator = null!;

        [Params(1000, 5000)]
        public int NumAtoms { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var serviceConfigurator = new ServiceConfigurator(_config, null, null, null, null);
            var services = serviceConfigurator.Provider;

            var gasGenerator = services.GetRequiredService<GasGenerator>();
            _iterationCalculator = services.GetRequiredService<IterationCalculator>();

            var area = new RectState(100, 100, 1000, 1000, 0);
            var rawAtoms = gasGenerator.Generate(area, NumAtoms, 100);

            _allStates = new(rawAtoms.MapToStates(_config).ToArray(), []);
        }

        [Benchmark]
        public void CalculateIteration()
        {
            _iterationCalculator.Calculate(_allStates);
        }
    }
}
