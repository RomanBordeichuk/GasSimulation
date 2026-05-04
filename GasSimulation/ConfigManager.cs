using GasSimulation.Exceptions;
using GasSimulation.Simulation.InitStateTransformer.DTOs;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GasSimulation
{
    public class ConfigManager
    {
        private readonly IConfiguration _configuration;
        private readonly Config _simulationConfig;

        public ConfigManager(string configFileName)
         {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFileName);

            _configuration = builder.Build();
            _simulationConfig = _configuration.GetSection("SimulationConstants").Get<Config>()
                ?? throw new ConfigErrorException();
        }

        public Config GetConfig()
        {
            return _simulationConfig;
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
