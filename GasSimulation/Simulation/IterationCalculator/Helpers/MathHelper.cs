using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Atom;

namespace GasSimulation.Simulation.IterationCalculator.Helpers
{
    public static class MathHelper
    {
        public static bool Equals(double a, double b, double errorRate)
        {
            return Math.Abs(a - b) < errorRate;
        }

        public static double CalculateCollisionAngle(PosState pos1, PosState pos2)
        {
            double dx = pos1.X - pos2.X;
            double dy = pos1.Y - pos2.Y;

            if (Equals(dx, 0)) return Math.PI / 2;

            return Math.Atan(dy / dx);
        }

        public static VelocityState TransformToNewBasis(VelocityState v, double angle)
        {
            double newDx = v.Dx * Math.Cos(angle) + v.Dy * Math.Sin(angle);
            double newDy = -v.Dx * Math.Sin(angle) + v.Dy * Math.Cos(angle);

            return new(newDx, newDy);
        }

        public static PosState TransformToNewBasis(PosState pos, double angle)
        {
            VelocityState v = TransformToNewBasis(new VelocityState(pos.X, pos.Y), angle);

            return new(v.Dx, v.Dy);
        }

        public static AtomState RotateField(AtomState a, double angle)
        {
            AtomState newA = new(TransformToNewBasis(a.Pos, -angle),
                TransformToNewBasis(a.Velocity, -angle));

            return newA;
        }

        public static PosState RotateField(PosState pos, double angle)
        {
            return TransformToNewBasis(pos, -angle);
        }

        public static PosState TranslateField(PosState pos1, PosState pos2)
        {
            return new(pos1.X - pos2.X, pos1.Y - pos2.Y);
        }

        public static (double dx1, double dx2) RecalculateMomentum(Config config, double dx1, double dx2)
        {
            double newDx1 = (dx1 + dx2 - config.Restitution * (dx1 - dx2)) / 2;
            double newDx2 = (dx1 + dx2 + config.Restitution * (dx1 - dx2)) / 2;

            return (newDx1, newDx2);
        }

        public static (double dx, double dy) DecomposeVelocity(double speed, double angle)
        {
            double dx = speed * Math.Cos(angle * Math.PI / 180);
            double dy = speed * Math.Sin(angle * Math.PI / 180);

            return (dx, dy);
        }

        public static double CalculateDistance(PosState pos1, PosState pos2)
        {
            return Math.Pow((pos1.X - pos2.X) * (pos1.X - pos2.X) + (pos1.Y - pos2.Y) * (pos1.Y - pos2.Y), 0.5);
        }

        public static double TransformAngleToRAD(double angle)
        {
            return angle * Math.PI / 180;
        }

        public static double TransformAngleToDEG(double angle)
        {
            return angle * 180 / Math.PI;
        }
    }
}
