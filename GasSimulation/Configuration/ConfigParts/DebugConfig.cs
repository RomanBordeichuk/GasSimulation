namespace GasSimulation.Configuration.ConfigParts
{
    public record DebugConfig( ActiveVisualDebugModule[] DebugModules, int DebugSteps);

    public enum ActiveVisualDebugModule
    {
        GasGenerator, IterationCalculator, SectorCalculator
    }
}
