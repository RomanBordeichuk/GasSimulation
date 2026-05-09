using GasSimulation.Configuration.ConfigParts;
using GasSimulation.Configuration.DTOs;

namespace GasSimulation.Mappers
{
    public static class SimulationConstsMapper
    {
        public static SimulationConsts Map(this SimulationConfig config)
        {
            return new SimulationConsts(
                config.FPS,
                config.SpeedMult,
                config.StartPosX, 
                config.StartPosY,
                config.ZoomScale,
                config.Restitution,
                config.ErrorRate,
                config.AtomDiameter,
                config.SectorSizeMult,
                config.PasteAtomAttempts);
        }
    }
}
