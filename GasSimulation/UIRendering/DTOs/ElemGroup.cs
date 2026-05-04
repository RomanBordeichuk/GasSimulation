using System.Windows.Shapes;

namespace GasSimulation.UIRendering.DTOs
{
    public struct ElemGroup
    {
        public string Name { get; }
        public List<Shape> Elems { get; } = new();

        public ElemGroup(string name)
        {
            Name = name;
        }
    }
}
