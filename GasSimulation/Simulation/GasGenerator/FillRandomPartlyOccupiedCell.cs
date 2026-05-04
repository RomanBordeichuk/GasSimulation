using GasSimulation.Debuggers;
using GasSimulation.Exceptions;
using GasSimulation.GeneralDTOs;
using GasSimulation.Simulation.GasGenerator.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.GasGenerator
{
    public static class FillRandomPartlyOccupiedCell
    {
        public static PosState Fill(Config config, CellsArray cellsArray, 
            List<(int i, int j)> partlyOccupiedCellsIds)
        {
            CellState partlyOccupiedCell;
            double relX = 0;
            double relY = 0;
            int cellId;

            while (true)
            {
                if (partlyOccupiedCellsIds.Count == 0) throw new NotEnoughPlaceException();

                cellId = config.Rand.Next(0, partlyOccupiedCellsIds.Count);
                partlyOccupiedCell = cellsArray.Array[partlyOccupiedCellsIds[cellId].i * cellsArray.Width + 
                    partlyOccupiedCellsIds[cellId].j];

                int attempts;

                for (attempts = 0; attempts < config.PasteAtomAttempts; attempts++)
                {
                    relX = (config.Rand.NextDouble() - 0.5) * config.AtomDiameter / config.Sqrt2;
                    relY = (config.Rand.NextDouble() - 0.5) * config.AtomDiameter / config.Sqrt2;

                    PosState newPos = MathHelper.TranslateField(partlyOccupiedCell.Pos, new(-relX, -relY));

                    if (!HasIntersection(config, newPos, partlyOccupiedCell.NearNeighbs)) break; 
                }

                if (attempts < config.PasteAtomAttempts)
                {
                    return FillEmptyCell.Fill(cellId, relX, relY,
                        cellsArray, partlyOccupiedCellsIds, partlyOccupiedCellsIds);
                }
                else
                {
                    GasGeneratorVDHepler.CreateOccupiedCell(partlyOccupiedCell);

                    partlyOccupiedCellsIds[cellId] = partlyOccupiedCellsIds[partlyOccupiedCellsIds.Count - 1];
                    partlyOccupiedCellsIds.RemoveAt(partlyOccupiedCellsIds.Count - 1);
                }
            }
        }

        private static bool HasIntersection(Config config, PosState newPos, List<PosState> neighbs)
        {
            foreach (PosState pos in neighbs)
            {
                if (MathHelper.CalculateDistance(newPos, pos) <= config.AtomDiameter)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
