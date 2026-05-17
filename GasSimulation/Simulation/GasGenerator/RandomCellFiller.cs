using GasSimulation.Configuration;
using GasSimulation.Debuggers;
using GasSimulation.Exceptions;
using GasSimulation.GeneralDTOs;
using GasSimulation.Simulation.GasGenerator.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.GasGenerator
{
    public class RandomCellFiller
    {
        private readonly Config _config;
        private readonly Random _rand;
        private readonly GasGeneratorVisualDebugger _debugger;
        private readonly EmptyCellFiller _emptyCellFiller;

        public RandomCellFiller(Config config, Random rand,
            EmptyCellFiller emptyCellFiller, GasGeneratorVisualDebugger debugger)
        {
            _config = config;
            _rand = rand;
            _emptyCellFiller = emptyCellFiller;
            _debugger = debugger;
        }

        public PosState FillFreeCell(CellsArray cellsArray,
            List<IDPosState> freeCellsIds, List<IDPosState> partlyOccupiedCellsIds)
        {
            int randomId = _rand.Next(0, freeCellsIds.Count);

            double relX = (_rand.NextDouble() - 0.5) *
                _config.Simulation.AtomDiameter / _config.Simulation.Sqrt2;

            double relY = (_rand.NextDouble() - 0.5) *
                _config.Simulation.AtomDiameter / _config.Simulation.Sqrt2;

            return _emptyCellFiller.Fill(randomId, relX, relY,
                cellsArray, freeCellsIds, partlyOccupiedCellsIds);
        }

        public PosState FillPartlyOccupiedCell(CellsArray cellsArray, 
            List<IDPosState> partlyOccupiedCellsIds)
        {
            CellState partlyOccupiedCell;
            double relX = 0;
            double relY = 0;
            int cellId;

            while (true)
            {
                if (partlyOccupiedCellsIds.Count == 0) throw new NotEnoughPlaceException();

                cellId = _rand.Next(0, partlyOccupiedCellsIds.Count);
                partlyOccupiedCell = cellsArray.Array[partlyOccupiedCellsIds[cellId].I * cellsArray.Width + 
                    partlyOccupiedCellsIds[cellId].J];

                int attempts;

                for (attempts = 0; attempts < _config.Simulation.PasteAtomAttempts; attempts++)
                {
                    relX = (_rand.NextDouble() - 0.5) * 
                        _config.Simulation.AtomDiameter / _config.Simulation.Sqrt2;

                    relY = (_rand.NextDouble() - 0.5) * 
                        _config.Simulation.AtomDiameter / _config.Simulation.Sqrt2;

                    PosState newPos = MathHelper.TranslateField(partlyOccupiedCell.Pos, new(-relX, -relY));

                    if (!HasIntersection(newPos, partlyOccupiedCell.NearNeighbs)) break; 
                }

                if (attempts < _config.Simulation.PasteAtomAttempts)
                {
                    return _emptyCellFiller.Fill(cellId, relX, relY,
                        cellsArray, partlyOccupiedCellsIds, partlyOccupiedCellsIds);
                }
                else
                {
                    _debugger.CreateOccupiedCell(partlyOccupiedCell);

                    partlyOccupiedCellsIds[cellId] = partlyOccupiedCellsIds[partlyOccupiedCellsIds.Count - 1];
                    partlyOccupiedCellsIds.RemoveAt(partlyOccupiedCellsIds.Count - 1);
                }
            }
        }

        private bool HasIntersection(PosState newPos, List<PosState> neighbs)
        {
            foreach (PosState pos in neighbs)
            {
                if (MathHelper.CalculateDistance(newPos, pos) <= _config.Simulation.AtomDiameter)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
