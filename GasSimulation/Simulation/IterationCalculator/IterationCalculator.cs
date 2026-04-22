using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.DTOs.Interfaces;
using GasSimulation.Simulation.Exceptions;
using GasSimulation.Simulation.Loggers;

namespace GasSimulation.Simulation.IterationCalculator
{
    public static class IterationCalculator
    {
        public static void Calculate(List<IElemState> elementsList)
        {
            double remT = 1;

            while (true)
            {
                CollisionState<AtomState, IElemState>? collisionState = CalculateClosestCollision(elementsList, remT);

                if (collisionState is null)
                {
                    MoveAll(elementsList, remT);
                    break;
                }

                Logger.Log("Collision!!!");

                MoveAll(elementsList, collisionState.Value.T);

                if (collisionState.Value.Obj2 is AtomState atom2)
                {
                    (VelocityState v1, VelocityState v2) = CollisionCalculator.CalculateVelocities(
                        collisionState.Value.Obj1, atom2, collisionState.Value.Angle);

                    elementsList[collisionState.Value.Id1] = new AtomState(collisionState.Value.Obj1.Pos, v1);
                    elementsList[collisionState.Value.Id2] = new AtomState(atom2.Pos, v2);
                }
                else if (collisionState.Value.Obj2 is RectState)
                {
                    VelocityState v = CollisionCalculator.CalculateVelocity(
                        collisionState.Value.Obj1, collisionState.Value.Angle);

                    elementsList[collisionState.Value.Id1] = new AtomState(collisionState.Value.Obj1.Pos, v);
                }

                remT -= collisionState.Value.T;
            }
        }

        private static CollisionState<AtomState, IElemState>?
            CalculateClosestCollision(List<IElemState> elementsList, double remT)
        {
            CollisionState<AtomState, IElemState>? collistionState = null;

            for (int i = 0; i < elementsList.Count; i++)
            {
                var elem1 = elementsList[i];

                if (elem1 is RectState) continue;
                if (elem1 is AtomState atom)
                {
                    for (int j = i + 1; j < elementsList.Count; j++)
                    {
                        var elem2 = elementsList[j];

                        CheckCollision(i, j, atom, elem2, remT, ref collistionState);
                    }
                }
                else throw new IncorrectTypeException();
            }

            if (collistionState is null)
            {
                return null;
            }

            return collistionState;
        }

        private static void CheckCollision(
            int id1, int id2, AtomState atomState1, IElemState elem2, double remT,
            ref CollisionState<AtomState, IElemState>? collistionState)
        {
            if (!AreElemsClose(atomState1, elem2)) return;

            (double? t, double? angle) = CalculateNewTAndAngle(atomState1, elem2, remT);

            if (t == null) return;

            if (collistionState == null || t < collistionState.Value.T)
                collistionState = new(id1, id2, atomState1, elem2, t.Value, angle!.Value);
        }

        private static bool AreElemsClose(AtomState atomState, IElemState elem2)
        {
            if (elem2 is RectState) return true;
            else if (elem2 is AtomState atom2)
            {
                return Math.Abs(atomState.X - atom2.Pos.X) <= Math.Abs(atomState.Dx - atom2.Velocity.Dx) + Constants.AtomDiameter
                    && Math.Abs(atomState.Y - atom2.Pos.Y) <= Math.Abs(atomState.Dy - atom2.Velocity.Dy) + Constants.AtomDiameter;
            }
            else throw new IncorrectTypeException();
        }

        private static (double? t, double? angle) CalculateNewTAndAngle(AtomState atomState1, IElemState elemState2, double remT)
        {
            if (elemState2 is RectState rect)
            {
                return CollisionCalculator.CalculateRectTAndAngle(atomState1, rect, remT);
            }
            else if (elemState2 is AtomState atom)
            {
                return CollisionCalculator.CalculateAtomTAndAngle(atomState1, atom, remT);
            }
            else throw new IncorrectTypeException();
        }

        private static void MoveAll(List<IElemState> elementsList, double t)
        {
            for (int i = 0; i < elementsList.Count; i++)
            {
                var element = elementsList[i];

                if (element is RectState) continue;
                if (element is AtomState atom)
                {
                    PosState pos = atom.Pos;
                    VelocityState v = atom.Velocity;

                    double x = pos.X;
                    double y = pos.Y;

                    x += v.Dx * t;
                    y += v.Dy * t;

                    elementsList[i] = new AtomState(new(x, y), atom.Velocity);
                }
                else throw new IncorrectTypeException();
            }
        }
    }
}
