using GasSimulation.Configuration;
using GasSimulation.Configuration.ConfigParts;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using System.Diagnostics;
using System.Windows;

namespace GasSimulation.Debuggers
{
    public class SectorPartitionerVisualDebugger : ConcreteVisualDebugger
    {
        private const string _sectorsGroup = "Sectors";
        private const string _ghostAtomsGroup = "GhostAtoms";

        public SectorPartitionerVisualDebugger(Config config, VisualDebugger debugger)
            : base(config, debugger)
        {
            if (_config.Debug != null &&
                _config.Debug.DebugModules.Contains(ActiveVisualDebugModule.SectorTransformer))
            {
                _disabled = false;
            }
            else _disabled = true;
        }

        [Conditional("DEBUG")]
        public void CreateAtom(int atomId)
        {
            if (_disabled) return;

            var atoms = GetParam<AtomState[]>("Atoms");

            _debugger.Draw(_atomsGroup, atoms[atomId], _config.Brushes!.Elem, null);
        }

        [Conditional("DEBUG")]
        public void CreateRect(int rectId)
        {
            if (_disabled) return;

            var rects = GetParam<RectState[]>("Rects");

            _debugger.Draw(_rectsGroup, rects[rectId], _config.Brushes!.Elem, null);
        }

        [Conditional("DEBUG")]
        public void CreateVector(int atomId)
        {
            if (_disabled) return;

            var atoms = GetParam<AtomState[]>("Atoms");

            var atom = atoms[atomId];
            var vector = new VectorState(atom.Pos,
                new(atom.X + atom.Dx, atom.Y + atom.Dy), 2);

            _debugger.Draw(_vectorsGroup, vector, _config.Brushes!.Vector, 1);
        }

        [Conditional("DEBUG")]
        public void CreateSector(IDPosState posState)
        {
            if (_disabled) return;

            var sectorSize = GetParam<double>("SectorSize");

            var area = new RectState(posState.J * sectorSize + sectorSize / 2, 
                posState.I * sectorSize + sectorSize / 2, sectorSize, sectorSize, 0);

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
        public void CreateGhostAtom(int atomId)
        {
            if (_disabled) return;

            var atoms = GetParam<AtomState[]>("Atoms");

            _debugger.Draw(_ghostAtomsGroup, atoms[atomId], _config.Brushes!.GhostElem, null);
        }

        [Conditional("DEBUG")]
        public void ClearGhostAtoms()
        {
            if (_disabled) return;

            _debugger.ClearGroup(_ghostAtomsGroup);
        }
    }
}
