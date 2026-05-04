using GasSimulation.Exceptions;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.GasGenerator.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;
using GasSimulation.Simulation.Mappers;
using System.Diagnostics;
using System.Windows.Media;

namespace GasSimulation.Debuggers
{
    public static class GasGeneratorVDHepler
    {
        private static Config _config = null!;
        private static Dictionary<string, object> _params = new();

        [Conditional("DEBUG")]
        public static void Initialize(Config config)
        {
            _config = config;
        }

        [Conditional("DEBUG")]
        public static void SetParam<T>(string name, T value)
        {
            _params[name] = value!;
        }

        [Conditional("DEBUG")]
        public static void CreateOccupiedCell(CellState cellState)
        {
            CreateCell(cellState, _config.OccupiedCellBrush);
        }

        [Conditional("DEBUG")]
        public static void CreatePartlyOccupiedCell(CellState cellState)
        {
            CreateCell(cellState, _config.PartlyOccupiedCellBrush);
        }

        [Conditional("DEBUG")]
        public static void CreateSector(RectState rectState)
        {
            var area = GetParam<RectState>("Area");

            area = new RectState(area.Pos, area.Dimentions, 
                MathHelper.TransformAngleToDEG(area.Angle)).MapToState(_config);

            VisualDebugger.DrawHollow("Sectors", area, 5, _config.SectorBrush);
        }

        private static void CreateCell(CellState cellState, SolidColorBrush brush)
        {
            var TransformMethod = GetParam<TranslateFieldDelegate>("TranslateFieldMethod");
            var area = GetParam<RectState>("Area");
            var cellSize = GetParam<double>("CellSize");

            VisualDebugger.Draw("Rects", new RectState(TransformMethod(ref area, cellState.Pos),
                new(cellSize * 0.95, cellSize * 0.95), MathHelper.TransformAngleToDEG(area.Angle))
                .MapToState(_config), brush);
        }

        private static T GetParam<T>(string name)
        {
            if (_params.TryGetValue(name, out object? value) && value is T valueT)
            {
                return valueT;
            }
            throw new IncorrectParamException();
        }
    }

    public delegate PosState TranslateFieldDelegate(ref RectState area, PosState pos);
}
