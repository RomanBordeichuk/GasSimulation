using GasSimulation.GeneralDTOs;

namespace GasSimulation.Simulation.InitStateTransformer.DTOs
{
    public struct AtomConfigInitState
    {
        public double X
        {
            get => Pos.X;
        }
        public double Y
        {
            get => Pos.Y;
        }

        public PosState Pos { get; }
        public double Speed { get; }
        public double Angle { get; }

        public AtomConfigInitState(double x, double y, double speed, double angle)
            : this(new(x, y), speed, angle) { }

        public AtomConfigInitState(PosState pos, double speed, double angle)
        {
            Pos = pos;
            Speed = speed;
            Angle = angle;
        }
    }
}
