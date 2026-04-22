using GasSimulation.Simulation.DTOs;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GasSimulation.Simulation.UIElements
{
    public class Atom : Element
    {
        public Atom()
        {
            _obj = new Ellipse();

            _obj.Fill = _elemColorBrush;
            _obj.Width = Constants.AtomDiameter;
            _obj.Height = Constants.AtomDiameter;
        }

        public override void UpdatePos(PosState pos)
        {
            Canvas.SetLeft(_obj, pos.X - Constants.AtomDiameter / 2);
            Canvas.SetTop(_obj, pos.Y - Constants.AtomDiameter / 2);
        }
    }
}
