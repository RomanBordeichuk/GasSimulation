using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;

namespace GasSimulation.Simulation.DTOs
{
    public struct AllStates
    {
        public AtomState[] Atoms { get; }
        public RectState[] Rects { get; }

        public AllStates(IEnumerable<AtomState> atoms, IEnumerable<RectState> rects)
        {
            Atoms = atoms.ToArray();
            Rects = rects.ToArray();
        }
    }
}
