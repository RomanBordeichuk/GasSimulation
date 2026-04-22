namespace GasSimulation.Simulation.Exceptions
{
    public class IncorrectTypeException : ArgumentException
    {
        public IncorrectTypeException()
            : base("Incorrect element type") { }
    }
}
