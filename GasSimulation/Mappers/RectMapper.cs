using GasSimulation.Configuration;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Mappers
{
    public static class RectMapper
    {
        public static RectState MapToState(this RectState rectState, Config config)
        {
            PosState newPos = MathHelper.TranslateField(
                rectState.Pos, new(-config.Simulation.StartPosX, 
                -config.Simulation.StartPosY));

            double angleRad = MathHelper.TransformAngleToRAD(rectState.Angle);

            return new(newPos, new(rectState.Width, rectState.Height), angleRad);
        }

        public static List<RectState> MapToStates(this List<RectState> rectStates, Config config)
        {
            List<RectState> rects = new();

            foreach (var rectState in rectStates)
            {
                rects.Add(rectState.MapToState(config));
            }

            return rects;
        }
    }
}
