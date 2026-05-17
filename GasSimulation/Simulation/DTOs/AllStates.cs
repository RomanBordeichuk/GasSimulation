using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;

namespace GasSimulation.Simulation.DTOs
{
    public class AllStates
    {
        public AtomState[] Atoms { get; }
        public RectState[] Rects { get; }

        public AllStates(AtomState[] atoms, RectState[] rects)
        {
            Atoms = atoms;
            Rects = rects;
        }

        public AllStates()
        {
            Atoms = [];
            Rects = [];
        }
    }
}
