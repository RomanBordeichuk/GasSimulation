namespace GasSimulation.Tests
{
    public class TestsBase
    {
        protected Config _config;

        protected TestsBase()
        {
            var configManager = new ConfigManager("TestsConfig.json");
            _config = configManager.GetConfig();
        }
    }
}
