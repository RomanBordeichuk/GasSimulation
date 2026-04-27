namespace GasSimulation
{
    public record Config
    {
        public double FPS { get; init; }
        public double SpeedMult { get; init; }
        public double StartPosX { get; init; }
        public double StartPosY { get; init; }
        public double Restitution { get; init; }
        public double ErrorRate { get; init; }
        public double AtomDiameter { get; init; }
        public int PasteAtomAttempts { get; init; }
        public string AtomColorHex { get; init; } = null!;
        public Random Rand { get; init; } = new Random();

        public double Mult => SpeedMult / FPS;
        public int Presicion => (int)-Math.Log10(ErrorRate);

        public static double Sqrt2 => Math.Sqrt(2);
    }
}
