using GasSimulation.GeneralDTOs.Atom;

namespace GasSimulation.Simulation.DTOs
{
    public struct SectorState
    {
        public AllStates AllStates { get; }
        public List<AtomState> GhostAtoms { get; }
        public int I { get; }
        public int J { get; }

        public SectorState(AllStates allStates, List<AtomState> ghostAtoms, int i, int j)
        {
            AllStates = allStates;
            GhostAtoms = ghostAtoms;
            I = i;
            J = j;
        }
    }
}
