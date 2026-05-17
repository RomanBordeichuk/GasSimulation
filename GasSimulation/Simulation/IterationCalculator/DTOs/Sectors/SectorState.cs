namespace GasSimulation.Simulation.IterationCalculator.DTOs.Sectors
{
    public class SectorState
    {
        public List<int> AtomIds { get; }
        public List<int> RectIds { get; }

        public SectorState(List<int> atomIds, List<int> rectIds)
        {
            AtomIds = atomIds;
            RectIds = rectIds;
        }

        public SectorState()
        {
            AtomIds = new();
            RectIds = new();
        }
    }
}
