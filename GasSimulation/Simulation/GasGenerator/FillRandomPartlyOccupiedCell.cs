using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.Exceptions;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.GasGenerator
{
    public static class FillRandomPartlyOccupiedCell
    {
        public static PosState Fill(ref CellState[,] cellsMatrix, 
            List<(int i, int j)> partlyOccupiedCellsIds)
        {
            double relX = 0;
            double relY = 0;
            CellState partlyOccupiedCell;

            while (true)
            {
                if (partlyOccupiedCellsIds.Count == 0) throw new NotEnoughPlaceException();

                int cellId = Statics.Rand.Next(0, partlyOccupiedCellsIds.Count);
                partlyOccupiedCell = cellsMatrix[
                    partlyOccupiedCellsIds[cellId].i, partlyOccupiedCellsIds[cellId].j];

                int attempts;

                for (attempts = 0; attempts < Constants.PasteAtomAttempts; attempts++)
                {
                    relX = Statics.Rand.NextDouble() - 0.5 * Constants.AtomDiameter / (2 * Statics.Sqrt2);
                    relY = Statics.Rand.NextDouble() - 0.5 * Constants.AtomDiameter / (2 * Statics.Sqrt2);

                    foreach (PosState pos in partlyOccupiedCell.NearNeighbs)
                    {
                        if (MathHelper.CalculateDistance(pos, new(relX, relY)) > Constants.AtomDiameter)
                            break;
                    }
                }
                
                partlyOccupiedCellsIds.RemoveAt(cellId);

                if (attempts < Constants.PasteAtomAttempts) break;
            }

            return MathHelper.TranslateField(partlyOccupiedCell.Pos, new(-relX, -relY));
        }
    }
}
