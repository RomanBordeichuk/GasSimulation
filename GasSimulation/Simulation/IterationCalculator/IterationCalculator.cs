using GasSimulation.Configuration;
using GasSimulation.Debuggers;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Logs;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.IterationCalculator
{
    public class IterationCalculator
    {
        private readonly Config _config;
        private readonly ClosestCollisionCalculator _closestCollisionCalculator;
        private readonly IterationCalculatorVisualDebugger _debugger;

        private AllStates _allStates = null!;
        private double _remT;

        public IterationCalculator(Config config, 
            ClosestCollisionCalculator closestCollisionCalculator,
            IterationCalculatorVisualDebugger debugger)
        {
            _config = config;
            _closestCollisionCalculator = closestCollisionCalculator;
            _debugger = debugger;
        }

        public void Calculate(AllStates allStates)
        {
            _allStates = allStates;
            _remT = 1;

            while (true)
            {
                (var atomCollision, var rectCollision) = 
                    _closestCollisionCalculator.Calculate(_allStates, _remT);

                int lowerSide = GetLowerSide(atomCollision.T, rectCollision.T);

                if (lowerSide == 0)
                {
                    MoveAll(_remT);

                    break;
                }

                Logger.Log("Collision!!!");

                if (lowerSide == -1) MoveToClosestAtomCollision(atomCollision);
                else MoveToClosestRectCollision(rectCollision);
            }
        }

        private int GetLowerSide(double t1, double t2)
        {
            if (MathHelper.Equals(t1, -1, _config.Simulation.ErrorRate))
            {
                if (MathHelper.Equals(t2, -1, _config.Simulation.ErrorRate)) return 0;
                return 1;
            }
            else
            {
                if (MathHelper.Equals(t2, -1, _config.Simulation.ErrorRate)) return -1;
                else if (t1 < t2) return -1;
                return 1;
            }
        }

        private void MoveToClosestAtomCollision(CollisionState<AtomState, AtomState> collision)
        {
            MoveAll(collision.T);

            (VelocityState newV1, VelocityState newV2) = CollisionCalculator.CalculateVelocities(
                _config, collision.Obj1, collision.Obj2, collision.Angle);

            var newPos1 = Move(collision.Obj1, collision.T);
            var newPos2 = Move(collision.Obj2, collision.T);

            _allStates.Atoms[collision.Id1] = new AtomState(newPos1, newV1);
            _allStates.Atoms[collision.Id2] = new AtomState(newPos2, newV2);

            _remT -= collision.T;
        }

        private void MoveToClosestRectCollision(CollisionState<AtomState, RectState> collision)
        {
            MoveAll(collision.T);

            VelocityState newV = CollisionCalculator.CalculateVelocity(
                collision.Obj1, collision.Angle);
            var newPos = Move(collision.Obj1, collision.T);

            _allStates.Atoms[collision.Id1] = new AtomState(newPos, newV);

            _remT -= collision.T;
        }

        private void MoveAll(double t)
        {
            for (int i = 0; i < _allStates.Atoms.Length; i++)
            {
                _allStates.Atoms[i] = new(Move(_allStates.Atoms[i], t), _allStates.Atoms[i].Velocity);
            }
        }

        private static PosState Move(AtomState atom, double t)
        {
            PosState pos = atom.Pos;
            VelocityState v = atom.Velocity;

            double x = pos.X;
            double y = pos.Y;

            x += v.Dx * t;
            y += v.Dy * t;

            return new(x, y);
        }
    }
}
