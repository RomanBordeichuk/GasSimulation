using GasSimulation.Debuggers;
using GasSimulation.Simulation;
using GasSimulation.Simulation.InitStateTransformer;
using System.Windows;
using System.Windows.Input;

namespace GasSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var configManager = new ConfigManager("SimulationConfig.json");
            var config = configManager.GetConfig();

            VisualDebugger.Initialize(config, this.DebugCanvas);
            SimulationTimer.Initialize(config);

            var rawInitStates = configManager.GetElemInitStates();
            var elemStates = await Task.Run(async () => 
                await ConfigInitStateTransformer.Transform(config, rawInitStates));

            Simulation.Simulation.Initialize(config, this.SimulationField, elemStates);

#if DEBUG
            SimulationTimer.Toggle();
#endif
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                SimulationTimer.Toggle();
            }
            else if (e.Key == Key.Tab)
            {
                SimulationTimer.DebugStepNext();
            }
        }
    }
}