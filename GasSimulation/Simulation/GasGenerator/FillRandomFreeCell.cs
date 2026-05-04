using GasSimulation.GeneralDTOs;
using GasSimulation.Simulation.GasGenerator.DTOs;

namespace GasSimulation.Simulation.GasGenerator
{
    public static class FillRandomFreeCell
    {
        public static PosState Fill(Config config, CellsArray cellsArray,
            List<(int i, int j)> freeCellsIds, List<(int i, int j)> partlyOccupiedCellsIds)
        {
            int randomId = config.Rand.Next(0, freeCellsIds.Count);

            double relX = (config.Rand.NextDouble() - 0.5) * config.AtomDiameter / config.Sqrt2;
            double relY = (config.Rand.NextDouble() - 0.5) * config.AtomDiameter / config.Sqrt2;

            return FillEmptyCell.Fill(randomId, relX, relY, 
                cellsArray, freeCellsIds, partlyOccupiedCellsIds);
        }
    }
}
