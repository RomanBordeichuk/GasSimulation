using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

namespace GasSimulation
{
    public record Config
    {
        private readonly double _mult;
        private readonly int _precision;
        private readonly double _sqrt;

        private readonly SolidColorBrush _blackBrush;
        private readonly SolidColorBrush _elemBrush;
        private readonly SolidColorBrush _occupiedCellBrush;
        private readonly SolidColorBrush _partlyOccupiedCellBrush;
        private readonly SolidColorBrush _sectorBrush;

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

        public byte[] ElemColorHex { get; } = null!;
        public byte[] OccupiedCellColorHex { get; } = null!;
        public byte[] PartlyOccupiedCellColorHex { get; } = null!;
        public byte[] SectorColorHex { get; } = null!;


        [SuppressMessage("SonarLint", "S107")]
        public Config(double fps, double speedMult, double startPosX, double startPosY, 
            double restitution, double errorRate, double atomDiameter, int pasteAtomAttempts, 
            byte[] elemColorHex, byte[] occupiedCellColorHex, byte[] partlyOccupiedCellColorHex,
            byte[] sectorColorHex)
        {
            FPS = fps; 
            SpeedMult = speedMult;
            StartPosX = startPosX;
            StartPosY = startPosY;
            Restitution = restitution;
            ErrorRate = errorRate;
            AtomDiameter = atomDiameter;
            PasteAtomAttempts = pasteAtomAttempts;

            DebuggerWaitHandler = new TaskCompletionSource();

            _mult = speedMult / fps;
            _precision = (int)-Math.Log10(errorRate);

            _blackBrush = CreateBrush([1, 0, 0, 0]);
            _blackBrush.Freeze();

            ElemColorHex = elemColorHex;
            _elemBrush = CreateBrush(elemColorHex);
            _elemBrush.Freeze();

            OccupiedCellColorHex = occupiedCellColorHex;
            _occupiedCellBrush = CreateBrush(occupiedCellColorHex);
            _occupiedCellBrush.Freeze();

            PartlyOccupiedCellColorHex = partlyOccupiedCellColorHex;
            _partlyOccupiedCellBrush = CreateBrush(partlyOccupiedCellColorHex);
            _partlyOccupiedCellBrush.Freeze();

            SectorColorHex = sectorColorHex;
            _sectorBrush = CreateBrush(sectorColorHex);
            _sectorBrush.Freeze();

            _sqrt = Math.Sqrt(2);
        }

        public TaskCompletionSource DebuggerWaitHandler { get; set; }

        public double Mult => _mult;
        public int Precision => _precision;
        public double Sqrt2 => _sqrt;

        public SolidColorBrush BlackBrush => _blackBrush;
        public SolidColorBrush ElemBrush => _elemBrush;
        public SolidColorBrush OccupiedCellBrush => _occupiedCellBrush;
        public SolidColorBrush PartlyOccupiedCellBrush => _partlyOccupiedCellBrush;
        public SolidColorBrush SectorBrush => _sectorBrush;

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

        private static SolidColorBrush CreateBrush(byte[] color)
        {
            return new SolidColorBrush(Color.FromArgb(color[0], color[1], color[2], color[3]));
        }
    }
}
