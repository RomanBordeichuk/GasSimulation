namespace GasSimulation.Simulation.DTOs
{
    public struct VelocityState
    {
        public double Dx { get; }
        public double Dy { get; }

        public VelocityState(double dx, double dy)
        {
            Dx = dx;
            Dy = dy;
        }
    }
}
