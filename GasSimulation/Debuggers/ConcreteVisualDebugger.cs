using GasSimulation.Configuration;
using GasSimulation.Exceptions;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.DTOs;
using System.Diagnostics;

namespace GasSimulation.Debuggers
{
    public abstract class ConcreteVisualDebugger
    {
        protected readonly Config _config;
        protected readonly VisualDebugger _debugger;

        protected bool _disabled = true;
        protected readonly Dictionary<string, object> _params = new();

        protected const string _atomsGroup = "Atoms";
        protected const string _rectsGroup = "Rects";
        protected const string _vectorsGroup = "Vectors";

        protected ConcreteVisualDebugger(Config config, VisualDebugger debugger)
        {
            _config = config;
            _debugger = debugger;

            if (_config.Debug != null &&
                _config.Debug.DebugModules.Length > 0)
            {
                _disabled = false;
            }
        }

        [Conditional("DEBUG")]
        public void SetParam<T>(string name, T value)
        {
            if (_disabled) return;

            _params[name] = value!;
        }

        [Conditional("DEBUG")]
        public void CreateAtom(AtomState atom)
        {
            if (_disabled) return;

            _debugger.Draw(_atomsGroup, atom, _config.Brushes!.Elem, null);
        }

        [Conditional("DEBUG")]
        public void CreateAtoms(AtomState[] atoms)
        {
            if (_disabled) return;

            foreach (var atom in atoms)
            {
                _debugger.Draw(_atomsGroup, atom, _config.Brushes!.Elem, null);
            }
        }

        [Conditional("DEBUG")]
        public void ClearAtoms()
        {
            if (_disabled) return;

            _debugger.ClearGroup(_atomsGroup);
        }

        [Conditional("DEBUG")]
        public void CreateRects(RectState[] rects)
        {
            if (_disabled) return;

            foreach (var rect in rects) CreateRect(rect);
        }

        [Conditional("DEBUG")]
        public void CreateRect(RectState rect)
        {
            if (_disabled) return;

            _debugger.Draw(_rectsGroup, rect, _config.Brushes!.Elem, null);
        }

        [Conditional("DEBUG")]
        public void ClearRects()
        {
            if (_disabled) return;

            _debugger.ClearGroup(_rectsGroup);
        }

        [Conditional("DEBUG")]
        public void CreateVectors(AllStates allStates)
        {
            if (_disabled) return;

            foreach (var atom in allStates.Atoms) CreateVector(atom, 1);
        }

        [Conditional("DEBUG")]
        public void CreateVectors(AllStates allStates, double t)
        {
            if (_disabled) return;

            foreach (var atom in allStates.Atoms) CreateVector(atom, t);
        }

        [Conditional("DEBUG")]
        public void CreateVector(AtomState atom)
        {
            if (_disabled) return;

            CreateVector(atom, 1);
        }

        [Conditional("DEBUG")]
        public void CreateVector(AtomState atom, double t)
        {
            if (_disabled) return;

            var vector = new VectorState(atom.Pos,
                new(atom.X + atom.Dx * t, atom.Y + atom.Dy * t), 2);

            _debugger.Draw(_vectorsGroup, vector, _config.Brushes!.Vector, 1);
        }

        [Conditional("DEBUG")]
        public void ClearVectors()
        {
            if (_disabled) return;

            _debugger.ClearGroup(_vectorsGroup);
        }

        [Conditional("DEBUG")]
        public void ClearAll()
        {
            if (_disabled) return;

            _debugger.ClearAll();
        }

        protected T GetParam<T>(string name)
        {
            if (_params.TryGetValue(name, out object? value) && value is T valueT)
            {
                return valueT;
            }
            throw new IncorrectParamException();
        }

        [Conditional("DEBUG")]
        public void BreakPoint()
        {
            if (_disabled) return;

            _debugger.BreakPoint();
        }

        [Conditional("DEBUG")]
        public void Debug()
        {
            if (_disabled) return;

            _debugger.Debug();
        }
    }
}
