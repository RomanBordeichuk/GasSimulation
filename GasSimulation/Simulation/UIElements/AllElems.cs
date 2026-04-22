namespace GasSimulation.Simulation.UIElements
{
    public class AllElems
    {
        public List<Atom> Atoms { get; }
        public List<Rect> Rects { get; }

        public AllElems()
        {
            Atoms = new();
            Rects = new();
        }
    }
}
