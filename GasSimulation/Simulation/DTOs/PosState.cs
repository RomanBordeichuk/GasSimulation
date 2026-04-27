namespace GasSimulation.Simulation.DTOs
{
    public struct PosState
    { 
        public double X { get; init; }
        public double Y { get; init; }

        public PosState(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
