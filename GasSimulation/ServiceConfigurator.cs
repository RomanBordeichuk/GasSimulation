using GasSimulation.Builders;
using GasSimulation.Configuration;
using GasSimulation.Debuggers;
using GasSimulation.Simulation.GasGenerator;
using GasSimulation.Simulation.InitStateTransformer;
using GasSimulation.Simulation.IterationCalculator;
using GasSimulation.UIRendering;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace GasSimulation
{
    public class ServiceConfigurator
    {
        private readonly ServiceProvider _provider;

        public ServiceConfigurator(Config config, Canvas? canvas, 
            SimulationField? field, int? randomSeed = null)
        {
            var rand = randomSeed != null ? new Random(randomSeed.Value) : new Random();

            var services = new ServiceCollection();

            if (canvas != null)
            {
                services.AddSingleton<Canvas>(canvas);
                services.AddSingleton<DebugCanvas>();
            }
            if (field != null) services.AddSingleton<SimulationField>(field);

            services.AddSingleton<Random>(rand);
            services.AddSingleton<Config>(config);
            services.AddSingleton<VisualDebugger>();
            services.AddSingleton<GasGeneratorVisualDebugger>();
            services.AddSingleton<EmptyCellFiller>();
            services.AddSingleton<RandomCellFiller>();
            services.AddSingleton<AtomGenerator>();
            services.AddSingleton<GasGenerator>();
            services.AddSingleton<SectorCalculatorVisualDebugger>();
            services.AddSingleton<SectorCalculator>();
            services.AddSingleton<ConfigInitStateTransformer>();
            services.AddSingleton<SimulationBuilder>();
            services.AddSingleton<SimulationTimerBuilder>();
            services.AddSingleton<SectorTransformer>();
            services.AddSingleton<IterationCalculator>();
            services.AddSingleton<SectorTransformerVisualDebugger>();
            services.AddSingleton<MainVisualDebugger>();

            _provider = services.BuildServiceProvider();
        }

        public ServiceProvider Provider => _provider; 
    }
}
