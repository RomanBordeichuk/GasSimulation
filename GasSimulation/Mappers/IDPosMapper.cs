using GasSimulation.GeneralDTOs;

namespace GasSimulation.Mappers
{
    public static class IDPosMapper
    {
        public static long MapToLong(this IDPosState idPos)
        {
            return (long)idPos.I << 32 | (uint)idPos.J;
        }

        public static IDPosState MapToIDPos(this long num)
        {
            return new((int)(num >> 32), (int)num);
        }
    }
}
