using GasSimulation.GeneralDTOs.Interfaces;
using System.Windows.Media;

namespace GasSimulation.Debuggers.DTOs.Interfaces
{
    public interface IDrawCommand
    {
        public IElemState Elem { get; }
        public SolidColorBrush Brush { get; }
        public int? Zindex { get; }
    }
}
