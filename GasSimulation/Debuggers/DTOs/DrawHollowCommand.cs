using GasSimulation.Debuggers.DTOs.Interfaces;
using GasSimulation.GeneralDTOs.Interfaces;
using System.Windows.Media;

namespace GasSimulation.Debuggers.DTOs
{
    public struct DrawHollowCommand : IDrawCommand
    {
        public IElemState Elem { get; }
        public double Border { get; }
        public SolidColorBrush Brush { get; }
        public int? Zindex { get; }

        public DrawHollowCommand(IElemState elem, 
            double bodrer, SolidColorBrush brush, int? zindex)
        {
            Elem = elem;
            Border = bodrer;
            Brush = brush;
            Zindex = zindex;
        }
    }
}
