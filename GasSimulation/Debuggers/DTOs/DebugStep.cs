using GasSimulation.Debuggers.DTOs.Interfaces;

namespace GasSimulation.Debuggers.DTOs
{
    public struct DebugStep
    {
        public List<IDrawCommand> AddedElems { get; } = new();
        public List<IDrawCommand> DeletedElems { get; } = new();

        public DebugStep() { }

        public void DeleteElems(List<IDrawCommand> elems)
        {
            foreach (var elem in elems)
            {
                AddedElems.Remove(elem);
            }

            DeletedElems.AddRange(elems);
        }

        public void DeleteElems(Dictionary<string, List<IDrawCommand>> groups)
        {
            foreach (var group in groups)
            {
                DeleteElems(group.Value);
            }
        }
    }
}
