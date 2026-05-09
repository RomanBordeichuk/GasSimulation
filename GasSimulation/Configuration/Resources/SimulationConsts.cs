using System.Diagnostics.CodeAnalysis;

namespace GasSimulation.Configuration.DTOs
{
    public struct SimulationConsts
    {
        public double FPS { get; }
        public double SpeedMult { get; }
        public double StartPosX { get; }
        public double StartPosY { get; }
        public double ZoomScale { get; }
        public double Restitution { get; }
        public double ErrorRate { get; }
        public double AtomDiameter { get; }
        public double SectorSizeMult { get; }
        public int PasteAtomAttempts { get; }
        public double Mult { get; }
        public int Precision { get; }
        public double Sqrt2 { get; }


        [SuppressMessage("SonarLint", "S107")]
        public SimulationConsts(double fps, double speedMult, double startPosX,
        double startPosY, double zoomScale, double restitution, double errorRate,
        double atomDiameter, double sectorSizeMult, int pasteAtomAttempts)
        {
            FPS = fps;
            SpeedMult = speedMult;
            StartPosX = startPosX;
            StartPosY = startPosY;
            ZoomScale = zoomScale;
            Restitution = restitution;
            ErrorRate = errorRate;
            AtomDiameter = atomDiameter;
            SectorSizeMult = sectorSizeMult;
            PasteAtomAttempts = pasteAtomAttempts;

            Mult = speedMult / fps;
            Precision = (int)-Math.Log10(errorRate);
            Sqrt2 = Math.Sqrt(2);
        }

        public SimulationConsts Copy(double atomDiameter)
        {
            return new(FPS, SpeedMult, StartPosX, StartPosY, ZoomScale, 
                Restitution, ErrorRate, atomDiameter, SectorSizeMult, PasteAtomAttempts);
        }
    }
}
