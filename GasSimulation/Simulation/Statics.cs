namespace GasSimulation.Simulation
{
    public static class Statics
    {
        public static readonly double Sqrt2 = Math.Pow(2, 0.5);
        public static readonly double Mult = Constants.SpeedMult / Constants.FPS;
        public static readonly Random Rand = new Random();
    }
}
