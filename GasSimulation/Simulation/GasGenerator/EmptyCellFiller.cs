using GasSimulation.Debuggers;
using GasSimulation.GeneralDTOs;
using GasSimulation.Simulation.GasGenerator.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.GasGenerator
{
    public class EmptyCellFiller
    {
        private readonly GasGeneratorVisualDebugger _debugger;

        public EmptyCellFiller(GasGeneratorVisualDebugger debugger)
        {
            _debugger = debugger;
        }

        public PosState Fill(int randFreeCellId, double relX, double relY, 
            CellsArray cellsArray,
            List<IDPosState> freeCellsIds, List<IDPosState> partlyOccupiedCellsIds)
        {
            var idPos = new IDPosState(freeCellsIds[randFreeCellId].I, freeCellsIds[randFreeCellId].J);

            cellsArray.Array[idPos.I * cellsArray.Width + idPos.J].Status = CellState.CellStatus.Occupied;

            freeCellsIds[randFreeCellId] = freeCellsIds[freeCellsIds.Count - 1];
            freeCellsIds.RemoveAt(freeCellsIds.Count - 1);

            CellState freeCell = cellsArray.Array[idPos.I * cellsArray.Width + idPos.J];

            _debugger.CreateOccupiedCell(freeCell);

            PosState pos = MathHelper.TranslateField(freeCell.Pos, new(-relX, -relY));

            FillNearestPartlyOccupiedCells(idPos.I, idPos.J, freeCellsIds, 
                partlyOccupiedCellsIds, cellsArray, pos);

            return pos;
        }

        private void FillNearestPartlyOccupiedCells(int i, int j, List<IDPosState> freeCellsIds,
            List<IDPosState> partlyOccupiedCellsIds, CellsArray cellsArray, PosState pos)
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
                if (j < 0 || j >= cellsArray.Width ||
                    i < 0 || i >= cellsArray.Height) return;

                if (cellsArray.Array[i * cellsArray.Width + j].Status ==
                        CellState.CellStatus.Occupied) return;
                else if (cellsArray.Array[i * cellsArray.Width + j].Status == CellState.CellStatus.Free)
                {
                    _debugger.CreatePartlyOccupiedCell(
                        cellsArray.Array[i * cellsArray.Width + j]);

                    for (int k = 0; k < freeCellsIds.Count; k++)
                    {
                        if (freeCellsIds[k].I == i && freeCellsIds[k].J == j)
                        {
                            freeCellsIds[k] = freeCellsIds[freeCellsIds.Count - 1];
                            freeCellsIds.RemoveAt(freeCellsIds.Count - 1);

                            break;
                        }
                    }

                    partlyOccupiedCellsIds.Add(new(i, j));
                    cellsArray.Array[i * cellsArray.Width + j].Status = CellState.CellStatus.PartlyOccupied;
                }

                cellsArray.Array[i * cellsArray.Width + j].NearNeighbs.Add(pos);
            }
        }
    }
}
