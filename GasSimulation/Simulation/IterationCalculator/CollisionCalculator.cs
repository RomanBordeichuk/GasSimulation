using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.IterationCalculator
{
    public static class CollisionCalculator
    {
        public static (double? t, double? angle) CalculateAtomTAndAngle(Config config,
            AtomState atomState1, AtomState atomState2, double remT)
        {
            return CalculateAtomTAndAngle(config, atomState1, atomState2, remT,
                config.AtomDiameter / 2, config.AtomDiameter / 2);
        }

        public static (double? t, double? angle) CalculateAtomTAndAngle(Config config,
            AtomState atomState1, AtomState atomState2, double remT, double r1, double r2)
        {
            return CollisionCalculatorHelper.CalculateAtomTAndAngle(
                config, atomState1, atomState2, remT, r1, r2);
        }

        public static (double? t, double? angle) CalculateRectTAndAngle(Config config,
            AtomState atomState, RectState rectState, double remT)
        {
            PosState updatedPos = MathHelper.TranslateField(atomState.Pos, rectState.Pos);

            atomState = new(updatedPos, atomState.Velocity);
            atomState = MathHelper.RotateField(atomState, -rectState.Angle);

            (double? t, double? angle) = CollisionCalculatorHelper.CalculateRectTAndAngle(
                config, atomState, rectState.Dimentions, remT);



            if (angle != null) angle += rectState.Angle;

            return (t, angle);
        }

        public static (VelocityState v1, VelocityState v2) CalculateVelocities(
            Config config, AtomState atomState1, AtomState atomState2, double angle)
        {
            VelocityState v1 = atomState1.Velocity;
            VelocityState v2 = atomState2.Velocity;

            v1 = MathHelper.TransformToNewBasis(v1, angle);
            v2 = MathHelper.TransformToNewBasis(v2, angle);

            (double dx1, double dx2) = MathHelper.RecalculateMomentum(config, v1.Dx, v2.Dx);

            v1 = new(dx1, v1.Dy);
            v2 = new(dx2, v2.Dy);

            v1 = MathHelper.TransformToNewBasis(v1, -angle);
            v2 = MathHelper.TransformToNewBasis(v2, -angle);

            return (v1, v2);
        }

        public static VelocityState CalculateVelocity(
            AtomState atom1, double angle)
        {
            VelocityState v1 = atom1.Velocity;

            v1 = MathHelper.TransformToNewBasis(v1, angle);

            v1 = new(-v1.Dx, v1.Dy);

            v1 = MathHelper.TransformToNewBasis(v1, -angle);

            return v1;
        }
    }
}
