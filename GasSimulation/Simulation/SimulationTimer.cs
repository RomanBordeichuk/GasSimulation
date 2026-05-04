using GasSimulation.Debuggers;
using GasSimulation.Logs;
using System.Windows.Threading;

namespace GasSimulation.Simulation
{
    static class SimulationTimer
    {
        private static Config _config = null!;
        private static DispatcherTimer _timer = null!;
        private static bool _paused = true;

        static public void Initialize(Config config)
        {
            _config = config;

            _timer = new(DispatcherPriority.Render);

            int iteration = 0;

            _timer.Interval = TimeSpan.FromMilliseconds(1000 / config.FPS);
            _timer.Tick += async (s, e) =>
            {
                Logger.Log($"Running... Iteration: {iteration}");

                await VisualDebugger.Stop();

                Simulation.Run();

                iteration++;
            };
        }

        static public void Toggle()
        {
            if (_paused)
            {
                _paused = false;
                Logger.Log("Starting...");
                _timer.Start();
            }
            else
            {
                _paused = true;
                Logger.Log("Stopping...");
                _timer.Stop();
            }
        }

        static public void DebugStepNext()
        {
            _config.DebuggerWaitHandler.TrySetResult();
        }
    }
}
