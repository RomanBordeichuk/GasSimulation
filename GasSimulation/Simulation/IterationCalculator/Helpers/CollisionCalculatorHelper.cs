using GasSimulation.Simulation.DTOs;

namespace GasSimulation.Simulation.IterationCalculator.Helpers
{
    public static class CollisionCalculatorHelper
    {
        public static (double? t, double? angle) CalculateAtomTAndAngle(
            AtomState atomState1, AtomState atomState2, double remT, double r1, double r2)
        {
            double nm1 = atomState1.Velocity.Dx - atomState2.Velocity.Dx;
            double nm2 = atomState1.Velocity.Dy - atomState2.Velocity.Dy;

            double a12 = atomState1.X - atomState2.X;
            double b12 = atomState1.Y - atomState2.Y;

            double a = nm1 * nm1 + nm2 * nm2;
            double b = 2 * (nm1 * a12 + nm2 * b12);
            double c = a12 * a12 + b12 * b12 - (r1 + r2) * (r1 + r2);

            double D = b * b - 4 * a * c;

            if (D >= 0)
            {
                double t = (-b - Math.Pow(D, 0.5)) / (2 * a);

                if (t > -Constants.ErrorRate && t < remT)
                {
                    double x1 = atomState1.X + atomState1.Dx * t;
                    double y1 = atomState1.Y + atomState1.Dy * t;

                    double x2 = atomState2.X + atomState2.Dx * t;
                    double y2 = atomState2.Y + atomState2.Dy * t;

                    double angle = MathHelper.CalculateCollisionAngle(new(x1, y1), new(x2, y2));

                    return (t, angle);
                }
            }

            return (null, null);
        }

        public static (double? t, double? angle) CalculateRectTAndAngle(
            AtomState a, DimentState dimentions, double remT)
        {
            double? lowestT = null;
            double? lowestAngle = null;

            CompareTWithAngle(CalculateClosestRectEdgeTAndAngle(a, dimentions), ref lowestT, ref lowestAngle);

            CompareTWithAngle(CalculateAtomTAndAngle(a, new(new(dimentions.Width / 2, dimentions.Height / 2),
                new(0, 0)), remT, Constants.AtomDiameter / 2, Constants.ErrorRate), ref lowestT, ref lowestAngle);

            CompareTWithAngle(CalculateAtomTAndAngle(a, new(new(-dimentions.Width / 2, dimentions.Height / 2),
                new(0, 0)), remT, Constants.AtomDiameter / 2, Constants.ErrorRate), ref lowestT, ref lowestAngle);

            CompareTWithAngle(CalculateAtomTAndAngle(a, new(new(dimentions.Width / 2, -dimentions.Height / 2),
                new(0, 0)), remT, Constants.AtomDiameter / 2, Constants.ErrorRate), ref lowestT, ref lowestAngle);

            CompareTWithAngle(CalculateAtomTAndAngle(a, new(new(-dimentions.Width / 2, -dimentions.Height / 2),
                new(0, 0)), remT, Constants.AtomDiameter / 2, Constants.ErrorRate), ref lowestT, ref lowestAngle);

            if (lowestT <= -Constants.ErrorRate || lowestT >= remT) return (null, null);

            return (lowestT, lowestAngle);
        }

        private static (double? lowestT, double? lowestAngle) CalculateClosestRectEdgeTAndAngle(
            AtomState a, DimentState dimentions)
        {
            double? lowestT = null;
            double? lowestAngle = null;

            if (MathHelper.Equals(a.Dx, 0) && MathHelper.Equals(a.Dy, 0)) return (null, null);

            if (CalculateTAndAngleForEdgesWith0dx(a, dimentions, ref lowestT, ref lowestAngle))
                return (lowestT, lowestAngle);

            if (CalculateTAndAngleForEdgesWith0dy(a, dimentions, ref lowestT, ref lowestAngle))
                return (lowestT, lowestAngle);

            CalculateTAndAngleForEdges(a, dimentions, ref lowestT, ref lowestAngle);

            if (lowestAngle is null) return (null, null);

            if (DoesAtomMovesFromEdge(lowestAngle.Value, a.Velocity)) return (null, null);

            return (lowestT, lowestAngle);
        }

        private static bool CalculateTAndAngleForEdgesWith0dx(
            AtomState a, DimentState dimentions,
            ref double? lowestT, ref double? lowestAngle)
        {
            if (MathHelper.Equals(a.Dx, 0))
            {
                if (Math.Abs(a.X) >= dimentions.Width / 2)
                {
                    lowestT = null;
                    lowestAngle = null;

                    return true;
                }

                double tymin = (-dimentions.Height / 2 - Constants.AtomDiameter / 2 - a.Y) / a.Dy;
                double tymax = (dimentions.Height / 2 + Constants.AtomDiameter / 2 - a.Y) / a.Dy;

                if (a.Dy > 0) CompareTWithAngle(tymin, Math.PI / 2, ref lowestT, ref lowestAngle);
                else CompareTWithAngle(tymax, -Math.PI / 2, ref lowestT, ref lowestAngle);

                return true;
            }

            return false;
        }

