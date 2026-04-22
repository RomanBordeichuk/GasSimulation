using GasSimulation.Simulation.DTOs;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GasSimulation.Simulation.UIElements
{
    public abstract class Element
    {
        protected static readonly Color _elemColor = (Color)(ColorConverter.ConvertFromString(Constants.AtomColorHex))!;
        protected static readonly SolidColorBrush _elemColorBrush = new SolidColorBrush(_elemColor);

        protected Shape _obj = null!;

        public Shape Obj
        {
            get => _obj;
        }

        public abstract void UpdatePos(PosState pos);
    }
}
