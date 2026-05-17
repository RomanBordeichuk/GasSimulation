using GasSimulation.Configuration.ConfigParts;
using GasSimulation.Exceptions;
using GasSimulation.Transformers.ConfigInitStateTransformer.DTOs;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GasSimulation.Configuration
{
    public class ConfigManager
    {
        private readonly IConfiguration _configuration;
        private readonly SimulationConfig _simulationConfig;

        public ConfigManager(string configFileName)
         {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFileName);

            _configuration = builder.Build();
            _simulationConfig = _configuration.GetSection("SimulationConfig")
                .Get<SimulationConfig>()
                ?? throw new ConfigErrorException();
        }

        public SimulationConfig GetSimulationConfig()
        {
            return _simulationConfig;
        }

        public Config GetConfig()
        {
            var ColorsConfig = _configuration.GetSection("ColorsConfig")
                .Get<ColorsConfig>()
                ?? throw new ConfigErrorException();

            var DebugConfig = _configuration.GetSection("DebugConfig")
                .Get<DebugConfig>()
                ?? throw new ConfigErrorException();

            return new Config(_simulationConfig, ColorsConfig, DebugConfig);
        }

        public List<ConfigInitState> GetElemInitStates()
        {
            var initStates = _configuration.GetSection("InitStates")
                ?? throw new ConfigErrorException();

            var variants = initStates.GetSection("Variants")
                ?? throw new ConfigErrorException();

            var activeVariants = initStates.GetSection("Active").Get<List<int>>()
                ?? throw new ConfigErrorException();

            return MapVariantsAndFilter(variants, activeVariants);
        }

        private static List<ConfigInitState> MapVariantsAndFilter(
            IConfigurationSection variants, List<int> activeVariants)
        {
            var rawVariantsList = variants.Get<List<ConfigInitState>>()
                ?? throw new ConfigErrorException();

            List<ConfigInitState> resList = new();

            foreach (int i in activeVariants)
            {
                resList.Add(rawVariantsList[i]);
            }

            return resList;
        }
    }
}
