namespace GasSimulation.GeneralDTOs
{
    public struct VelocityState
    {
        public double Dx { get; init; }
        public double Dy { get; init; }

        public VelocityState(double dx, double dy)
        {
            Dx = dx;
            Dy = dy;
        }
    }
}
