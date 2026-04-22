namespace GasSimulation.Simulation.DTOs
{
    public struct PosState
    { 
        public double X { get; }
        public double Y { get; }

        public PosState(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