        private static bool CalculateTAndAngleForEdgesWith0dy(
            AtomState a, DimentState dimentions,
            ref double? lowestT, ref double? lowestAngle)
        {
            if (MathHelper.Equals(a.Dy, 0))
            {
                if (Math.Abs(a.Y) >= dimentions.Height / 2)
                {
                    lowestT = null;
                    lowestAngle = null;

                    return true;
                }

                double txmin = (-dimentions.Width / 2 - Constants.AtomDiameter / 2 - a.X) / a.Dx;
                double txmax = (dimentions.Width / 2 + Constants.AtomDiameter / 2 - a.X) / a.Dx;

                if (a.Dx > 0) CompareTWithAngle(txmin, 0, ref lowestT, ref lowestAngle);
                else CompareTWithAngle(txmax, Math.PI, ref lowestT, ref lowestAngle);

                return true;
            }

            return false;
        }

        private static void CalculateTAndAngleForEdges(
            AtomState a, DimentState dimentions,
            ref double? lowestT, ref double? lowestAngle)
        {
            double txmin = (-dimentions.Width / 2 - a.X) / a.Dx;
            double txmax = (dimentions.Width / 2 - a.X) / a.Dx;

            double tymin = (-dimentions.Height / 2 - a.Y) / a.Dy;
            double tymax = (dimentions.Height / 2 - a.Y) / a.Dy;

            double t1x = (dimentions.Width / 2 + Constants.AtomDiameter / 2 - a.X) / a.Dx;
            double t2x = (-dimentions.Width / 2 - Constants.AtomDiameter / 2 - a.X) / a.Dx;

            double t1y = (dimentions.Height / 2 + Constants.AtomDiameter / 2 - a.Y) / a.Dy;
            double t2y = (-dimentions.Height / 2 - Constants.AtomDiameter / 2 - a.Y) / a.Dy;

            CalculateTAndAngleForHorizontalEdges(a.Dx,
                txmin, txmax, t1y, t2y,
                ref lowestT, ref lowestAngle);

            CalculateTAndAngleForVerticalEdges(a.Dy,
                tymin, tymax, t1x, t2x,
                ref lowestT, ref lowestAngle);
        }

        private static void CalculateTAndAngleForHorizontalEdges(double dx,
            double txmin, double txmax, double t1y, double t2y,
            ref double? lowestT, ref double? lowestAngle)
        {
            if (dx > 0)
            {
                if (t1y >= txmin && t1y <= txmax)
                    CompareTWithAngle(t1y, -Math.PI / 2, ref lowestT, ref lowestAngle);

                if (t2y >= txmin && t2y <= txmax)
                    CompareTWithAngle(t2y, Math.PI / 2, ref lowestT, ref lowestAngle);
            }
            else
            {
                if (t1y >= txmax && t1y <= txmin)
                    CompareTWithAngle(t1y, -Math.PI / 2, ref lowestT, ref lowestAngle);

                if (t2y >= txmax && t2y <= txmin)
                    CompareTWithAngle(t2y, Math.PI / 2, ref lowestT, ref lowestAngle);
            }
        }

        private static void CalculateTAndAngleForVerticalEdges(double dy,
            double tymin, double tymax, double t1x, double t2x,
            ref double? lowestT, ref double? lowestAngle)
        {
            if (dy > 0)
            {
                if (t1x >= tymin && t1x <= tymax)
                    CompareTWithAngle(t1x, Math.PI, ref lowestT, ref lowestAngle);

                if (t2x >= tymin && t2x <= tymax)
                    CompareTWithAngle(t2x, 0, ref lowestT, ref lowestAngle);
            }
            else
            {
                if (t1x >= tymax && t1x <= tymin)
                    CompareTWithAngle(t1x, Math.PI, ref lowestT, ref lowestAngle);

                if (t2x >= tymax && t2x <= tymin)
                    CompareTWithAngle(t2x, 0, ref lowestT, ref lowestAngle);
            }
        }

        private static bool DoesAtomMovesFromEdge(double lowestAngle, VelocityState v)
        {
            return (MathHelper.Equals(lowestAngle, 0) && v.Dx <= 0) ||
                (MathHelper.Equals(lowestAngle, Math.PI / 2) && v.Dy <= 0) ||
                (MathHelper.Equals(lowestAngle, -Math.PI / 2) && v.Dy >= 0) ||
                (MathHelper.Equals(lowestAngle, Math.PI) && v.Dx >= 0);
        }

        private static void CompareTWithAngle(
            double? t, double? angle, ref double? lowestT, ref double? lowestAngle)
        {
            CompareTWithAngle((t, angle), ref lowestT, ref lowestAngle);
        }

        private static void CompareTWithAngle(
            (double? t, double? angle) value, ref double? lowestT, ref double? lowestAngle)
        {
            if (value.t is null) return;

            if (lowestT == null || value.t < lowestT)
            {
                lowestT = value.t;
                lowestAngle = value.angle;
            }
        }
    }
}
