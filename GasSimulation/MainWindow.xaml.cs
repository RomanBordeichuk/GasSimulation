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

            (var uiElems, var elemStates) = ElementsInitializer.Initialize(
                config, configManager.GetElemInitStates(), this);

            Simulation.Simulation.Initialize(config, uiElems, elemStates);
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