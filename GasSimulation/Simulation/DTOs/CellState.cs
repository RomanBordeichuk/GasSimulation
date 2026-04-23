namespace GasSimulation.Simulation.DTOs
{
    public struct CellState
    {
        public double X
        {
            get => Pos.X;
        }
        public double Y
        {
            get => Pos.Y;
        }

        public PosState Pos { get; }
        public List<PosState> NearNeighbs { get; }

        public CellState(double x, double y)
            : this(new(x, y)) { }

        public CellState(PosState pos)
        {
            Pos = pos;

            NearNeighbs = new();
        }
    }
}
