namespace GasSimulation.Configuration.ConfigParts
{
    public record SimulationConfig(double FPS, double SpeedMult, double StartPosX, 
        double StartPosY, double ZoomScale, double Restitution, double ErrorRate, 
        double AtomDiameter, double SectorSizeMult, int PasteAtomAttempts);
}
