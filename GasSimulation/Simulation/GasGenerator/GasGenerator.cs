using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.Exceptions;

namespace GasSimulation.Simulation.GasGenerator
{
    public static class GasGenerator
    {
        private static double _sqrt2 = Math.Pow(2, 0.5);

        public static List<AtomInitState>
            Generate(RectState area, int numAtoms, double avSpeed)
        {
            double cellSize = Constants.AtomDiameter / _sqrt2;

            int numCellsX = (int)((area.Width - Constants.AtomDiameter) / cellSize);
            int numCellsY = (int)((area.Height - Constants.AtomDiameter) / cellSize);

            CellState[,] cellsMatrix = new CellState[numCellsX, numCellsY];
            List<(int i, int j)> freeCellsIds = new(numCellsX * numCellsY);
            List<(int i, int j)> partlyOccupiedCellsIds = new();

            for (int i = 0; i < numCellsX; i++)
            {
                for (int j = 0; j < numCellsY; j++)
                {
                    cellsMatrix[i, j] = new(
                        new(i * cellSize + cellSize / 2, j * cellSize + cellSize / 2));
                    freeCellsIds.Add((i, j));
                }
            }

            List<AtomInitState> atoms = new();

            for (int i = 0; i < numAtoms; i++)
            {
                if (freeCellsIds.Count != 0)
                {
                    PosState pos = FillRandomFreeCell.Fill(ref cellsMatrix, 
                        freeCellsIds, partlyOccupiedCellsIds);

                    atoms.Add(AtomGenerator.Generate(pos, avSpeed));
                }
                else if (partlyOccupiedCellsIds.Count != 0)
                {
                    PosState pos = FillRandomPartlyOccupiedCell.Fill(ref cellsMatrix,
                        partlyOccupiedCellsIds);

                    atoms.Add(AtomGenerator.Generate(pos, avSpeed));
                }
                else throw new NotEnoughPlaceException();
            }

            return atoms;
        }
    }
}
