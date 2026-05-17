namespace GasSimulation.Configuration.ConfigParts
{
    public record DebugConfig(ActiveVisualDebugModule[] DebugModules);

    public enum ActiveVisualDebugModule
    {
        GasGenerator, IterationCalculator, SectorTransformer
    }
}
