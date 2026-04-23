namespace GasSimulation.Simulation.DTOs
{
    public struct AtomInitState
    {
        public double X { get; }
        public double Y { get; }
        public double Speed { get; }
        public double Angle { get; }

        public AtomInitState(double x, double y, double speed, double angle)
        {
            X = x;
            Y = y;
            Speed = speed;
            Angle = angle;
        }
    }
}
