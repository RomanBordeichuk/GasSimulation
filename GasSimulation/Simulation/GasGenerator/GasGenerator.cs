using GasSimulation.Exceptions;
using GasSimulation.Logs;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.GasGenerator
{
    public static class GasGenerator
    {
        public static List<AtomInitState>
            Generate(Config config, RectState area, int numAtoms, double speed)
        {
            area = new(area.Pos, area.Dimentions, area.Angle * Math.PI / 180);

            double cellSize = config.AtomDiameter / Config.Sqrt2;

            int numCellsX = (int)((area.Width - config.AtomDiameter) / cellSize);
            int numCellsY = (int)((area.Height - config.AtomDiameter) / cellSize);

            CellsArray cellsArray = new(numCellsX, numCellsY);

            List<(int i, int j)> freeCellsIds = new(numCellsX * numCellsY);
            List<(int i, int j)> partlyOccupiedCellsIds = new();

            double translateX = ((double)numCellsX - 1) * cellSize / 2;
            double translateY = ((double)numCellsY - 1) * cellSize / 2;

            for (int i = 0; i < numCellsX; i++)
            {
                for (int j = 0; j < numCellsY; j++)
                {
                    cellsArray.Array[i * cellsArray.Width + j] = new(
                        new(i * cellSize - translateX, j * cellSize - translateY));
                    freeCellsIds.Add((i, j));
                }
            }

            List<AtomInitState> atoms = new();

            for (int i = 0; i < numAtoms; i++)
            {
                if (freeCellsIds.Count != 0)
                {
                    PosState pos = FillRandomFreeCell.Fill(config, cellsArray, 
                        freeCellsIds, partlyOccupiedCellsIds);

                    atoms.Add(AtomGenerator.Generate(config, pos, speed));
                }
                else if (partlyOccupiedCellsIds.Count != 0)
                {
                    PosState pos = FillRandomPartlyOccupiedCell.Fill(config, cellsArray,
                        partlyOccupiedCellsIds);

                    atoms.Add(AtomGenerator.Generate(config, pos, speed));
                }
                else throw new NotEnoughPlaceException();
            }

            for (int i = 0; i < atoms.Count; i++)
            {
                PosState pos = MathHelper.RotateField(atoms[i].Pos, area.Angle);
                pos = MathHelper.TranslateField(area.Pos, new(-pos.X, -pos.Y));

                atoms[i] = new(pos, atoms[i].Speed, atoms[i].Angle);
            }

            return atoms;
        }
    }
}
