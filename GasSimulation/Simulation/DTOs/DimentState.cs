namespace GasSimulation.Simulation.DTOs
{
    public struct DimentState
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public DimentState(double width, double height)
        {
            Width = width;
            Height = height;
        }
    }
}
