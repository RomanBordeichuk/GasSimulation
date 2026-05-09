using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.IterationCalculator;
using System.Diagnostics.CodeAnalysis;

namespace GasSimulation.Tests.Unit.IterationCalculator
{
    public class CollisionCalculatorTests : TestsBase
    {
        [Theory]
        [InlineData(
            473.4294919243113, 237.27980021920965, 1.7500000000000013, 3.031088913245534,
            478.5, 248.7, -3.5, 0,
            1,
            0.548641854933463, 1.3499926755451952)]
        [InlineData(
            380, 200, 4, 0,
            390, 203, -5.5, 6.735557395310443E-16,
            1,
            0.04848505114005726, 0.30469265401539813)]
        [InlineData(
            488.0, 200.0, 4.0, 0.0,
            500.0, 200.0, 0.0, 0.0,
            1.0,
            0.5, 0.0)]
        [InlineData(
            300.0, 300.0, 2.4492935982947064E-16, 4.0,
            301.0, 312.5, 2.143131898507868E-16, -3.5,
            1,
            0.34001675052450664, 1.4706289056333368)]
        [InlineData(
            100.0, 100.0, 0.0, 0.0,
            300.0, 100.0, 0.0, 0.0,
            1,
            null, null)]
        [InlineData(
            300.0, 300.0, 2.4492935982947064E-16, 4.0,
            301.0, 312.5, 2.143131898507868E-16, -3.5,
            -1,
            null, null)]
        [SuppressMessage("SonarLint", "S107")]
        public void CalculateAtomTAndAngle_StandartCase_ExpectedReturn(
            double x1, double y1, double dx1, double dy1, 
            double x2, double y2, double dx2, double dy2,
            double remT,
            double? expT, double? expAngle)
        {
            //Arrange

            AtomState atomState1 = new(new(x1, y1), new(dx1, dy1));
            AtomState atomState2 = new(new(x2, y2), new(dx2, dy2));

            //Act

            (double? t, double? angle) = CollisionCalculator.CalculateAtomTAndAngle(_config, atomState1, atomState2, remT);

            //Assert

            if(expT == null)
            {
                Assert.Null(t);
                Assert.Null(angle);
            }
            else
            {
                Assert.Equal(expT.Value, t!.Value, _config.Simulation.Precision);
                Assert.Equal(expAngle!.Value, angle!.Value, _config.Simulation.Precision);
            }
        }

        [Theory]
        [InlineData(
            300.0, 301.36006700209805, 2.4492935982947064E-16, 4.0,
            301.0, 311.30994137316424, 2.4492935982947064E-16, -3.5,
            1.4706289056333368,
            -0.7462405778299648, -3.425, 0.7462405778299652, 3.9249999999999994)]
        [InlineData(
            401.3333333333333, 200.0, 4.0, 0.0,
            411.3333333333333, 200.0, -3.5, 4.286263797015736E-16,
            0.0,
            -3.5, 0.0, 4.0, 4.286263797015736E-16)]
        [InlineData(
            490.0, 200.0, 4.0, 0.0,
            500.0, 200.0, 0.0, 0.0,
            0.0,
            0.0, 0.0, 4.0, 0.0)]
        [InlineData(
            380.19394020456025, 203.0, 4.0, 0.0,
            389.7333322187297, 200.0, -5.5, 6.735557395310443E-16,
            -0.30469265401539813,
            -4.644999999999998, 2.7187267240383, 3.144999999999997, -2.7187267240382997)]
        [InlineData(
            378.9477953621885, 231.73464757277608, 3.7587704831436337, 1.3680805733026749,
            387.44084573752855, 237.01367166079996, -5.359035356318794, -1.2372307988912563,
            0.5561288905429723,
            -3.9861683436793434, -3.4459399281868057, 2.385903470504182, 3.576789702598224)]
        [SuppressMessage("SonarLint", "S107")]
        public void CalculateVelocities_StandartCase_ExpectedResult(
            double x1, double y1, double dx1, double dy1,
            double x2, double y2, double dx2, double dy2,
            double angle,
            double expV1x, double expV1y, double expV2x, double expV2y)
        {
            //Arrange

            AtomState atomState1 = new(new(x1, y1), new(dx1, dy1));
            AtomState atomState2 = new(new(x2, y2), new(dx2, dy2));

            //Act

            (VelocityState v1, VelocityState v2) = CollisionCalculator.CalculateVelocities(
                _config, atomState1, atomState2, angle);

            //Assert

            Assert.Equal(expV1x, v1.Dx, _config.Simulation.Precision);
            Assert.Equal(expV1y, v1.Dy, _config.Simulation.Precision);
            Assert.Equal(expV2x, v2.Dx, _config.Simulation.Precision);
            Assert.Equal(expV2y, v2.Dy, _config.Simulation.Precision);
        }

