using GasSimulation.Simulation.DTOs.Interfaces;

namespace GasSimulation.Simulation.DTOs
{
    public struct AtomState : IElemState
    {
        public double X
        {
            get => Pos.X;
        }
        public double Y
        {
            get => Pos.Y;
        }
        public double Dx
        {
            get => Velocity.Dx;
        }
        public double Dy
        {
            get => Velocity.Dy;
        }

        public PosState Pos { get; }
        public VelocityState Velocity { get; }

        public AtomState(PosState pos, VelocityState velocity)
        {
            Pos = pos;
            Velocity = velocity;
        }

        public AtomState(double x, double y, double dx, double dy)
        {
            Pos = new(x, y);
            Velocity = new(dx, dy);
        }
    }
}
