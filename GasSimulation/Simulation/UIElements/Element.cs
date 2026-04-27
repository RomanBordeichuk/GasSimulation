using GasSimulation.Simulation.DTOs;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GasSimulation.Simulation.UIElements
{
    public abstract class Element
    {
        protected Config _config;
        protected SolidColorBrush _elemColorBrush;

        protected Shape _obj = null!;

        public Shape Obj
        {
            get => _obj;
        }

        public abstract void UpdatePos(PosState pos);

        protected Element(Config config)
        {
            _config = config;

            var elemColor = (Color)(ColorConverter.ConvertFromString(config.AtomColorHex))!;
            _elemColorBrush = new SolidColorBrush(elemColor);
        }
    }
}
