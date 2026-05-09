using GasSimulation.Configuration;
using GasSimulation.Debuggers.DTOs;
using GasSimulation.Debuggers.DTOs.Interfaces;
using GasSimulation.Exceptions;
using GasSimulation.GeneralDTOs.Interfaces;
using GasSimulation.UIRendering;
using System.Windows.Media;

namespace GasSimulation.Debuggers
{
    public class VisualDebugger
    {
        private readonly Config _config;
        private readonly DebugCanvas? _canvas;

        private TaskCompletionSource _debugWaitHandler = null!;
        private readonly Dictionary<string, List<IDrawCommand>> _groupsList = new();
        private readonly DebugStep[] _debugSteps;
        private int _currentStep = 0;

        public VisualDebugger(Config config, DebugCanvas? canvas = null)
        {
            _config = config;
            _canvas = canvas;

            if (canvas == null)
            {
                _debugSteps = [];
                return;
            }

            _debugSteps = new DebugStep[_config.Debug!.DebugSteps];

            for (int i = 0; i < _debugSteps.Length; i++)
            {
                _debugSteps[i] = new();
            }
        }

        public void ClearGroup(string name)
        {
            if (!_groupsList.TryGetValue(name, out var currentGroup)) return;

            _debugSteps[0].DeleteElems(currentGroup);
            _groupsList.Remove(name);

            _canvas!.Clear(currentGroup);
        }

        public void ClearAll()
        {
            _debugSteps[0].DeleteElems(_groupsList);
            _groupsList.Clear();

            _canvas!.ClearAll();
        }

        public void Draw<T>(string groupName, T item, 
            SolidColorBrush brush, int? zindex)
            where T : IElemState
        {
            var command = new DrawCommand(item, brush, zindex);

            AddToGroups(groupName, command);
            _debugSteps[0].AddedElems.Add(command);

            _canvas!.Draw(command);
        }

        public void DrawHollow<T>(string groupName, T item, 
            double border, SolidColorBrush brush, int? zindex)
            where T : IElemState
        {
            var command = new DrawHollowCommand(item, border, brush, zindex);

            AddToGroups(groupName, command);
            _debugSteps[0].AddedElems.Add(command);

            _canvas!.DrawHollow(command);
        }

        public async ValueTask Stop()
        {
            _debugWaitHandler = new();
            await _debugWaitHandler.Task;
        }

        public void StepNext()
        {
            if (_currentStep == 0)
            {
                InitNewStep();
                _debugWaitHandler.TrySetResult();
            }
            else
            {
                _currentStep--;
                RedrawStep(_debugSteps[_currentStep]);
            }
        }

        public void StepBack()
        {
            if (_currentStep < _config.Debug!.DebugSteps - 1)
            {
                RollbackStep(_debugSteps[_currentStep]);
                _currentStep++;
            }
        }

        private void AddToGroups(string groupName, IDrawCommand command)
        {
            if (_groupsList.TryGetValue(groupName, out var group))
            {
                group.Add(command);

                return;
            }

            _groupsList.Add(groupName, new() { command });
        }

        private void InitNewStep()
        {
            for (int i = _debugSteps.Length - 1; i > 0; i--)
            {
                _debugSteps[i] = _debugSteps[i - 1];
            }

            _debugSteps[0] = new();
        }

        private void RollbackStep(DebugStep step)
        {
            foreach (var elem in step.AddedElems)
            {
                _canvas!.Clear(elem);
            }

            foreach (var command in step.DeletedElems)
            {
                if (command is DrawCommand drawCommand)
                {
                    _canvas!.Draw(drawCommand);
                }
                else if (command is DrawHollowCommand drawHollowCommand)
                {
                    _canvas!.DrawHollow(drawHollowCommand);
                }
                else throw new IncorrectTypeException();
            }
        }

        private void RedrawStep(DebugStep step)
        {
            foreach (var elem in step.DeletedElems)
            {
                _canvas!.Clear(elem);
            }

            foreach (var command in step.AddedElems)
            {
                if (command is DrawCommand drawCommand)
                {
                    _canvas!.Draw(drawCommand);
                }
                else if (command is DrawHollowCommand drawHollowCommand)
                {
                    _canvas!.DrawHollow(drawHollowCommand);
                }
                else throw new IncorrectTypeException();
            }
        }
    }
}
