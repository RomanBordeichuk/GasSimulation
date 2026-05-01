using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

namespace GasSimulation
{
    public record Config
    {
        private readonly double _mult;
        private readonly int _precision;
        private readonly SolidColorBrush _brush;
        private readonly SolidColorBrush _blackBrush;
        private readonly double _sqrt;

        private bool _settedRand = false;
        private Random _rand = new Random();

        public double FPS { get; }
        public double SpeedMult { get; }
        public double StartPosX { get; }
        public double StartPosY { get; }
        public double Restitution { get; }
        public double ErrorRate { get; }
        public double AtomDiameter { get; }
        public int PasteAtomAttempts { get; }
        public string AtomColorHex { get; } = null!;


        [SuppressMessage("SonarLint", "S107")]
        public Config(double fps, double speedMult, double startPosX, double startPosY, 
            double restitution, double errorRate, double atomDiameter, int pasteAtomAttempts, 
            string atomColorHex)
        {
            FPS = fps; 
            SpeedMult = speedMult;
            StartPosX = startPosX;
            StartPosY = startPosY;
            Restitution = restitution;
            ErrorRate = errorRate;
            AtomDiameter = atomDiameter;
            PasteAtomAttempts = pasteAtomAttempts;
            AtomColorHex = atomColorHex;

            _mult = speedMult / fps;
            _precision = (int)-Math.Log10(errorRate);

            _brush = new SolidColorBrush((Color)(ColorConverter.ConvertFromString(atomColorHex)));
            _brush.Freeze();

            _blackBrush = new SolidColorBrush(Colors.Black);
            _blackBrush.Freeze();

            _sqrt = Math.Sqrt(2);
        }

        public double Mult => _mult;
        public int Precision => _precision;
        public SolidColorBrush ElemBrush => _brush;
        public SolidColorBrush BlackBrush => _blackBrush;
        public double Sqrt2 => _sqrt;

        public Random Rand
        {
            get => _rand;
            set
            {
                if (_settedRand) throw new InvalidOperationException();
                else
                {
                    _rand = value;
                    _settedRand = true;
                }
            }
        }
    }
}
