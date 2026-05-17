using GasSimulation.Builders;
using GasSimulation.Configuration;
using GasSimulation.Debuggers;
using GasSimulation.Simulation;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace GasSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Config _config = null!;
        private MainVisualDebugger _debugger = null!;
        private SimulationCore _simulation = null!;

        private Point _lastMousePos;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var configManager = new ConfigManager("SimulationConfig.json");
            _config = configManager.GetConfig();

            var serviceConfigurator = new ServiceConfigurator(_config, this.DebugCanvas, this.SimulationField);
            var services = serviceConfigurator.Provider;

            _debugger = services.GetRequiredService<MainVisualDebugger>();
            var simulationBuilder = services.GetRequiredService<SimulationBuilder>();

            var rawInitState = configManager.GetElemInitStates();

            _simulation = simulationBuilder.Build(rawInitState);
            var rendererBuilder = services.GetRequiredService<UIRendererBuilder>();
            var iterationCalculatorDebugger = services.GetRequiredService<IterationCalculatorVisualDebugger>();

            var renderer = rendererBuilder.Build(_simulation);
            renderer.SubscribeOnRendering(iterationCalculatorDebugger.IsDisabled());
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool isCtrlPressed = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);

            switch (e.Key){
                case Key.Space:
                    _simulation.Toggle();

                    break;

                case Key.Tab:
                    if (isCtrlPressed) _debugger.StepBack();
                    else _debugger.StepNext();

                    break;
            }

            e.Handled = true;
        }

        private void Window_Scroll(object sender, MouseWheelEventArgs e)
        {
            var pos = e.GetPosition(this.MainContainer);
            double zoom = e.Delta > 0 ? _config.Simulation.ZoomScale 
                : 2 - _config.Simulation.ZoomScale;

            var matrix = this.ScaleMatrix.Matrix;
            matrix.ScaleAtPrepend(zoom, zoom, pos.X, pos.Y);
            this.ScaleMatrix.Matrix = matrix;

            e.Handled = true;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(this);
                var delta = pos - _lastMousePos;
                var matrix = this.ScaleMatrix.Matrix;

                matrix.Translate(delta.X, delta.Y);
                this.ScaleMatrix.Matrix = matrix;
            }

            _lastMousePos = e.GetPosition(this);
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            _simulation.Stop();
        }
    }
}