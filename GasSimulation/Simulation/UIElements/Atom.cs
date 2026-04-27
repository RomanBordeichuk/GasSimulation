using GasSimulation.Simulation.DTOs;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GasSimulation.Simulation.UIElements
{
    public class Atom : Element
    {
        public Atom(Config config)
            : base(config)
        {
            _obj = new Ellipse();

            _obj.Fill = _elemColorBrush;
            _obj.Width = _config.AtomDiameter;
            _obj.Height = _config.AtomDiameter;
        }

        public override void UpdatePos(PosState pos)
        {
            Canvas.SetLeft(_obj, pos.X - _config.AtomDiameter / 2);
            Canvas.SetTop(_obj, pos.Y - _config.AtomDiameter / 2);
        }
    }
}
