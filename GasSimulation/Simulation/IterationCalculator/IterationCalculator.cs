using GasSimulation.Configuration;
using GasSimulation.Simulation.DTOs;

namespace GasSimulation.Simulation.IterationCalculator
{
    public class IterationCalculator
    {
        private readonly Config _config;

        public IterationCalculator(Config config)
        {
            _config = config;
        }

        public async ValueTask Calculate(SectorStates sectorStates)
        {
            
        }
    }
}
