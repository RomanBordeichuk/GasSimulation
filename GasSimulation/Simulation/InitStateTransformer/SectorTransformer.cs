using GasSimulation.Configuration;
using GasSimulation.Debuggers;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.InitStateTransformer
{
    public class SectorTransformer
    {
        private readonly Config _config;
        private readonly SectorTransformerVisualDebugger _debugger;

        private SectorStates _sectors;

        public SectorTransformer(Config config, SectorTransformerVisualDebugger debugger)
        {
            _config = config;
            _debugger = debugger;
        }

        public async ValueTask<SectorStates> Transform(AllStates allStates)
        {
            var velocities = allStates.Atoms.Select(a => a.Velocity);
            var speeds = velocities.Select(v => MathHelper.DecomposeVelocity(v).speed).ToList();
            var avSpeed = speeds.Sum() / speeds.Count;
            var sectorSize = Math.Max(avSpeed, _config.Simulation.AtomDiameter) * 
                _config.Simulation.SectorSizeMult;

            _debugger.SetParam<double>("SectorSize", sectorSize);

            _sectors = new SectorStates(new(), sectorSize);

            foreach (var atom in allStates.Atoms)
            {
                var velocityArea = CalculateVelocityArea(atom);
                var belongedSects = CalculateBelongedSects(velocityArea, sectorSize);

                if (belongedSects.Length == 1)
                    await AddToSector(belongedSects[0], atom);
                else
                    await AddGhostToSectors(belongedSects, atom);
            }

            foreach (var rect in allStates.Rects)
            {
                var area = CalculateVelocityArea(rect);
                var belongedSects = CalculateBelongedSects(area, sectorSize);

                await AddToSectors(belongedSects, rect);
            }

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

        private static (int i, int j)[] CalculateBelongedSects(RectState velocityArea, double sectorSize)
        {
            int[] jmass = CalculateBelongedSectOnLine(velocityArea.X, velocityArea.Width, sectorSize);
            int[] imass = CalculateBelongedSectOnLine(velocityArea.Y, velocityArea.Height, sectorSize);

            (int i, int j)[] res = new (int i, int j)[imass.Length * jmass.Length];

            for (int i = 0; i < imass.Length; i++)
            {
                for (int j = 0; j < jmass.Length; j++)
                {
                    res[i * jmass.Length + j] = (imass[i], jmass[j]);
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

        private async ValueTask AddToSector((int i, int j) sect, AtomState atom)
        {
            _debugger.CreateAtom(atom);
            _debugger.CreateVector(atom);
            await _debugger.Stop();

            bool foundSector = false;

            foreach (var sector in _sectors.Sectors)
            {
                if (sect.i == sector.I && sect.j == sector.J)
                {
                    sector.AllStates.Atoms.Add(atom);
                    foundSector = true;

                    break;
                }
            }

            if (!foundSector)
            {
                var newAllStates = new AllStates(new List<AtomState>(), new List<RectState>());
                newAllStates.Atoms.Add(atom);

                var newSector = new SectorState(newAllStates, new List<AtomState>(), sect.i, sect.j);

                _debugger.CreateSector(newSector);
                await _debugger.Stop();

                _sectors.Sectors.Add(newSector);
            }
        }

        private async ValueTask AddGhostToSectors((int i, int j)[] sects, AtomState atom)
        {
            _debugger.CreateGhostAtom(atom);
            _debugger.CreateVector(atom);
            await _debugger.Stop();

            for (int i = 0; i < sects.Length; i++)
            {
                bool foundSector = false;

                foreach (var sector in _sectors.Sectors)
                {
                    if (sects[i].i == sector.I && sects[i].j == sector.J)
                    {
                        sector.GhostAtoms.Add(atom);
                        foundSector = true;

                        break;
                    }
                }

                if (!foundSector)
                {
                    var newAllStates = new AllStates(new List<AtomState>(), new List<RectState>());

                    var ghostAtoms = new List<AtomState>();
                    ghostAtoms.Add(atom);

                    var newSector = new SectorState(newAllStates, ghostAtoms, sects[i].i, sects[i].j);

                    _debugger.CreateSector(newSector);
                    await _debugger.Stop();

                    _sectors.Sectors.Add(newSector);
                }
            }
        }

        private async ValueTask AddToSectors((int i, int j)[] sects, RectState rect)
        {
            _debugger.CreateRect(rect);
            await _debugger.Stop();

            for (int i = 0; i < sects.Length; i++)
            {
                bool foundSector = false;

                foreach (var sector in _sectors.Sectors)
                {
                    if (sects[i].i == sector.I && sects[i].j == sector.J)
                    {
                        sector.AllStates.Rects.Add(rect);
                        foundSector = true;

                        break;
                    }
                }

                if (!foundSector)
                {
                    var newAllStates = new AllStates(new List<AtomState>(), new List<RectState>());
                    newAllStates.Rects.Add(rect);

                    var newSector = new SectorState(newAllStates, new List<AtomState>(), sects[i].i, sects[i].j);

                    _debugger.CreateSector(newSector);
                    await _debugger.Stop();

                    _sectors.Sectors.Add(newSector);
                }
            }
        }
    }
}
