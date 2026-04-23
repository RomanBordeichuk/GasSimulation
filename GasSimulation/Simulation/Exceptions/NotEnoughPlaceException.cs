namespace GasSimulation.Simulation.Exceptions
{
    public class NotEnoughPlaceException : Exception
    {
        public NotEnoughPlaceException()
            : base("not anough place for all atoms") { } 
    }
}
