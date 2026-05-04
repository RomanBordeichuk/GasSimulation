using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.DTOs;

namespace GasSimulation.Simulation.IterationCalculator.Helpers
{
    public static class ClosestRectCollisionHelper
    {
        public static CollisionState<AtomState, RectState>
            Calculate(Config config, ref AllStates allStates, double remT)
        {
            CollisionState<AtomState, RectState> collistion = new(new(), new(), -1, 0);

            for (int i = 0; i < allStates.Atoms.Length; i++)
            {
                var atom1 = allStates.Atoms[i];

                for (int j = 0; j < allStates.Rects.Length; j++)
                {
                    var rect2 = allStates.Rects[j];

                    CheckCollisionRect(config, i, j, atom1, rect2, remT, ref collistion);
                }
            }

            return collistion;
        }

        private static void CheckCollisionRect(Config config,
            int id1, int id2, AtomState atom1, RectState rect2, double remT,
            ref CollisionState<AtomState, RectState> collision)
        {
            (double? t, double? angle) = CalculateNewTAndAngleRect(config, atom1, rect2, remT);

            if (t == null) return;

            if (MathHelper.Equals(collision.T, -1, config.ErrorRate) || t < collision.T)
                collision = new(id1, id2, atom1, rect2, t.Value, angle!.Value);
        }



        private static (double? t, double? angle) CalculateNewTAndAngleRect(
            Config config, AtomState atom1, RectState rect2, double remT)
        {
            return CollisionCalculator.CalculateRectTAndAngle(config, atom1, rect2, remT);
        }
    }
}
