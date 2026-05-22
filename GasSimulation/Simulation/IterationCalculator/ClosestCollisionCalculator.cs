using GasSimulation.Configuration;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.IterationCalculator
{
    public class ClosestCollisionCalculator
    {
        private readonly Config _config;
        private readonly SectorPartitioner _partitioner;
        private readonly CollisionCalculator _collisionCalculator;

        public ClosestCollisionCalculator(Config config, SectorPartitioner partitioner,
            CollisionCalculator collisionCalculator)
        {
            _config = config;
            _partitioner = partitioner;
            _collisionCalculator = collisionCalculator;
        }

        public (CollisionState<AtomState, AtomState>, CollisionState<AtomState, RectState>)
            Calculate(AllStates allStates, double remT)
        {
            var sectors = _partitioner.Partition(allStates);
            var sectsArr = sectors.Sects.Values.ToArray();

            var atomCollision = new CollisionState<AtomState, AtomState>(new(), new(), -1, 0);
            var rectCollision = new CollisionState<AtomState, RectState>(new(), new(), -1, 0);

            var collisionStatesArr = new (double lowestT, 
                CollisionState<AtomState, AtomState> atomCollision,
                CollisionState<AtomState, RectState> rectCollision)[sectsArr.Length];

            Parallel.For(0, sectsArr.Length, (index) =>
            {
                var sect = sectsArr[index];

                double localLowestT = 1;
                var localAtomCollision = new CollisionState<AtomState, AtomState>(new(), new(), -1, 0);
                var localRectsCollision = new CollisionState<AtomState, RectState>(new(), new(), -1, 0);

                for (int i = 0; i < sect.AtomIds.Count; i++)
                {
                    for (int j = i + 1; j < sect.AtomIds.Count; j++)
                    {
                        if (!AreElemsClose(allStates.Atoms[sect.AtomIds[i]],
                            allStates.Atoms[sect.AtomIds[j]], localLowestT)) continue;

                        CheckCollisionAtom(sect.AtomIds[i], sect.AtomIds[j],
                            allStates.Atoms[sect.AtomIds[i]], allStates.Atoms[sect.AtomIds[j]],
                            remT, ref localLowestT, ref localAtomCollision);
                    }
                }

                for (int i = 0; i < sect.AtomIds.Count; i++)
                {
                    for (int j = 0; j < sect.RectIds.Count; j++)
                    {
                        CheckCollisionRect(sect.AtomIds[i], sect.RectIds[j],
                            allStates.Atoms[sect.AtomIds[i]], allStates.Rects[sect.RectIds[j]],
                            remT, ref localLowestT, ref localRectsCollision);
                    }
                }

                collisionStatesArr[index] = (localLowestT, localAtomCollision, localRectsCollision);
            });

            double lowestT = 1;

            foreach (var collisionStates in collisionStatesArr)
            {
                if (collisionStates.lowestT < lowestT)
                {
                    lowestT = collisionStates.lowestT;
                    atomCollision = collisionStates.atomCollision;
                    rectCollision = collisionStates.rectCollision;
                }
            }

            return (atomCollision, rectCollision);
        }

        private void CheckCollisionAtom(int id1, int id2, 
            AtomState atom1, AtomState atom2, double remT, ref double localLowestT,
            ref CollisionState<AtomState, AtomState> collision)
        {
            (double t, double angle) = _collisionCalculator.CalculateAtomTAndAngle(
                atom1, atom2, remT);

            if (MathHelper.Equals(t, -1, _config.Simulation.ErrorRate)) return;

            if (t < localLowestT)
            {
                localLowestT = t;
                collision = new(id1, id2, atom1, atom2, t, angle);
            }
        }

        private void CheckCollisionRect(int id1, int id2, 
            AtomState atom1, RectState rect2, double remT, ref double localLowestT,
            ref CollisionState<AtomState, RectState> collision)
        {
            (double t, double angle) = _collisionCalculator.CalculateRectTAndAngle(
                atom1, rect2, remT);

            if (MathHelper.Equals(t, -1, _config.Simulation.ErrorRate)) return;

            if (t < localLowestT)
            {
                localLowestT = t;
                collision = new(id1, id2, atom1, rect2, t, angle);
            }
        }

        private bool AreElemsClose(AtomState atom1, AtomState atom2, double localLowestT)
        {
            bool xClose;
            bool yClose;

            xClose = Math.Abs(atom1.X - atom2.X) <= Math.Abs(atom1.Dx - atom2.Dx) *
                localLowestT + _config.Simulation.AtomDiameter;

            yClose = Math.Abs(atom1.Y - atom2.Y) <= Math.Abs(atom1.Dy - atom2.Dy) *
                localLowestT + _config.Simulation.AtomDiameter;

            return xClose && yClose;
        }
    }
}
