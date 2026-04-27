using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.UIElements;

namespace GasSimulation.Simulation
{
    public static class Simulation
    {
        private static Config _config = null!;
        private static AllElems _uiElems = null!;
        private static AllStates _elemStates;

        public static void Initialize(Config config, AllElems uiElems, AllStates elemStates)
        {
            _config = config;
            _uiElems = uiElems;
            _elemStates  = elemStates;
        }

        public static void Run()
        {
            IterationCalculator.IterationCalculator.Calculate(_config, _elemStates);

            SetStates();
        }

        private static void SetStates()
        {
            for (int i = 0; i < _uiElems.Atoms.Count; i++)
            {
                _uiElems.Atoms[i].UpdatePos(_elemStates.Atoms[i].Pos);
            }

            for (int i = 0; i < _uiElems.Rects.Count; i++)
            {
                _uiElems.Rects[i].UpdatePos(_elemStates.Rects[i].Pos);
            }
        }
    }
}
