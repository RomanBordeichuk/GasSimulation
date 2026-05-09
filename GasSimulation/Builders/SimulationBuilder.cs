using GasSimulation.Configuration;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator;
using GasSimulation.UIRendering;

namespace GasSimulation.Builders
{
    public class SimulationBuilder
    {
        private readonly Config _config;
        private readonly IterationCalculator _iterationCalculator;
        private readonly SimulationField _field;

        public SimulationBuilder(Config config, IterationCalculator 
            iterationCalculator, SimulationField field)
        {
            _config = config;
            _iterationCalculator = iterationCalculator;
            _field = field;
        }

        public Simulation.Simulation Build(SectorStates sectorStates, bool renderEnabled)
        {
            return new(_config, _iterationCalculator, _field, sectorStates, renderEnabled);
        }
    }
}
