using System.Windows.Media;

namespace GasSimulation.Configuration.DTOs
{
    public record Brushes(
        SolidColorBrush Black, 
        SolidColorBrush Elem,
        SolidColorBrush GhostElem,
        SolidColorBrush OccupiedCell,
        SolidColorBrush PartlyOccupiedCell,
        SolidColorBrush Sector,
        SolidColorBrush Vector,
        SolidColorBrush Area);
}
