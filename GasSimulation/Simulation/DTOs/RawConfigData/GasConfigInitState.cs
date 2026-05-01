namespace GasSimulation.Simulation.DTOs.Config
{
    public struct GasConfigInitState
    {
        public List<double> Area { get; init; }
        public int NumAtoms { get; init; }
        public double AtomSpeed { get; init; }

        public GasConfigInitState(List<double> area, int numAtoms, double atomSpeed)
        {
            Area = area;
            NumAtoms = numAtoms;
            AtomSpeed = atomSpeed;
        }
    }
}
