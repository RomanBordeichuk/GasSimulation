using GasSimulation.Simulation.DTOs;
using GasSimulation.UIRendering;

namespace GasSimulation.Simulation
{
    public static class Simulation
    {
        private static Config _config = null!;
        private static SimulationField _field = null!;
        private static AllStates _elemStates;

        public static void Initialize(Config config, SimulationField field, AllStates elemStates)
        {
            _config = config;
            _field = field;
            _elemStates  = elemStates;

            Render();
        }

        public static void Run()
        {
            IterationCalculator.IterationCalculatorOld.Calculate(_config, _elemStates);

            Render();
        }

        private static void Render()
        {
            _field.RenderFrame(_config, _elemStates);
        }
    }
}
