using GasSimulation.Simulation;
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

            var configManager = new ConfigManager("SimulationConfig.json");

            configManager.SetLogsState();
            var config = configManager.GetConfig();

            var rawInitStates = configManager.GetElemInitStates();
            var elemStates = ConfigInitStateTransformer.Transform(config, rawInitStates);

            Simulation.Simulation.Initialize(config, this.ParticleRenderer, elemStates);
            SimulationTimer.Initialize(config);
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                SimulationTimer.Toggle();
            }
        }
    }
}