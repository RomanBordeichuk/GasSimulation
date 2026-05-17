using GasSimulation.Configuration;
using GasSimulation.Simulation;
using GasSimulation.UIRendering;

namespace GasSimulation.Builders
{
    public class UIRendererBuilder
    {
        private readonly Config _config;
        private readonly SimulationField _field;

        public UIRendererBuilder(Config config, SimulationField field)
        {
            _config = config;
            _field = field;
        }

        public UIRenderer Build(SimulationCore simulation)
        {
            return new(_config, _field, simulation);
        }
    }
}
