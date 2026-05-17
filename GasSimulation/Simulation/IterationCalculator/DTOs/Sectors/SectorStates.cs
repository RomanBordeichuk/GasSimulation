namespace GasSimulation.Simulation.IterationCalculator.DTOs.Sectors
{
    public class SectorStates
    {
        public Dictionary<long, SectorState> Sects { get; }
        public double SectorSize { get; }

        public SectorStates(Dictionary<long, SectorState> sects, double sectorSize)
        {
            Sects = sects;
            SectorSize = sectorSize;
        }

        public SectorStates()
        {
            Sects = [];
        }
    }
}