        [Theory]
        [InlineData(
            321.0, 220.0, 3.5, 0.0,
            500.0, 250.0, 150.0, 15.0, 1.5707963267948966,
            1.0,
            null, null)]
        [InlineData(
            429.5, 160.0, 3.5, 0.0,
            500.0, 200.0, 150.0, 15.0, 0.5235987755982988,
            1.0,
            0.3479907706414029, 0.5235987755982988)]
        [InlineData(
            440.0, 180.0, 3.5, 0.0,
            500.0, 200.0, 150.0, 15.0, 0.5235987755982988,
            1.0,
            0.10256681389212678, -1.0471975511965979)]
        [InlineData(
            424.0, 172.0, 3.5, 0.0,
            500.0, 200.0, 150.0, 15.0, 0.5235987755982988,
            1.0,
            0.9433446690318787, -0.6447040195953698)]
        [InlineData(
            485.5, 220, 3.5, 0.0,
            500.0, 250.0, 150.0, 15.0, 1.5707963267948966,
            1.0,
            0.5714285714285714, 0.0)]
        [InlineData(
            485.5, 248.7, 3.5, 0.0,
            500.0, 250.0, 150.0, 15.0, 1.5707963267948966,
            1.0,
            0.5714285714285714, 0.0)]
        [InlineData(
            432.5, 152.2, 3.5, 0.0,
            500.0, 200.0, 150.0, 15.0, 0.5235987755982988,
            1.0,
            0.8726033835251816, 0.8647944110510333)]
        [InlineData(
            559.0, 248.7, 3.5, 0.0,
            500.0, 200.0, 150.0, 15.0, 0.5235987755982988,
            1.0,
            0.1455265079464541, 1.916131946007825)]
        [InlineData(
            349.5, 285.7365149746596, 2.7500000000000004, 4.763139720814412,
            400.0, 300.0, 150.0, 15.0, 0.0,
            1.0,
            0.37023583785169256, 1.5707963267948966)]
        [InlineData(
            317.25, 290.499654695474, 2.7500000000000004, 4.763139720814412,
            400.0, 300.0, 150.0, 15.0, 0.0,
            1.0,
            0.9999999999999999, 0.0)]
        [InlineData(
            334.5, 285.7365149746596, 2.7500000000000004, 4.763139720814412,
            400.0, 300.0, 150.0, 15.0, 0.0,
            1.0,
            0.37023583785169256, 1.5707963267948966)]
        [InlineData(
            320.25, 290.499654695474, 2.7500000000000004, 4.763139720814412,
            400.0, 300.0, 150.0, 15.0, 0.0,
            1.0,
            0.03545547246872298, 0.37502220084435434)]
        [InlineData(472.21106775953757, 285.96084826764053, 5.062776693988422, 2.1490212066910055,
            400.0, 300.0, 150.0, 15.0, 0.0,
            1.0,
            0.7793049373617864, -1.3373796119619454)]
        [InlineData(
            323.0, 282.5, 3.3677786976552215E-16, 5.5,
            400.0, 300.0, 150.0, 15.0, 0.0,
            1.0,
            0.9849862372609183, 1.1592794807361373)]
        [InlineData(415.5, 208, 5.5, 0.0,
            500.0, 200.0, 150.0, 15, 0.0,
            1.0,
            0.822738693521163, -0.10016742115954966)]
        [SuppressMessage("SonarLint", "S107")]
        public void CalculateRectTAndAngle_StandartCase_ExpectedResult(
            double x1, double y1, double dx1, double dy1,
            double x2, double y2, double width, double height, double rectAngle,
            double remT,
            double? expT, double? expAngle)
        {
            //Arrange

            AtomState atomState = new(new(x1, y1), new(dx1, dy1));
            RectState rectState = new(new(x2, y2), new(width, height), rectAngle);

            //Act

            (double? t, double? angle) = CollisionCalculator.CalculateRectTAndAngle(
                _config, atomState, rectState, remT);

            //Accert

            if(expT == null)
            {
                Assert.Null(t);
                Assert.Null(angle);
            }
            else
            {
                Assert.Equal(expT.Value, t!.Value, _config.Simulation.Precision);
                Assert.Equal(expAngle!.Value, angle!.Value, _config.Simulation.Precision);
            }
        }

        [Theory]
        [InlineData(
            430.7179676972449, 160.0, 3.5, 0,
            0.5235987755982988,
            -1.7500000000000007, -3.031088913245535)]
        [InlineData(
            440.3589838486225, 180.0, 3.5, 0.0,
            -1.0471975511965979,
            1.7500000000000009, 3.0310889132455348)]
        [InlineData(
            427.3017063416116, 172.0, 3.5, 0,
            -0.6447040195953698,
            -0.9719136111002433, 3.3623479791000337)]
        [InlineData(
            350.51814855409214, 287.5, 2.7500000000000004, 4.763139720814412,
            -1.5707963267948966,
            2.7500000000000013, -4.763139720814412)]
        [InlineData(
            320.0, 295.26279441628844, 2.7500000000000004, 4.763139720814412,
            3.141592653589793,
            -2.7499999999999996, 4.763139720814412)]
        [InlineData(
            335.51814855409214, 287.5, 2.7500000000000004, 4.763139720814412,
            1.5707963267948966,
            2.7499999999999996, -4.763139720814412)]
        [InlineData(
            323.0, 287.917424304935, 3.3677786976552215E-16, 5.5,
            1.1592794807361373,
            -4.0326666114958485, -3.7400000000704003)]
        [InlineData(
            420.0250628143664, 208, 5.5, 0.0,
            -0.10016742115954966,
            -5.3900000000044, 1.0944861807956126)]
        public void CalculateVelocity_StandartCase_ExpectedResult(
            double x, double y, double dx, double dy,
            double angle,
            double expDx, double expDy)
        {
            //Arrange

            AtomState atomState = new(new(x, y), new(dx, dy));

            //Act

            VelocityState v = CollisionCalculator.CalculateVelocity(atomState, angle);

            //Assert

            Assert.Equal(expDx, v.Dx, _config.Simulation.Precision);
            Assert.Equal(expDy, v.Dy, _config.Simulation.Precision);
        }
    }
}
