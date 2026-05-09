using GasSimulation.Configuration;
using GasSimulation.Mappers;

namespace GasSimulation.Benchmarks.Benchmarks
{
    public class BenchmarksBase
    {
        protected Config _config;

        public BenchmarksBase()
        {
            var configManager = new ConfigManager("BenchmarksConfig.json");
            _config = new(configManager.GetSimulationConfig().Map());
        }
    }
}
