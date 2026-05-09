using GasSimulation.Configuration.ConfigParts;
using GasSimulation.Configuration.DTOs;
using GasSimulation.Mappers;

namespace GasSimulation.Configuration
{
    public record Config
    {
        public SimulationConsts Simulation { get; }
        public Brushes? Brushes { get; }
        public DebugConfig? Debug { get; }

        public Config(SimulationConfig simConfig, 
            ColorsConfig colorsConfig, DebugConfig debugConfig)
        {
            Simulation = simConfig.Map();
            Brushes = BrushMapper.Map(colorsConfig);
            Debug = debugConfig;
        }

        public Config(SimulationConsts simulation)
        {
            Simulation = simulation;
        }
    }
}
