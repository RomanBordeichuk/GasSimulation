using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;

namespace GasSimulation.Simulation.DTOs
{
    public class AllStates
    {
        public int? SelectedAtom { get; set; }
        public AtomState[] Atoms { get; }
        public RectState[] Rects { get; }

        public AllStates(AtomState[] atoms, RectState[] rects)
        {
            Atoms = atoms;
            Rects = rects;
        }

        public AllStates(AtomState[] atoms, RectState[] rects, int? selected)
        {
            Atoms = atoms;
            Rects = rects;
            SelectedAtom = selected;
        }

        public AllStates()
        {
            Atoms = [];
            Rects = [];
        }
    }
}
