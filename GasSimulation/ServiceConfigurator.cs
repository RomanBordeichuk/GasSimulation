using GasSimulation.Builders;
using GasSimulation.Configuration;
using GasSimulation.Debuggers;
using GasSimulation.Logs;
using GasSimulation.Simulation.GasGenerator;
using GasSimulation.Simulation.IterationCalculator;
using GasSimulation.Simulation.IterationCalculator.Helpers;
using GasSimulation.Transformers.ConfigInitStateTransformer;
using GasSimulation.UIRendering;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace GasSimulation
{
    public class ServiceConfigurator
    {
        private readonly ServiceProvider _provider;

        public ServiceConfigurator(Config config, Canvas? canvas, 
            SimulationField? field, int? randomSeed, string? filePath)
        {
            var services = new ServiceCollection();

            services.AddSingleton<Config>(config);

            var rand = randomSeed != null ? new Random(randomSeed.Value) : new Random();
            services.AddSingleton<Random>(rand);

            if (canvas != null)
            {
                services.AddSingleton<Canvas>(canvas);
                services.AddSingleton<DebugCanvas>();
            }
            if (field != null) services.AddSingleton<SimulationField>(field);

            var fileLogger = new FileLogger(filePath);
            services.AddSingleton<FileLogger>(fileLogger);

            services.AddSingleton<VisualDebugger>();
            services.AddSingleton<GasGeneratorVisualDebugger>();
            services.AddSingleton<EmptyCellFiller>();
            services.AddSingleton<RandomCellFiller>();
            services.AddSingleton<AtomGenerator>();
            services.AddSingleton<GasGenerator>();
            services.AddSingleton<IterationCalculatorVisualDebugger>();
            services.AddSingleton<ConfigInitStateTransformer>();
            services.AddSingleton<SimulationBuilder>();
            services.AddSingleton<SectorPartitioner>();
            services.AddSingleton<CollisionCalculatorHelper>();
            services.AddSingleton<CollisionCalculator>();
            services.AddSingleton<ClosestCollisionCalculator>();
            services.AddSingleton<IterationCalculator>();
            services.AddSingleton<SectorPartitionerVisualDebugger>();
            services.AddSingleton<MainVisualDebugger>();
            services.AddSingleton<UIRendererBuilder>();
            services.AddSingleton<ManualResetEventSlim>();

            _provider = services.BuildServiceProvider();
        }

        public ServiceProvider Provider => _provider; 
    }
}
