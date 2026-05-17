using GasSimulation.Debuggers.DTOs.Interfaces;
using GasSimulation.GeneralDTOs.Interfaces;
using System.Windows.Media;

namespace GasSimulation.Debuggers.DTOs
{
    public struct DrawCommand : IDrawCommand
    {
        public IElemState Elem { get; }
        public SolidColorBrush Brush { get; }
        public int? Zindex { get; }
        public string GroupName { get; }

        public DrawCommand(IElemState elem, SolidColorBrush brush, 
            int? zindex, string groupName)
        {
            Elem = elem;
            Brush = brush;
            Zindex = zindex;
            GroupName = groupName;
        }
    }
}
