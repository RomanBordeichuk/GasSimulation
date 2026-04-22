using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.DTOs.Interfaces;
using GasSimulation.Simulation.Exceptions;
using GasSimulation.Simulation.UIElements;

namespace GasSimulation.Simulation
{
    public static class Simulation
    {
        private static List<Element> _uiElems = null!;
        private static List<IElemState> _elemStates = null!;

        public static void Initialize(List<Element> uiElems, List<IElemState> elemStates)
        {
            _uiElems = uiElems;
            _elemStates  = elemStates;
        }

        public static void Run()
        {
            IterationCalculator.IterationCalculator.Calculate(_elemStates);

            SetStates();
        }

        private static void SetStates()
        {
            for (int i = 0; i < _uiElems.Count; i++)
            {
                if (_elemStates[i] is AtomState atom)
                    _uiElems[i].UpdatePos(atom.Pos);
                else if (_elemStates[i] is RectState rect)
                    _uiElems[i].UpdatePos(rect.Pos);
                else throw new IncorrectTypeException();
            }
        }
    }
}
