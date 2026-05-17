namespace GasSimulation.GeneralDTOs
{
    public struct IDPosState
    {
        public int I { get; }
        public int J { get; }

        public IDPosState(int i, int j)
        {
            I = i; 
            J = j;
        }
    }
}
