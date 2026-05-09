using GasSimulation.Configuration;
using GasSimulation.Simulation.DTOs;
using GasSimulation.UIRendering;
using System.Windows;

namespace GasSimulation.Simulation
{
    public class Simulation
    {
        private readonly Config _config;
        private readonly IterationCalculator.IterationCalculator _iterationCalculator;
        private readonly SimulationField? _field;

        private SectorStates _sectorStates;
        private readonly bool _renderEnabled;

        public Simulation(Config config, IterationCalculator.IterationCalculator iterationCalculator, 
            SimulationField? field, SectorStates sectorStates, bool renderEnabled)
        {
            _config = config;
            _field = field;
            _sectorStates = sectorStates;
            _renderEnabled = renderEnabled;
            _iterationCalculator = iterationCalculator;

            Render();
        }

        public async ValueTask Run()
        {
            await _iterationCalculator.Calculate(_sectorStates);

            Render();
        }

        private void Render()
        {
            if (!_renderEnabled) return;

            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                _field!.RenderFrame(_config, _sectorStates);
            });
        }
    }
}
