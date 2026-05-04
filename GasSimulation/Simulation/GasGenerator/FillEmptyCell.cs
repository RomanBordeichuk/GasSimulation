using GasSimulation.Debuggers;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.GasGenerator.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.GasGenerator
{
    public static class FillEmptyCell
    {
        public static PosState Fill(int randFreeCellId, double relX, double relY, 
            CellsArray cellsArray,
            List<(int i, int j)> freeCellsIds, List<(int i, int j)> partlyOccupiedCellsIds)
        {
            int i = freeCellsIds[randFreeCellId].i;
            int j = freeCellsIds[randFreeCellId].j;

            cellsArray.Array[i * cellsArray.Width + j].Status = CellState.CellStatus.Occupied;

            freeCellsIds[randFreeCellId] = freeCellsIds[freeCellsIds.Count - 1];
            freeCellsIds.RemoveAt(freeCellsIds.Count - 1);

            CellState freeCell = cellsArray.Array[i * cellsArray.Width + j];

            GasGeneratorVDHepler.CreateOccupiedCell(freeCell);

            PosState pos = MathHelper.TranslateField(freeCell.Pos, new(-relX, -relY));

            FillNearestPartlyOccupiedCells(i, j, freeCellsIds, partlyOccupiedCellsIds, cellsArray, pos);

            return pos;
        }

        private static void FillNearestPartlyOccupiedCells(int i, int j, List<(int i, int j)> freeCellsIds,
            List<(int i, int j)> partlyOccupiedCellsIds, CellsArray cellsArray, PosState pos)
        {
            CheckRangeAndAdd(i - 1, j - 1);
            CheckRangeAndAdd(i - 1, j + 1);
            CheckRangeAndAdd(i - 1, j);
            CheckRangeAndAdd(i + 1, j - 1);
            CheckRangeAndAdd(i + 1, j + 1);
            CheckRangeAndAdd(i + 1, j);
            CheckRangeAndAdd(i, j - 1);
            CheckRangeAndAdd(i, j + 1);

            CheckRangeAndAdd(i + 2, j - 1);
            CheckRangeAndAdd(i + 2, j + 1);
            CheckRangeAndAdd(i + 2, j);

            CheckRangeAndAdd(i - 2, j - 1);
            CheckRangeAndAdd(i - 2, j + 1);
            CheckRangeAndAdd(i - 2, j);

            CheckRangeAndAdd(i - 1, j - 2);
            CheckRangeAndAdd(i + 1, j - 2);
            CheckRangeAndAdd(i, j - 2);

            CheckRangeAndAdd(i - 1, j + 2);
            CheckRangeAndAdd(i + 1, j + 2);
            CheckRangeAndAdd(i, j + 2);

            void CheckRangeAndAdd(int i, int j)
            {
                if (i < 0 || i >= cellsArray.Width ||
                    j < 0 || j >= cellsArray.Height) return;

                if (cellsArray.Array[i * cellsArray.Width + j].Status ==
                        CellState.CellStatus.Occupied) return;
                else if (cellsArray.Array[i * cellsArray.Width + j].Status == CellState.CellStatus.Free)
                {
                    GasGeneratorVDHepler.CreatePartlyOccupiedCell(
                        cellsArray.Array[i * cellsArray.Width + j]);

                    for (int k = 0; k < freeCellsIds.Count; k++)
                    {
                        if (freeCellsIds[k].i == i && freeCellsIds[k].j == j)
                        {
                            freeCellsIds[k] = freeCellsIds[freeCellsIds.Count - 1];
                            freeCellsIds.RemoveAt(freeCellsIds.Count - 1);

                            break;
                        }
                    }

                    partlyOccupiedCellsIds.Add((i, j));
                    cellsArray.Array[i * cellsArray.Width + j].Status = CellState.CellStatus.PartlyOccupied;
                }

                cellsArray.Array[i * cellsArray.Width + j].NearNeighbs.Add(pos);
            }
        }
    }
}
