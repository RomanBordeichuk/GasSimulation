using BenchmarkDotNet.Attributes;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.GasGenerator;
using GasSimulation.Simulation.IterationCalculator;
using GasSimulation.Simulation.Mappers;

namespace GasSimulation.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    public class SimulationBenchmarks : BenchmarksBase
    {
        private AllStates _allStates;

        [Params(500, 1000)]
        public int NumAtoms { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var area = new RectState(100, 100, 400, 400, 0);
            var rawAtoms = GasGenerator.Generate(_config, area, NumAtoms, 100);

            _allStates = new(rawAtoms.MapToStates(_config), new List<RectState>());
        }

        [Benchmark]
        public void CalculateIteration()
        {
            IterationCalculatorOld.Calculate(_config, _allStates);
        }
    }
}
