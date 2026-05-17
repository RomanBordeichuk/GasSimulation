using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator;
using Microsoft.Extensions.DependencyInjection;

namespace GasSimulation.Tests.Integration
{
    public class IterationCalculatorTests : TestsBase
    {
        public static IEnumerable<object[]> MultipleAtomsNRects_StandartCase_ExpectedResult_Data()
        {
            yield return new object[]
            {
                new AtomState[]
                {
                    new (300.0, 200.0, 1.3781867790849958, 4.8063084796915945),
                    new (304.0, 300.0, 2.755455298081545E-16, -4.5),
                },
                new RectState[] {},
                16,
                new AtomState[]
                {
                    new (322.0509884653602, 276.90093567506574, 1.3781867790849958, 4.8063084796915945),
                    new (304.0, 228.0, 2.755455298081545E-16, -4.5)
                }
            };

            yield return new object[]
            {
                new AtomState[]
                {
                    new (500.0, 200.0, -3.3587572106361003, 3.3587572106361008),
                    new (300.0, 200.0, 3.7123106012293747, 3.7123106012293747),
                    new (400.0, 200.0, 5.0, 0.0),
                    new (320.0, 250.0, 3.5355339059327378, -3.5355339059327378),
                    new (400.0, 700.0, 3.9801020972288977E-16, -6.5),
                    new (700.0, 300.0, -5.0, 6.123233995736766E-16),
                    new (640.0, 650.0, -2.9999999999999987, -5.196152422706632)
                },
                new RectState[]
                {
                    new (550.0, 400.0, 150.0, 20.0, 0.5235987755982988),
                },
                65,
                new AtomState[]
                {
                    new (282.9806162912199, 405.35336474023296, -3.3234900395047724, 3.006967178600534),
                    new (403.50703230398375, 411.69067008686767, -5.040320684633784, 1.3449564040946282),
                    new (725.0, 200.0, 5.0, 0.0),
                    new (549.8097038856296, 20.190296114372046, 3.5355339059327378, -3.5355339059327378),
                    new (400.0, 310.0, 7.960204194457795E-16, -4.874219293651037E-32),
                    new (375.0, 267.5, -5.0, -6.5),
                    new (511.62436638802393, 475.8420461020283, 0.7135300363812647, 6.189886662811453)
                }
            };

            yield return new object[]
            {
                new AtomState[]
                {
                    new (240.0, 152.2, 3.5, 0.0),
                    new (270.0, 172.0, 3.5, 0.0),
                    new (300.0, 180.0, 3.5, 0.0),
                    new (300.0, 160.0, 3.5, 0.0),
                    new (300.0, 220.0, 3.5, 0.0),
                    new (300.0, 240.0, 3.5, 0.0)
                },
                new RectState[]
                {
                    new (500.0, 200.0, 150.0, 15.0, 0.5235987755982988),
                    new (500.0, 250.0, 150.0, 15.0, 1.5707963267948966)
                },
                70,
                new AtomState[]
                {
                    new (443.3727928069722, 103.37619347415361, 0.5534410321230949, -3.4559662938116924),
                    new (402.9488019767957, 256.2491944150868, -0.9719136111002433, 3.3623479791000337),
                    new (451.4843488283086, 235.44059033081695, -1.4794131932781869, 0.4849729437789722),
                    new (373.57695154586736, 61.02885682970024, -1.7500000000000007, -3.031088913245535),
                    new (435.652690162082, 196.44868376575906, -3.0204616875783925, -1.997944008683386),
                    new (465.54245293392074, 298.7325041683348, -0.7501251191434193, 4.544059978149949)
                }
            };
        }

        [Theory]
        [MemberData(nameof(MultipleAtomsNRects_StandartCase_ExpectedResult_Data))]
        public void MultipleAtomsNRects_StandartCase_ExpectedResult(
            AtomState[] atoms, RectState[] rects, int iterations, AtomState[] expAtoms)
        {
            //Arrange

            AllStates elemStates = new(atoms, rects);

            var serviceConfigurator = new ServiceConfigurator(_config, null, null);
            var services = serviceConfigurator.Provider;

            var iterationCalculator = services.GetRequiredService<IterationCalculator>();

            //Act

            for (int i = 0; i < iterations; i++) iterationCalculator.Calculate(elemStates);

            //Assert

            for (int i = 0; i < atoms.Length; i++)
            {
                AtomState atom = elemStates.Atoms[i];

                Assert.Equal(expAtoms[i].X, atom.X, _config.Simulation.Precision);
                Assert.Equal(expAtoms[i].Y, atom.Y, _config.Simulation.Precision);
                Assert.Equal(expAtoms[i].Dx, atom.Dx, _config.Simulation.Precision);
                Assert.Equal(expAtoms[i].Dy, atom.Dy, _config.Simulation.Precision);
            }
        }
    }
}
