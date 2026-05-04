using GasSimulation.Debuggers;
using GasSimulation.Exceptions;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.GasGenerator.DTOs;
using GasSimulation.Simulation.InitStateTransformer.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;
using GasSimulation.Simulation.Mappers;

namespace GasSimulation.Simulation.GasGenerator
{
    public static class GasGenerator
    {
        public static async Task<List<AtomConfigInitState>>
            Generate(Config config, RectState area, int numAtoms, double speed)
        {
            area = new(area.Pos, area.Dimentions, area.Angle * Math.PI / 180);

            double cellSize = config.AtomDiameter / config.Sqrt2;

            GasGeneratorVDHepler.SetParam<double>("CellSize", cellSize);

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

            GasGeneratorVDHepler.Initialize(config);
            GasGeneratorVDHepler.SetParam<RectState>("Area", area);
            GasGeneratorVDHepler.SetParam<TranslateFieldDelegate>("TranslateFieldMethod", TranslateField);

            await VisualDebugger.Stop();

            GasGeneratorVDHepler.CreateSector(area);

            await VisualDebugger.Stop();

            List<AtomConfigInitState> atoms = new();

            for (int i = 0; i < numAtoms; i++)
            {
                if (freeCellsIds.Count != 0)
                {
                    PosState pos = FillRandomFreeCell.Fill(config, cellsArray, 
                        freeCellsIds, partlyOccupiedCellsIds);

                    await VisualDebugger.Stop();

                    atoms.Add(AtomGenerator.Generate(config, pos, speed));
                }
                else if (partlyOccupiedCellsIds.Count != 0)
                {
                    PosState pos = FillRandomPartlyOccupiedCell.Fill(config, cellsArray,
                        partlyOccupiedCellsIds);

                    atoms.Add(AtomGenerator.Generate(config, pos, speed));
                }
                else throw new NotEnoughPlaceException();

                var lastAtom = atoms[atoms.Count - 1];

                VisualDebugger.Draw("Atoms", new AtomConfigInitState(
                    TranslateField(ref area, lastAtom.Pos), lastAtom.Speed, 
                    lastAtom.Angle).MapToState(config), config.ElemBrush);

                await VisualDebugger.Stop();
            }

            VisualDebugger.ClearGroup("Atoms");
            VisualDebugger.ClearGroup("Rects");
            VisualDebugger.ClearGroup("Sectors");

            await VisualDebugger.Stop();

            for (int i = 0; i < atoms.Count; i++)
            {
                atoms[i] = new(TranslateField(ref area, atoms[i].Pos), atoms[i].Speed, atoms[i].Angle);
            }

            return atoms;
        }

        private static PosState TranslateField(ref RectState area, PosState pos)
        {
            pos = MathHelper.RotateField(pos, area.Angle);
            pos = MathHelper.TranslateField(area.Pos, new(-pos.X, -pos.Y));

            return pos;
        }
    }
}
