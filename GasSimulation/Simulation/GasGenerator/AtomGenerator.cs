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

        public AtomConfigInitState Generate(PosState pos, double speed)
        {
            double randAngle = (_rand.NextDouble() - 0.5) * 360;

            return new(pos.X, pos.Y, speed, randAngle);
        }
    }
}
