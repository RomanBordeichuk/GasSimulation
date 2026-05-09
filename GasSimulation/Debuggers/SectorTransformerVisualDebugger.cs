using GasSimulation.Configuration;
using GasSimulation.Configuration.ConfigParts;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;
using System.Diagnostics;

namespace GasSimulation.Debuggers
{
    public class SectorTransformerVisualDebugger : ConcreteVisualDebugger
    {
        private const string _areasGroup = "Areas";
        private const string _sectorsGroup = "Sectors";
        private const string _ghostAtomsGroup = "GhostAtoms";

        public SectorTransformerVisualDebugger(Config config, VisualDebugger debugger)
            : base(config, debugger)
        {
            if (_config.Debug != null &&
                _config.Debug.DebugModules.Contains(ActiveVisualDebugModule.SectorCalculator))
            {
                _disabled = false;
            }
            else _disabled = true;
        }

        [Conditional("DEBUG")]
        public void ClearGroup(string name)
        {
            if (_disabled) return;

            _debugger.ClearGroup(name);
        }

        public void CreateArea(RectState area)
        {
            if (_disabled) return;

            _debugger.Draw(_areasGroup, area, _config.Brushes!.Area, -1);
        }

        [Conditional("DEBUG")]
        public void ClearAreas()
        {
            if (_disabled) return;

            _debugger.ClearGroup(_areasGroup);
        }

        [Conditional("DEBUG")]
        public void CreateSector(SectorState sector)
        {
            if (_disabled) return;

            var sectorSize = GetParam<double>("SectorSize");

            var area = new RectState(sector.J * sectorSize + sectorSize / 2, 
                sector.I * sectorSize + sectorSize / 2, sectorSize, sectorSize, 0);

            _debugger.DrawHollow(_sectorsGroup, area, 1,
                _config.Brushes!.Sector, null);
        }

        [Conditional("DEBUG")]
        public void ClearSectors()
        {
            if (_disabled) return;

            _debugger.ClearGroup(_sectorsGroup);
        }

        [Conditional("DEBUG")]
        public void CreateGhostAtom(AtomState atom)
        {
            if (_disabled) return;

            _debugger.Draw(_ghostAtomsGroup, atom, _config.Brushes!.GhostElem, null);
        }

        [Conditional("DEBUG")]
        public void ClearGhostAtoms()
        {
            if (_disabled) return;

            _debugger.ClearGroup(_ghostAtomsGroup);
        }
    }
}
