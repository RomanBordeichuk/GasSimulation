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
        private AllStates _allStates;
        private SectorCalculator _iterationCalculator = null!;

        [Params(500, 1000)]
        public int NumAtoms { get; set; }

        [GlobalSetup]
        public async ValueTask Setup()
        {
            var serviceConfigurator = new ServiceConfigurator(_config, null, null);
            var services = serviceConfigurator.Provider;

            var gasGenerator = services.GetRequiredService<GasGenerator>();
            _iterationCalculator = services.GetRequiredService<SectorCalculator>();

            var area = new RectState(100, 100, 400, 400, 0);
            var rawAtoms = await gasGenerator.Generate(area, NumAtoms, 100);

            _allStates = new(rawAtoms.MapToStates(_config), new List<RectState>());
        }

        [Benchmark]
        public async ValueTask CalculateIteration()
        {
            await _iterationCalculator.Calculate(_allStates);
        }
    }
}
