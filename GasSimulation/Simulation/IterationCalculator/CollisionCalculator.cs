using GasSimulation.Configuration;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.IterationCalculator
{
    public class CollisionCalculator
    {
        private readonly Config _config;
        private readonly CollisionCalculatorHelper _collisionCalculatorHelper;

        public CollisionCalculator(Config config, 
            CollisionCalculatorHelper collisionCalculatorHelper)
        {
            _config = config;
            _collisionCalculatorHelper = collisionCalculatorHelper;
        }

        public (double t, double angle) CalculateAtomTAndAngle(
            AtomState atomState1, AtomState atomState2, double remT)
        {
            return CalculateAtomTAndAngle(atomState1, atomState2, remT,
                _config.Simulation.AtomDiameter / 2, _config.Simulation.AtomDiameter / 2);
        }

        public (double t, double angle) CalculateAtomTAndAngle(
            AtomState atomState1, AtomState atomState2, double remT, double r1, double r2)
        {
            return _collisionCalculatorHelper.CalculateAtomTAndAngle(
                atomState1, atomState2, remT, r1, r2);
        }

        public (double t, double angle) CalculateRectTAndAngle(
            AtomState atomState, RectState rectState, double remT)
        {
            PosState updatedPos = MathHelper.TranslateField(atomState.Pos, rectState.Pos);

            atomState = new(updatedPos, atomState.Velocity);
            atomState = MathHelper.RotateField(atomState, -rectState.Angle);

            (double t, double angle) = _collisionCalculatorHelper.CalculateRectTAndAngle(
                atomState, rectState.Dimentions, remT);

            if (!MathHelper.Equals(t, -1, _config.Simulation.ErrorRate)) angle += rectState.Angle;

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
