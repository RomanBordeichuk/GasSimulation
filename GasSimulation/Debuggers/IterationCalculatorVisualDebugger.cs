using GasSimulation.Configuration;
using GasSimulation.Configuration.ConfigParts;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Mappers;
using GasSimulation.Simulation.IterationCalculator;
using System.Diagnostics;

namespace GasSimulation.Debuggers
{
    public class IterationCalculatorVisualDebugger : ConcreteVisualDebugger
    {
        private const string _sectorsGroup = "SectorsGroup";
        private const string _ghostAtomsGroup = "GhostAtoms";

        public IterationCalculatorVisualDebugger(Config config, VisualDebugger debugger)
            : base(config, debugger)
        {
            if (_config.Debug!= null &&
                _config.Debug.DebugModules.Contains(ActiveVisualDebugModule.IterationCalculator))
            {
                _disabled = false;
            }
            else _disabled = true;
        }

        public bool IsDisabled()
        {
            return _disabled;
        }

        //[Conditional("DEBUG")]
        //public void CreateSector(IterationCalculator sector)
        //{
        //    if (_disabled) return;

        //    var sectorSize = GetParam<double>("SectorSize");
        //    var controlDict = GetParam<ControlDict>("ControlDict");

        //    var sectorsPos = controlDict.Dict.First(s => s.Value == sector).Key.MapToIDPos();
        //    var area = new RectState(sectorsPos.J * sectorSize + sectorSize / 2,
        //        sectorsPos.I * sectorSize + sectorSize / 2, sectorSize, sectorSize, 0);

        //    _debugger.DrawHollow(_sectorsGroup, area, 1, _config.Brushes!.Sector, null);
        //}

        [Conditional("DEBUG")]
        public void ClearSectors()
        {
            if (_disabled) return;

            _debugger.ClearGroup(_sectorsGroup);
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
