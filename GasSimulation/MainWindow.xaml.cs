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

            (var uiElems, var elemStates) = ElementsInitializer.Initialize(this);
            Simulation.Simulation.Initialize(uiElems, elemStates);
            SimulationTimer.Initialize();
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