using GasSimulation.Configuration;
using GasSimulation.Debuggers;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Mappers;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.DTOs.Sectors;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.IterationCalculator
{
    public class SectorPartitioner
    {
        private readonly Config _config;
        private readonly SectorPartitionerVisualDebugger _debugger;

        private SectorStates _sectors = null!;

        public SectorPartitioner(Config config, SectorPartitionerVisualDebugger debugger)
        {
            _config = config;
            _debugger = debugger;
        }

        public SectorStates Partition(AllStates allStates)
        {
            var velocities = allStates.Atoms.Select(a => a.Velocity);
            var speeds = velocities.Select(v => MathHelper.DecomposeVelocity(v).speed).ToList();
            var avSpeed = speeds.Sum() / speeds.Count;
            var sectorSize = Math.Max(avSpeed, _config.Simulation.AtomDiameter) * 
                _config.Simulation.SectorSizeMult;

            _debugger.SetParam<double>("SectorSize", sectorSize);
            _debugger.SetParam<AtomState[]>("Atoms", allStates.Atoms);
            _debugger.SetParam<RectState[]>("Rects", allStates.Rects);

            _sectors = new SectorStates(new(), sectorSize);

            for (int i = 0; i < allStates.Atoms.Length; i++)
            {
                var atom = allStates.Atoms[i];

                var velocityArea = CalculateVelocityArea(atom);
                var belongedSects = CalculateBelongedSects(velocityArea);

                if (belongedSects.Length == 1)
                    AddToSector(belongedSects[0], i);
                else
                    AddGhostToSectors(belongedSects, i);
            }

            for (int i = 0; i < allStates.Rects.Length; i++)
            {
                var rect = allStates.Rects[i];

                var area = CalculateVelocityArea(rect);
                var belongedSects = CalculateBelongedSects(area);

                AddToSectors(belongedSects, i);
            }

            _debugger.BreakPoint();
            _debugger.ClearAll();
            _debugger.BreakPoint();
            _debugger.Debug();

            return _sectors;
        }

        private RectState CalculateVelocityArea(AtomState atom)
        {
            var x = atom.X + atom.Dx / 2;
            var y = atom.Y + atom.Dy / 2;
            var width = _config.Simulation.AtomDiameter + Math.Abs(atom.Dx);
            var height = _config.Simulation.AtomDiameter + Math.Abs(atom.Dy);

            return new(x, y, width, height, 0);
        }

        private static RectState CalculateVelocityArea(RectState rect)
        {
            double areaWidth = rect.Width * Math.Abs(Math.Cos(rect.Angle)) +
                rect.Height * Math.Abs(Math.Sin(rect.Angle));

            double areaHeight = rect.Width * Math.Abs(Math.Sin(rect.Angle)) +
                rect.Height * Math.Abs(Math.Cos(rect.Angle));

            return new RectState(rect.X, rect.Y, areaWidth, areaHeight, 0);
        }

        private IDPosState[] CalculateBelongedSects(RectState velocityArea)
        {
            int[] jmass = CalculateBelongedSectOnLine(velocityArea.X, velocityArea.Width, _sectors.SectorSize);
            int[] imass = CalculateBelongedSectOnLine(velocityArea.Y, velocityArea.Height, _sectors.SectorSize);

            IDPosState[] res = new IDPosState[imass.Length * jmass.Length];

            for (int i = 0; i < imass.Length; i++)
            {
                for (int j = 0; j < jmass.Length; j++)
                {
                    res[i * jmass.Length + j] = new(imass[i], jmass[j]);
                }
            }

            return res;
        }

        private static int[] CalculateBelongedSectOnLine(double x, double width, double step)
        {
            int leftEdgeId = (int)Math.Floor((x - width / 2) / step);
            int rightEdgeId = (int)Math.Floor((x + width / 2) / step);

            List<int> sects = new();

            for (int i = leftEdgeId; i <= rightEdgeId; i++) sects.Add(i);

            return sects.ToArray();
        }

        private void AddToSector(IDPosState sectId, int atomId)
        {
            _debugger.CreateAtom(atomId);
            _debugger.CreateVector(atomId);
            _debugger.BreakPoint();

            bool foundSector = _sectors.Sects.TryGetValue(sectId.MapToLong(), out var sector);

            if (foundSector) sector!.AtomIds.Add(atomId);
            else
            {
                var newSector = new SectorState();
                newSector.AtomIds.Add(atomId);

                _debugger.CreateSector(sectId);
                _debugger.BreakPoint();

                _sectors.Sects.Add(sectId.MapToLong(), newSector);
            }
        }

        private void AddGhostToSectors(IDPosState[] sectIds, int atomId)
        {
            _debugger.CreateGhostAtom(atomId);
            _debugger.CreateVector(atomId);
            _debugger.BreakPoint();

            foreach (var sectId in sectIds)
            {
                bool foundSector = _sectors.Sects.TryGetValue(sectId.MapToLong(), out var sector);

                if (foundSector) sector!.AtomIds.Add(atomId);
                else
                {
                    var newSector = new SectorState();
                    newSector.AtomIds.Add(atomId);

                    _debugger.CreateSector(sectId);
                    _debugger.BreakPoint();

                    _sectors.Sects.Add(sectId.MapToLong(), newSector);
                }
            }
        }

        private void AddToSectors(IDPosState[] sectIds, int rectId)
        {
            _debugger.CreateRect(rectId);
            _debugger.BreakPoint();

            foreach (var sectId in sectIds)
            {
                bool foundSector = _sectors.Sects.TryGetValue(sectId.MapToLong(), out var sector);

                if (foundSector) sector!.RectIds.Add(rectId);
                else
                {
                    var newSector = new SectorState();
                    newSector.RectIds.Add(rectId);

                    _debugger.CreateSector(sectId);
                    _debugger.BreakPoint();

                    _sectors.Sects.Add(sectId.MapToLong(), newSector);
                }
            }
        }
    }
}
