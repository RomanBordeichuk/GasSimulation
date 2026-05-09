using GasSimulation.Configuration;
using GasSimulation.Logs;

namespace GasSimulation.Simulation
{
    public class SimulationTimer
    {
        private readonly Config _config;
        private readonly Simulation _simulation;

        private PeriodicTimer _timer = null!;
        private int _iteration = 0;
        private bool _paused = true;

        public SimulationTimer(Config config, Simulation simulation)
        {
            _config = config;
            _simulation = simulation;
        }

        public void Toggle()
        {
            if (_paused)
            {
                _paused = false;
                Logger.Log("Starting...");
                StartTimer();
            }
            else
            {
                _paused = true;
                Logger.Log("Stopping...");
                _timer.Dispose();
            }
        }

        private void StartTimer()
        {
            Task.Run(async () =>
            {
                _timer = new(TimeSpan.FromMilliseconds(1000 / _config.Simulation.FPS));

                while (await _timer.WaitForNextTickAsync())
                {
                    Logger.Log($"Running... Iteration: {_iteration}");

                    await _simulation.Run();

                    _iteration++;
                }
            });
        }
    }
}
