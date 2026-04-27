using GasSimulation.Logs;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.IterationCalculator
{
    public static class IterationCalculator
    {
        public static void Calculate(Config config, AllStates allStates)
        {
            double remT = 1;

            while (true)
            {
                CollisionState<AtomState, AtomState> atomCollision = 
                    ClosestAtomCollisionHelper.Calculate(config, ref allStates, remT);

                CollisionState<AtomState, RectState> rectCollision = 
                    ClosestRectCollisionHelper.Calculate(config, ref allStates, remT);

                if (MathHelper.Equals(atomCollision.T, -1, config.ErrorRate) && 
                    MathHelper.Equals(rectCollision.T, -1, config.ErrorRate))
                {
                    MoveAll(allStates.Atoms, remT);
                    break;
                }

                Logger.Log("Collision!!!");

                if (MathHelper.Equals(rectCollision.T, -1, config.ErrorRate) || 
                    (!MathHelper.Equals(atomCollision.T, -1, config.ErrorRate) && 
                    atomCollision.T < rectCollision.T))
                {
                    MoveAll(allStates.Atoms, atomCollision.T);

                    (VelocityState v1, VelocityState v2) = CollisionCalculator.CalculateVelocities(
                        config, atomCollision.Obj1, atomCollision.Obj2, atomCollision.Angle);

                    allStates.Atoms[atomCollision.Id1] = new AtomState(atomCollision.Obj1.Pos, v1);
                    allStates.Atoms[atomCollision.Id2] = new AtomState(atomCollision.Obj2.Pos, v2);

                    remT -= atomCollision.T;
                }
                else
                {
                    MoveAll(allStates.Atoms, rectCollision.T);

                    VelocityState v = CollisionCalculator.CalculateVelocity(
                        rectCollision.Obj1, rectCollision.Angle);

                    allStates.Atoms[rectCollision.Id1] = new AtomState(rectCollision.Obj1.Pos, v);

                    remT -= rectCollision.T;
                }
            }
        }

        private static void MoveAll(AtomState[] atoms, double t)
        {
            for (int i = 0; i < atoms.Length; i++)
            {
                var atom = atoms[i];

                PosState pos = atom.Pos;
                VelocityState v = atom.Velocity;

                double x = pos.X;
                double y = pos.Y;

                x += v.Dx * t;
                y += v.Dy * t;

                atoms[i] = new AtomState(new(x, y), atom.Velocity);
            }
        }
    }
}
