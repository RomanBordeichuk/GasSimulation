using GasSimulation.Debuggers.DTOs.Interfaces;

namespace GasSimulation.Debuggers.DTOs
{
    public class DebugStep
    {
        public List<IDrawCommand> AddedElems { get; }
        public List<IDrawCommand> DeletedElems { get; }

        public DebugStep(List<IDrawCommand> addedElems, List<IDrawCommand> deletedElems)
        {
            AddedElems = addedElems;
            DeletedElems = deletedElems;
        }

        public DebugStep()
        {
            AddedElems = new();
            DeletedElems = new();
        }

        public void AddElems(IEnumerable<IDrawCommand> elems)
        {
            foreach (IDrawCommand elem in elems) AddElem(elem);
        }

        public void AddElem(IDrawCommand elem)
        {
            if (!AddedElems.Contains(elem)) AddedElems.Add(elem);
            DeletedElems.Remove(elem);
        }

        public void RemoveElems(IEnumerable<IEnumerable<IDrawCommand>> elems)
        {
            foreach (var group in elems)
            {
                foreach (var elem in group) RemoveElem(elem);
            }
        }
        public void RemoveElems(IEnumerable<IDrawCommand> elems)
        {
            foreach (IDrawCommand elem in elems) RemoveElem(elem);
        }

        public void RemoveElem(IDrawCommand elem)
        {
            if (!DeletedElems.Contains(elem)) DeletedElems.Add(elem);
            AddedElems.Remove(elem);
        }
    }
}
