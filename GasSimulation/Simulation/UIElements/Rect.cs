using GasSimulation.Simulation.DTOs;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GasSimulation.Simulation.UIElements
{
    public class Rect : Element
    {
        private readonly double _width;
        private readonly double _height;

        public Rect(double width, double height, double angle) 
        {
            _width = width;
            _height = height;

            _obj = new Rectangle();

            _obj.Fill = _elemColorBrush;
            _obj.Width = width;
            _obj.Height = height;
            _obj.RenderTransform = new RotateTransform(angle);
            _obj.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
        }

        public override void UpdatePos(PosState pos)
        {
            Canvas.SetLeft(_obj, pos.X - _width / 2);
            Canvas.SetTop(_obj, pos.Y - _height / 2);
        }
    }
}
