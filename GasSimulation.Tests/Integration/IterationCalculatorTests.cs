using GasSimulation.Simulation;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.DTOs.Interfaces;
using GasSimulation.Simulation.IterationCalculator;

namespace GasSimulation.Tests.Integration
{
    public class IterationCalculatorTests
    {
        public static IEnumerable<object[]> MultipleAtomsNRects_StandartCase_ExpectedResult_Data()
        {
            yield return new object[]
            {
                new AtomState[]
                {
                    new (300.0, 200.0, 3.061616997868383E-16, 5.0),
                    new (304.0, 300.0, 2.755455298081545E-16, -4.5),
                },
                new RectState[] {},
                16,
                new AtomState[]
                {
                    new (277.57648510898383, 225.81345777453285, -3.4827575281664336, -2.9800000000000058),
                    new (326.42351489101617, 281.905760719673, 3.4827575281664336, 3.4800000000000044)
                }
            };

            yield return new object[]
            {
                new AtomState[]
                {
                    new (300.0, 200.0, 1.3781867790849958, 4.8063084796915945),
                    new (304.0, 300.0, 2.755455298081545E-16, -4.5),
                },
                new RectState[] {},
                12,
                new AtomState[]
                {
                    new (316.53824134902015, 257.6757017562993, 1.3781867790849958, 4.8063084796915945),
                    new (304.0, 246.0, 2.755455298081545E-16, -4.5)
                }
            };

            yield return new object[]
            {
                new AtomState[]
                {
                    new(240, 152.2, 3.5, 0),
                    new(270, 172, 3.5, 0),
                    new(300, 180, 3.5, 0),
                    new(300, 160, 3.5, 0),
                    new(300, 220, 3.5, 0),
                    new(300, 240, 3.5, 0)
                },
                new RectState[]
                {
                    new(500, 200, 150, 15, 0.5235987755982988),
                    new(500, 250, 150, 15, 1.5707963267948966)
                },
                70,
                new AtomState[]
                {
                    new(440.31868096463404, 103.37619347415364, 0.5534410321230949, -3.4559662938116924),
                    new(399.64709563518414, 256.2491944150868, -0.9719136111002433, 3.3623479791000337),
                    new(452.8783710291364, 234.9406109174932, -1.3898131082370906, 0.30998177400039917),
                    new(372.3589838486225, 61.028856829700224, -1.7500000000000007, -3.031088913245535),
                    new(434.28066502643287, 198.19699326929643, -3.2333288160570945, -1.815352010878191),
                    new(467.41996694296614, 298.4737459968266, -0.6268580757058138, 4.536459150123328)
                }
            };
        }

        [Theory]
        [MemberData(nameof(MultipleAtomsNRects_StandartCase_ExpectedResult_Data))]
        public void MultipleAtomsNRects_StandartCase_ExpectedResult(
            AtomState[] atoms, RectState[] rects, int iterations, AtomState[] expAtoms)
        {
            //Arrange

            List<IElemState> elems = new(atoms.Length + rects.Length);

            foreach (var atom in atoms) elems.Add(atom);
            foreach (var rect in rects) elems.Add(rect);

            int precision = (int)-Math.Log10(Constants.ErrorRate);

            //Act

            for (int i = 0; i < iterations; i++) IterationCalculator.Calculate(elems);

            //Assert

            for (int i = 0; i < atoms.Length; i++)
            {
                AtomState atom = (AtomState)elems[i];

                Assert.Equal(expAtoms[i].X, atom.X, precision);
                Assert.Equal(expAtoms[i].Y, atom.Y, precision);
                Assert.Equal(expAtoms[i].Dx, atom.Dx, precision);
                Assert.Equal(expAtoms[i].Dy, atom.Dy, precision);
            }
        }
    }
}
