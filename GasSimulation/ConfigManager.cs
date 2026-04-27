using GasSimulation.Exceptions;
using GasSimulation.Logs;
using GasSimulation.Simulation.DTOs;
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

        public void SetLogsState()
        {
            Logger.Enabled = _configuration.GetSection("EnableLogs").Get<bool>();
        }

        public List<AllConfigInitState> GetElemInitStates()
        {
            var initStates = _configuration.GetSection("InitStates")
                ?? throw new ConfigErrorException();

            var variants = initStates.GetSection("Variants")
                ?? throw new ConfigErrorException();

            var activeVariants = initStates.GetSection("Active").Get<List<int>>()
                ?? throw new ConfigErrorException();

            return MapVariantsAndFilter(variants, activeVariants);
        }

        private static List<AllConfigInitState> MapVariantsAndFilter(
            IConfigurationSection variants, List<int> activeVariants)
        {
            var rawVariantsList = variants.Get<List<AllConfigInitState>>()
                ?? throw new ConfigErrorException();

            List<AllConfigInitState> resList = new();

            foreach (int i in activeVariants)
            {
                resList.Add(rawVariantsList[i]);
            }

            return resList;
        }
    }
}
