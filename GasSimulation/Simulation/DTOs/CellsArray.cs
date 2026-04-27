namespace GasSimulation.Simulation.DTOs
{
    public struct CellsArray
    {
        public CellState[] Array { get; }
        public int Width { get; }
        public int Height { get; }

        public CellsArray(int width, int height)
        {
            Width = width;
            Height = height;

            Array = new CellState[width * height];
        }
    }
}
