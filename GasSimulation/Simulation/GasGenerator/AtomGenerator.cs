using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.DTOs.Config;

namespace GasSimulation.Simulation.GasGenerator
{
    public static class AtomGenerator
    {
        public static AtomConfigInitState Generate(Config config, PosState pos, double speed)
        {
            double randAngle = (config.Rand.NextDouble() - 0.5) * 360;

            return new(pos.X, pos.Y, speed, randAngle);
        }
    }
}
