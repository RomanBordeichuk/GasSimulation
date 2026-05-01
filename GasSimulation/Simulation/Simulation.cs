using GasSimulation.Simulation.DTOs;

namespace GasSimulation.Simulation
{
    public static class Simulation
    {
        private static Config _config = null!;
        private static ParticleRenderer _renderer = null!;
        private static AllStates _elemStates;

        public static void Initialize(Config config, ParticleRenderer renderer, AllStates elemStates)
        {
            _config = config;
            _renderer = renderer;
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
            _renderer.RenderFrame(_config, _elemStates);
        }
    }
}
