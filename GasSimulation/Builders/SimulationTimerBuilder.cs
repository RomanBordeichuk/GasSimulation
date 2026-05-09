using GasSimulation.Configuration;
using GasSimulation.Simulation;

namespace GasSimulation.Builders
{
    public class SimulationTimerBuilder
    {
        private readonly Config _config;

        public SimulationTimerBuilder(Config config)
        {
            _config = config;
        }

        public SimulationTimer Build(Simulation.Simulation simulation)
        {
            return new(_config, simulation);
        }
    }
}
