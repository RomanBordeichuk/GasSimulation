using GasSimulation.Simulation.DTOs;

namespace GasSimulation.Simulation.GasGenerator
{
    public static class AtomGenerator
    {
        public static AtomInitState Generate(PosState pos, double avSpeed)
        {
            double randAngle = (Statics.Rand.NextDouble() - 0.5) * 360;

            return new(pos.X, pos.Y, avSpeed, randAngle);
        }
    }
}
