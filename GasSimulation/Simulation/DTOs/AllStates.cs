using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;

namespace GasSimulation.Simulation.DTOs
{
    public struct AllStates
    {
        public List<AtomState> Atoms { get; }
        public List<RectState> Rects { get; }

        public AllStates(IEnumerable<AtomState> atoms, IEnumerable<RectState> rects)
        {
            Atoms = atoms.ToList();
            Rects = rects.ToList();
        }
    }
}
