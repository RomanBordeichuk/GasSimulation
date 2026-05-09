using GasSimulation.Configuration.ConfigParts;
using System.Windows.Media;

namespace GasSimulation.Mappers
{
    public static class BrushMapper
    {
        public static Configuration.DTOs.Brushes Map(this ColorsConfig colorsConfig)
        {
            var black = Map([255, 0, 0, 0]);

            var brushes = new Configuration.DTOs.Brushes(black,
                colorsConfig.Elem.Map(),
                colorsConfig.GhostElem.Map(),
                colorsConfig.OccupiedCell.Map(),
                colorsConfig.PartlyOccupiedCell.Map(),
                colorsConfig.Sector.Map(),
                colorsConfig.Vector.Map(),
                colorsConfig.Area.Map());

            return brushes;
        }

        public static SolidColorBrush Map(this byte[] color)
        {
            var brush = new SolidColorBrush(Color.FromArgb(color[0], color[1], color[2], color[3]));
            brush.Freeze();

            return brush;
        }
    }
}
