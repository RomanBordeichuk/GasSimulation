namespace GasSimulation.Simulation.DTOs.Config
{
    public struct ConfigInitState
    {
        public List<GasConfigInitState> Gas { get; init; }
        public List<List<double>> Atoms { get; init; }
        public List<List<double>> Rects { get; init; }

        public ConfigInitState(List<GasConfigInitState> gas, 
            List<List<double>> atoms, List<List<double>> rects)
        {
            Gas = gas;
            Atoms = atoms;
            Rects = rects;
        }
    }
}
