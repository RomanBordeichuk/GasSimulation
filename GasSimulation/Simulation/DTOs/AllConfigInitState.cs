namespace GasSimulation.Simulation.DTOs
{
    public struct AllConfigInitState
    {
        public List<GasConfigInitState> Gas { get; init; }
        public List<List<double>> Atoms { get; init; }
        public List<List<double>> Rects { get; init; }

        public AllConfigInitState(List<GasConfigInitState> gas, 
            List<List<double>> atoms, List<List<double>> rects)
        {
            Gas = gas;
            Atoms = atoms;
            Rects = rects;
        }
    }
}
