namespace GasSimulation.Exceptions
{
    public class ConfigErrorException : Exception
    {
        public ConfigErrorException()
            : base("An error occured while reading configuration") { }
    }
}
