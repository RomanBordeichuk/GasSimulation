using GasSimulation.Simulation.DTOs.Interfaces;

namespace GasSimulation.Simulation.DTOs
{
    public struct RectState : IElemState
    {
        public double X
        {
            get => Pos.X;
        }

        public double Y
        {
            get => Pos.Y;
        }

        public double Width
        {
            get => Dimentions.Width;
        }

        public double Height
        {
            get => Dimentions.Height;
        }

        public PosState Pos { get; }
        public DimentState Dimentions { get; }
        public double Angle { get; }

        public RectState(double x, double y, double width, double height, double angle)
        {
            Pos = new(x, y);
            Dimentions = new(width, height);
            Angle = angle;
        }

        public RectState(PosState pos, DimentState dimentions, double angle)
        {
            Pos = pos;
            Dimentions = dimentions;
            Angle = angle;
        }
    }
}
