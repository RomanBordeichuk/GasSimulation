using GasSimulation.Simulation.DTOs;

namespace GasSimulation.Simulation.IterationCalculator.Helpers
{
    public static class ClosestAtomCollisionHelper
    {
        public static CollisionState<AtomState, AtomState>
            Calculate(Config config, ref AllStates allStates, double remT)
        {
            CollisionState<AtomState, AtomState> collistion = new(new(), new(), -1, 0);

            for (int i = 0; i < allStates.Atoms.Length; i++)
            {
                var atom1 = allStates.Atoms[i];

                for (int j = i + 1; j < allStates.Atoms.Length; j++)
                {
                    var atom2 = allStates.Atoms[j];

                    CheckCollisionAtom(config, i, j, atom1, atom2, remT, ref collistion);
                }
            }

            return collistion;
        }

        private static void CheckCollisionAtom(Config config,
            int id1, int id2, AtomState atom1, AtomState atom2, double remT,
            ref CollisionState<AtomState, AtomState> collision)
        {
            if (!AreElemsClose(config, atom1, atom2)) return;

            (double? t, double? angle) = CalculateNewTAndAngleAtom(config, atom1, atom2, remT);

            if (t == null) return;

            if (MathHelper.Equals(collision.T, -1, config.ErrorRate) || t < collision.T)
                collision = new(id1, id2, atom1, atom2, t.Value, angle!.Value);
        }

        private static bool AreElemsClose(Config config, AtomState atom1, AtomState atom2)
        {
            return Math.Abs(atom1.X - atom2.Pos.X) <= Math.Abs(atom1.Dx - atom2.Velocity.Dx) + config.AtomDiameter
                && Math.Abs(atom1.Y - atom2.Pos.Y) <= Math.Abs(atom1.Dy - atom2.Velocity.Dy) + config.AtomDiameter;
        }

        private static (double? t, double? angle) CalculateNewTAndAngleAtom(Config config,
            AtomState atom1, AtomState atom2, double remT)
        {
            return CollisionCalculator.CalculateAtomTAndAngle(config, atom1, atom2, remT);
        }
    }
}
