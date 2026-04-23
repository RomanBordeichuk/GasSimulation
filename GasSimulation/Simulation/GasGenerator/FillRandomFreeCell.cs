using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.GasGenerator
{
    public static class FillRandomFreeCell
    {
        public static PosState Fill(ref CellState[,] cellsMatrix,
            List<(int i, int j)> freeCellsIds, List<(int i, int j)> partlyOccupiedCellsIds)
        {
            int randomId = Statics.Rand.Next(0, freeCellsIds.Count);

            int i = freeCellsIds[randomId].i;
            int j = freeCellsIds[randomId].j;

            CellState freeCell = cellsMatrix[i, j];

            FillNearestPartlyOccupiedCells(i, j, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);

            double relX = (Statics.Rand.NextDouble() - 0.5) * Constants.AtomDiameter / (2 * Statics.Sqrt2);
            double relY = (Statics.Rand.NextDouble() - 0.5) * Constants.AtomDiameter / (2 * Statics.Sqrt2);

            if (relX >= 2 - Statics.Sqrt2) FillRightPartlyOccupiedCells(
                i, j, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);

            else if (relX <= -2 + Statics.Sqrt2) FillLeftPartlyOccupiedCells(
                i, j, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);

            if (relY >= 2 - Statics.Sqrt2) FillTopPartlyOccupiedCells(
                i, j, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);

            else if (relY <= -2 + Statics.Sqrt2) FillBottomPartlyOccupiedCells(
                i, j, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);

            freeCellsIds.RemoveAt(randomId);

            return MathHelper.TranslateField(freeCell.Pos, new(-relX, -relY));
        }

        private static void FillNearestPartlyOccupiedCells(int i, int j,
            List<(int i, int j)> partlyOccupiedCellsIds, ref CellState[,] cellsMatrix, CellState freeCell)
        {
            CheckRangeAndAdd(i - 1, j - 1, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i - 1, j + 1, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i - 1, j, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i + 1, j - 1, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i + 1, j + 1, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i + 1, j, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i, j + 1, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i, j - 1, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
        }

        private static void FillTopPartlyOccupiedCells(int i, int j,
            List<(int i, int j)> partlyOccupiedCellsIds, ref CellState[,] cellsMatrix, CellState freeCell)
        {
            CheckRangeAndAdd(i + 2, j - 1, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i + 2, j + 1, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i + 2, j, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
        }

        private static void FillBottomPartlyOccupiedCells(int i, int j,
            List<(int i, int j)> partlyOccupiedCellsIds, ref CellState[,] cellsMatrix, CellState freeCell)
        {
            CheckRangeAndAdd(i - 2, j - 1, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i - 2, j + 1, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i - 2, j, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
        }

        private static void FillLeftPartlyOccupiedCells(int i, int j,
            List<(int i, int j)> partlyOccupiedCellsIds, ref CellState[,] cellsMatrix, CellState freeCell)
        {
            CheckRangeAndAdd(i - 1, j - 2, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i + 1, j - 2, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i, j - 2, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
        }

        private static void FillRightPartlyOccupiedCells(int i, int j,
            List<(int i, int j)> partlyOccupiedCellsIds, ref CellState[,] cellsMatrix, CellState freeCell)
        {
            CheckRangeAndAdd(i - 1, j + 2, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i + 1, j + 2, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
            CheckRangeAndAdd(i, j + 2, partlyOccupiedCellsIds, ref cellsMatrix, freeCell);
        }

        private static void CheckRangeAndAdd(int i, int j, List<(int i, int j)> partlyOccupiedCellsIds,
            ref CellState[,] cellsMatrix, CellState freeCell)
        {
            if (i >= 0 && i < cellsMatrix.GetLength(0) &&
                j >= 0 && j < cellsMatrix.GetLength(1))
            {
                partlyOccupiedCellsIds.Add((i, j));
                cellsMatrix[i, j].NearNeighbs.Add(freeCell.Pos);
            }
        }
    }
}
