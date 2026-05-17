using GasSimulation.Configuration;
using GasSimulation.Debuggers;
using GasSimulation.Exceptions;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.GasGenerator.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;
using GasSimulation.Transformers.ConfigInitStateTransformer.DTOs;

namespace GasSimulation.Simulation.GasGenerator
{
    public class GasGenerator
    {
        private readonly Config _config;
        private readonly RandomCellFiller _randomCellFiller;
        private readonly AtomGenerator _atomGenerator;
        private readonly GasGeneratorVisualDebugger _debugger;

        public GasGenerator(Config config, 
            RandomCellFiller randomCellFiller,
            AtomGenerator atomGenerator,
            GasGeneratorVisualDebugger debugger)
        {
            _config = config;
            _randomCellFiller = randomCellFiller;
            _atomGenerator = atomGenerator;
            _debugger = debugger;
        }

        public List<AtomConfigInitState>
            Generate(RectState area, int numAtoms, double speed)
        {
            area = new(area.Pos, area.Dimentions, area.Angle * Math.PI / 180);

            double cellSize = _config.Simulation.AtomDiameter / _config.Simulation.Sqrt2;

            _debugger.SetParam<double>("CellSize", cellSize);

            int numCellsX = (int)((area.Width - _config.Simulation.AtomDiameter) / cellSize);
            int numCellsY = (int)((area.Height - _config.Simulation.AtomDiameter) / cellSize);

            CellsArray cellsArray = new(numCellsX, numCellsY);

            List<IDPosState> freeCellsIds = new(numCellsX * numCellsY);
            List<IDPosState> partlyOccupiedCellsIds = new();

            double translateX = ((double)numCellsX - 1) * cellSize / 2;
            double translateY = ((double)numCellsY - 1) * cellSize / 2;

            for (int i = 0; i < numCellsY; i++)
            {
                for (int j = 0; j < numCellsX; j++)
                {
                    cellsArray.Array[i * cellsArray.Width + j] = new(
                        new(j * cellSize - translateX, i * cellSize - translateY));
                    freeCellsIds.Add(new(i, j));
                }
            }

            _debugger.SetParam<RectState>("Area", area);
            _debugger.SetParam<TranslateFieldDelegate>("TranslateFieldMethod", TranslateField);

            _debugger.BreakPoint();
            _debugger.CreateSector();
            _debugger.BreakPoint();

            List<AtomConfigInitState> atoms = new();

            for (int i = 0; i < numAtoms; i++)
            {
                if (freeCellsIds.Count != 0)
                {
                    PosState pos = _randomCellFiller.FillFreeCell(cellsArray, 
                        freeCellsIds, partlyOccupiedCellsIds);

                    atoms.Add(_atomGenerator.Generate(pos, speed));
                }
                else if (partlyOccupiedCellsIds.Count != 0)
                {
                    PosState pos = _randomCellFiller.FillPartlyOccupiedCell(cellsArray,
                        partlyOccupiedCellsIds);

                    atoms.Add(_atomGenerator.Generate(pos, speed));
                }
                else throw new NotEnoughPlaceException();

                _debugger.CreateAtom(atoms[atoms.Count - 1]);
                _debugger.BreakPoint();
            }

            _debugger.ClearAll();
            _debugger.BreakPoint();

            for (int i = 0; i < atoms.Count; i++)
            {
                atoms[i] = new(TranslateField(ref area, atoms[i].Pos), atoms[i].Speed, atoms[i].Angle);
            }

            _debugger.Debug();

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
