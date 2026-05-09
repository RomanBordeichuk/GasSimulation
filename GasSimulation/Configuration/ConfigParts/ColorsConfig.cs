namespace GasSimulation.Configuration.ConfigParts
{
    public record ColorsConfig(byte[] Elem, byte[] GhostElem, 
        byte[] OccupiedCell, byte[] PartlyOccupiedCell, 
        byte[] Sector, byte[] Vector, byte[] Area);
}
