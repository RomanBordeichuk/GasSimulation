using GasSimulation.Configuration;
using GasSimulation.Simulation.IterationCalculator;
using GasSimulation.Transformers.ConfigInitStateTransformer;
using GasSimulation.Transformers.ConfigInitStateTransformer.DTOs;

namespace GasSimulation.Builders
{
    public class SimulationBuilder
    {
        private readonly Config _config;
        private readonly IterationCalculator _iterationCalculator;
        private readonly ConfigInitStateTransformer _configInitStateTransformer;
        private readonly ManualResetEventSlim _coreResetEvent;

        public SimulationBuilder(Config config, IterationCalculator iterationCalculator,
            ConfigInitStateTransformer configInitStateTransformer, 
            ManualResetEventSlim coreResetEvent)
        {
            _config = config;
            _iterationCalculator = iterationCalculator;
            _configInitStateTransformer = configInitStateTransformer;
            _coreResetEvent = coreResetEvent;
        }

        public Simulation.SimulationCore Build(List<ConfigInitState> rawInitState)
        {
            return new(_config, _iterationCalculator, 
                _configInitStateTransformer, _coreResetEvent, rawInitState);
        }
    }
}
