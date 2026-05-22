using GasSimulation.GeneralDTOs;
using GasSimulation.Transformers.ConfigInitStateTransformer.DTOs;

namespace GasSimulation.Simulation.GasGenerator
{
    public class AtomGenerator
    {
        private readonly Random _rand;

        public AtomGenerator(Random rand)
        {
            _rand = rand;
        }

        public AtomConfigInitState Generate(PosState pos, double avSpeed)
        {
            double randAngle = (_rand.NextDouble() - 0.5) * 360;

            return new(pos.X, pos.Y, CalculateGaussian(avSpeed), randAngle);
        }

        private double CalculateGaussian(double avSpeed)
        {
            var k = Math.Sqrt(avSpeed / 0.5);
            var t = 0.5;
            double x = (_rand.NextDouble() - 0.5) * 2 * k;

            return k / t * Math.Exp(-((x / (k * t)) * (x / (k * t)))) + avSpeed;
        }
    }
}
