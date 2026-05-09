using GasSimulation.Configuration;
using GasSimulation.Configuration.ConfigParts;
using GasSimulation.GeneralDTOs.Atom;
using System.Diagnostics;

namespace GasSimulation.Debuggers
{
    public class SectorCalculatorVisualDebugger : ConcreteVisualDebugger
    {
        private const string _ghostAtomsGroup = "GhostAtoms";

        public SectorCalculatorVisualDebugger(Config config, VisualDebugger debugger)
            : base(config, debugger)
        {
            if (_config.Debug!= null &&
                _config.Debug.DebugModules.Contains(ActiveVisualDebugModule.SectorCalculator))
            {
                _disabled = false;
            }
            else _disabled = true;
        }

        public bool IsDisabled()
        {
            return _disabled;
        }

        [Conditional("DEBUG")]
        public void ClearGroup(string name)
        {
            if (_disabled) return;

            _debugger.ClearGroup(name);
        }

        [Conditional("DEBUG")]
        public void CreateGhostAtom(AtomState atom, double t)
        {
            if (_disabled) return;

            atom = new(atom.X + atom.Dx * t, atom.Y + atom.Dy * t, 
                atom.Dx, atom.Dy);

            _debugger.Draw(_ghostAtomsGroup, atom, _config.Brushes!.GhostElem, 1);
        }

        [Conditional("DEBUG")]
        public void ClearGhostAtoms()
        {
            if (_disabled) return;

            _debugger.ClearGroup(_ghostAtomsGroup);
        }
    }
}
