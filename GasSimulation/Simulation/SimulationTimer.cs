using GasSimulation.Simulation.Loggers;
using System.Diagnostics;
using System.Windows.Threading;

namespace GasSimulation.Simulation
{
    static class SimulationTimer
    {
        private static DispatcherTimer _timer = null!;

        private static bool _paused = true;

        static public void Initialize()
        {
            _timer = new(DispatcherPriority.Render);

            int iteration = 0;

            _timer.Interval = TimeSpan.FromMilliseconds(1000 / Constants.FPS);
            _timer.Tick += (s, e) =>
            {
                Logger.Log($"Running... Iteration: {iteration}");

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
    }
}
