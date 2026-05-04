using GasSimulation.GeneralDTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Tests.Unit.IterationCalculator.Helpers
{
    public class MathHelperTests : TestsBase
    {
        [Theory]
        [InlineData(0.0, 0.0, true)]
        [InlineData(1.0, 1.435546, false)]
        public void Equals_StandartCse_ExpectedResult(
            double a, double b, bool exp)
        {
            //Act

            bool res = MathHelper.Equals(a, b, _config.ErrorRate);

            //Assert

            Assert.Equal(exp, res);
        }

        [Theory]
        [InlineData(
            -76.95856911994554, 12.10043552322992, -75.0, 7.5,
            -1.1683027951936686)]
        [InlineData(
            -79.7117763139859, -9.173070222065258, -75.0, -7.5,
            0.3411956354527345)]
        [InlineData(
            300.0, 247.3684210526316, 300.0, 257.36842105263156,
            -1.5707963267948966)]
        public void CalculateCollisionAngle_StandartCase_ExpectedResult(
            double x1, double y1, double x2, double y2,
            double expAngle)
        {
            //Arrange

            PosState pos1 = new(x1, y1);
            PosState pos2 = new(x2, y2);

            //Act

            double angle = MathHelper.CalculateCollisionAngle(pos1, pos2);

            //Assert

            Assert.Equal(expAngle, angle, _config.Precision);
        }

        [Theory]
        [InlineData(
            3.061616997868383E-16, 5.0, 1.5707963267948966,
            5.0, 0.0)]
        [InlineData(
            -39, 48.69999999999999, 0.5235987755982988,
            -9.424990747593117, 61.67543716430215)]
        public void TransformToNewBasis_StandartCase_ExpectedResult(
            double dx, double dy, double angle,
            double expDx, double expDy)
        {
            //Arrange

            VelocityState v = new(dx, dy);

            //Act

            VelocityState newV = MathHelper.TransformToNewBasis(v, angle);

            //Assert

            Assert.Equal(expDx, newV.Dx, _config.Precision);
            Assert.Equal(expDy, newV.Dy, _config.Precision);
        }

        [Theory]
        [InlineData(
            425.89745962155615, 151.650635094611, 500.0, 250.0,
            -74.10254037844385, -98.349364905389)]
        [InlineData(
            380.3589838486225, 152.2, 500.0, 200.0,
            -119.64101615137753, -47.80000000000001)]
        public void TranslateField_StandartCase_ExpectedResult(
            double x1, double y1, double x2, double y2,
            double expX, double expY)
        {
            //Arrange

            PosState pos1 = new(x1, y1);
            PosState pos2 = new(x2, y2);

            //Act

            PosState newPos = MathHelper.TranslateField(pos1, pos2);

            //Assert

            Assert.Equal(expX, newPos.X, _config.Precision);
            Assert.Equal(expY, newPos.Y, _config.Precision);
        }

        [Theory]
        [InlineData(
            4.582575694955842, -4.124318125460258,
            -4.124318125460258, 4.582575694955841)]
        [InlineData(
            4.720419829680665, -4.411183668492287,
            -4.411183668492287, 4.720419829680665)]
        public void RecalculateMomentum_StandartCase_ExpectedResult(
            double dx1, double dx2, 
            double expDx1, double expDx2)
        {
            //Act

            (double newDx1, double newDx2) = MathHelper.RecalculateMomentum(
                _config, dx1, dx2);

            //Arrange

            Assert.Equal(expDx1, newDx1, _config.Precision);
            Assert.Equal(expDx2, newDx2, _config.Precision);
        }

        [Theory]
        [InlineData(
            3.5, 0.0,
            3.5, 0.0)]
        [InlineData(
            4.75, 135.0,
            -3.3587572106361003, 3.3587572106361008)]
        [InlineData(
            5.25, 45.0,
            3.7123106012293747, 3.7123106012293747)]
        [InlineData(
            5.0, 0.0,
            5.0, 0.0)]
        [InlineData(
            5.0, -45.0,
            3.5355339059327378, -3.5355339059327378)]
        [InlineData(
            6.5, -90.0,
            3.9801020972288977E-16, -6.5)]
        [InlineData(
            4.0, -155.0,
            -3.6252311481465997, -1.690473046962798)]
        [InlineData(
            5.0, 180.0,
            -5.0, 6.123233995736766E-16)]
        [InlineData(
            6.0, -120.0,
            -2.9999999999999987, -5.196152422706632)]
        public void DecomposeVelocity_CtandartCase_ExpectedResult(
            double speed, double angle,
            double expDx, double expDy)
        {
            //Act

            (double dx, double dy) = MathHelper.DecomposeVelocity(speed, angle);

            //Assert

            Assert.Equal(expDx, dx, _config.Precision);
            Assert.Equal(expDy, dy, _config.Precision);
        }
    }
}
