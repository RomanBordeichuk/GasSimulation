using GasSimulation.Configuration;
using GasSimulation.Configuration.ConfigParts;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Mappers;
using GasSimulation.Simulation.GasGenerator.DTOs;
using GasSimulation.Simulation.InitStateTransformer.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;
using System.Diagnostics;
using System.Windows.Media;

namespace GasSimulation.Debuggers
{
    public class GasGeneratorVisualDebugger : ConcreteVisualDebugger
    {
        private const string _sectorsGroup = "Sectors";

        public GasGeneratorVisualDebugger(Config config, VisualDebugger debugger)
            : base(config, debugger)
        {
            if (_config.Debug != null &&
                _config.Debug.DebugModules.Contains(ActiveVisualDebugModule.GasGenerator))
            {
                _disabled = false;
            }
            else _disabled = true;
        }

        [Conditional("DEBUG")]
        public void CreateAtom(AtomConfigInitState atomState)
        {
            if (_disabled) return;

            var TransformMethod = GetParam<TranslateFieldDelegate>("TranslateFieldMethod");
            var area = GetParam<RectState>("Area");

            atomState = new AtomConfigInitState(
                TransformMethod(ref area, atomState.Pos), atomState.Speed,
                atomState.Angle);

            _debugger.Draw(_atomsGroup, atomState.MapToState(_config),
                _config.Brushes!.Elem, null);
        }

        [Conditional("DEBUG")]
        public void CreateOccupiedCell(CellState cellState)
        {
            if (_disabled) return;

            CreateCell(cellState, _config.Brushes!.OccupiedCell);
        }

        [Conditional("DEBUG")]
        public void CreatePartlyOccupiedCell(CellState cellState)
        {
            if (_disabled) return;

            CreateCell(cellState, _config.Brushes!.PartlyOccupiedCell);
        }

        [Conditional("DEBUG")]
        public void CreateSector()
        {
            if (_disabled) return;

            var area = GetParam<RectState>("Area");

            area = new RectState(area.Pos, area.Dimentions,
                MathHelper.TransformAngleToDEG(area.Angle)).MapToState(_config);

            _debugger.DrawHollow(_sectorsGroup, area, 5,
                _config.Brushes!.Sector, null);
        }

        [Conditional("DEBUG")]
        public void ClearSectors()
        {
            if (_disabled) return;

            _debugger.ClearGroup(_sectorsGroup);
        }

        private void CreateCell(CellState cellState, SolidColorBrush brush)
        {
            var TransformMethod = GetParam<TranslateFieldDelegate>("TranslateFieldMethod");
            var area = GetParam<RectState>("Area");
            var cellSize = GetParam<double>("CellSize");

            _debugger.Draw(_rectsGroup, new RectState(TransformMethod(ref area, cellState.Pos),
                new(cellSize * 0.95, cellSize * 0.95), MathHelper.TransformAngleToDEG(area.Angle))
                .MapToState(_config), brush, -1);
        }
    }

    public delegate PosState TranslateFieldDelegate(ref RectState area, PosState pos);
}
