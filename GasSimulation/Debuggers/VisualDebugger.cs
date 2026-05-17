using GasSimulation.Debuggers.DTOs;
using GasSimulation.Debuggers.DTOs.Interfaces;
using GasSimulation.Exceptions;
using GasSimulation.GeneralDTOs.Interfaces;
using GasSimulation.UIRendering;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

namespace GasSimulation.Debuggers
{
    public class VisualDebugger
    {
        private readonly ManualResetEventSlim _coreResetEvent;
        private readonly DebugCanvas? _canvas;

        private readonly ConcurrentQueue<IDebugCommand> _commandsQueue;

        private List<DebugStep> _debugSteps = null!;
        private int _currentStepId = 0;

        public VisualDebugger(ManualResetEventSlim coreResetEvent, 
            DebugCanvas? canvas = null)
        {
            _coreResetEvent = coreResetEvent;
            _canvas = canvas;

            _commandsQueue = new();
        }

        public void ClearGroup(string name)
        {
            _commandsQueue.Enqueue(new ClearGroupCommand(name));
        }

        public void ClearAll()
        {
            _commandsQueue.Enqueue(new ClearAllCommand());
        }

        public void Draw<T>(string groupName, T item, 
            SolidColorBrush brush, int? zindex)
            where T : IElemState
        {
            _commandsQueue.Enqueue(new DrawCommand(item, brush, zindex, groupName));
        }

        public void DrawHollow<T>(string groupName, T item, 
            double border, SolidColorBrush brush, int? zindex)
            where T : IElemState
        {
            _commandsQueue.Enqueue(new DrawHollowCommand(item, border, brush, zindex, groupName));
        }

        public void BreakPoint()
        {
            _commandsQueue.Enqueue(new BreakPointCommand());
        }

        public void Debug()
        {
            PrepareToDebug();

            _coreResetEvent.Reset();
            _coreResetEvent.Wait();
        }

        public void StepNext()
        {
            if (_currentStepId < _debugSteps.Count)
            {
                ExecuteStep(_debugSteps[_currentStepId]);
                _currentStepId++;
            }
            else EndDebug();
        }

        public void StepBack()
        {
            if (_currentStepId > 0)
            {
                _currentStepId--;
                RollbackStep(_debugSteps[_currentStepId]);
            }
        }

        [SuppressMessage("SonarLint", "S3776")]
        private void PrepareToDebug()
        {
            _currentStepId = 0;
            _debugSteps = new();

            var groups = new Dictionary<string, List<IDrawCommand>>();
            var debugStep = new DebugStep();

            while (_commandsQueue.TryDequeue(out var command))
            {
                switch (command)
                {
                    case IDrawCommand draw:
                        {
                            AddToGroups(groups, draw);
                            debugStep.AddElem(draw);

                            break;
                        }

                    case ClearGroupCommand clearGroup:
                        {
                            var group = DeleteGroup(groups, clearGroup.GroupName);
                            debugStep.RemoveElems(group);

                            break;
                        }

                    case ClearAllCommand:
                        {
                            debugStep.RemoveElems(groups.Values);
                            groups.Clear();

                            break;
                        }

                    case BreakPointCommand:
                        {
                            _debugSteps.Add(debugStep);
                            debugStep = new();

                            break;
                        }

                    default: throw new IncorrectTypeException();
                }
            }
        }

        private static void AddToGroups(Dictionary<string, List<IDrawCommand>> groups, IDrawCommand elem)
        {
            if (groups.TryGetValue(elem.GroupName, out var group))
            {
                group.Add(elem);

                return;
            }

            groups.Add(elem.GroupName, new() { elem });
        }

        private static List<IDrawCommand> DeleteGroup(Dictionary<string, List<IDrawCommand>> groups, 
            string groupName)
        {
            if (groups.TryGetValue(groupName, out var group))
            {
                groups.Remove(groupName);
                return group;
            }

            throw new IncorrectGroupException();
        }

        private void ExecuteStep(DebugStep debugStep)
        {   
            foreach (var elem in debugStep.AddedElems)
            {
                if (elem is DrawCommand draw) _canvas!.Draw(draw);
                else if (elem is DrawHollowCommand drawHollow) _canvas!.DrawHollow(drawHollow);
                else throw new IncorrectTypeException();
            }

            foreach (var elem in debugStep.DeletedElems)
            {
                _canvas!.Clear(elem);
            }
        }

        private void RollbackStep(DebugStep debugStep)
        {
            foreach (var elem in debugStep.DeletedElems)
            {
                if (elem is DrawCommand draw) _canvas!.Draw(draw);
                else if (elem is DrawHollowCommand drawHollow) _canvas!.DrawHollow(drawHollow);
                else throw new IncorrectTypeException();
            }

            foreach (var elem in debugStep.AddedElems)
            {
                _canvas!.Clear(elem);
            }
        }

        private void EndDebug()
        {
            _coreResetEvent.Set();
        }
    }

    public record struct ComplexKey(object Id, string GroupName); 
}
