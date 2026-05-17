using GasSimulation.Configuration;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.Simulation;
using System.Windows.Media;

namespace GasSimulation.UIRendering
{
    public class UIRenderer
    {
        private readonly Config _config;
        private readonly SimulationField _field;
        private readonly SimulationCore _simulation;
        private readonly Action<AtomState> _focusAtom;

        public UIRenderer(Config config, SimulationField field, SimulationCore simulation, Action<AtomState> focusAtom)
        {
            _config = config;
            _field = field;
            _simulation = simulation;
            _focusAtom = focusAtom;
        }

        public void SubscribeOnRendering(bool renderEnabled)
        {
            if (!renderEnabled) return;

            CompositionTarget.Rendering += (s, e) =>
            {
                var snapshot = _simulation.GetSnapshot();
                _field.RenderFrame(_config, snapshot, _focusAtom);
            };
        }
    }
}
