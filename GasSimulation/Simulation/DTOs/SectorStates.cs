namespace GasSimulation.Simulation.DTOs
{
    public struct SectorStates
    {
        public List<SectorState> Sectors { get; }
        public double CellSize { get; }

        public SectorStates(List<SectorState> sectors, double cellSize)
        {
            Sectors = sectors;
            CellSize = cellSize;
        }
    }
}
