using GasSimulation.Configuration;
using GasSimulation.Logs;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Transformers.ConfigInitStateTransformer;
using GasSimulation.Transformers.ConfigInitStateTransformer.DTOs;
using System.Diagnostics;

namespace GasSimulation.Simulation
{
    public class SimulationCore
    {
        private readonly IterationCalculator.IterationCalculator _iterationCalculator;
        private readonly ConfigInitStateTransformer _configInitStateTransformer;
        private readonly ManualResetEventSlim _coreResetEvent;
        private readonly FileLogger _fileLogger;

        private readonly ManualResetEventSlim _timeoutResetEvent;
        private AllStates _snapshot;
        private int _iteration = 0;
        private bool _snapshotUpdated = true;
        private bool _isPaused = true;
        private bool _disposed = false;

        public SimulationCore(Config config,
            IterationCalculator.IterationCalculator iterationCalculator,
            ConfigInitStateTransformer configInitStateTransformer,
            ManualResetEventSlim coreResetEvent,
            FileLogger logger,
            List<ConfigInitState> rawInitState)
        {
            _iterationCalculator = iterationCalculator;
            _configInitStateTransformer = configInitStateTransformer;
            _coreResetEvent = coreResetEvent;
            _fileLogger = logger;

            _timeoutResetEvent = new();
            _snapshot = new();

            var coreThread = new Thread(() =>
            {
                var frameDelta = 1000 / config.Simulation.FPS;

                Stopwatch sw = Stopwatch.StartNew();
                double startTime = 0;
                double endTime = 0;
                int delay = 0;

                var allStates = ProcessPreIteration(rawInitState);
                UpdateShapshot(allStates);
                _coreResetEvent.Wait();

                while (!_disposed)
                {
                    startTime = sw.Elapsed.TotalMilliseconds;

                    //DebugLogger.Log($"Running... Iteration: {_iteration}");
                    Trace.WriteLine($"Running... Iteration: {_iteration}");

                    UpdateShapshot(allStates);
                    ProcessIteration(allStates);

                    ////
                    //if (_iteration == 0)
                    //{
                    //    Trace.WriteLine("Logging...");
                    //    _fileLogger.LogAllStates(allStates, _iteration);
                    //}
                    ////

                    //
                    if (_iteration == 2000)
                    {
                        Trace.WriteLine("Logging...");
                        _fileLogger.LogAllStates(allStates, _iteration);
                    }
                    //

                    _iteration++;

                    endTime = sw.Elapsed.TotalMilliseconds;
                    delay = (int)(frameDelta - (endTime - startTime));

                    _timeoutResetEvent.Wait(delay < 0 ? 0 : delay);
                    _coreResetEvent.Wait();
                }
            });

            coreThread.Priority = ThreadPriority.AboveNormal;
            coreThread.Start();
        }

        private AllStates ProcessPreIteration(List<ConfigInitState> rawInitState)
        {
            return _configInitStateTransformer.Transform(rawInitState);
        }

        private void ProcessIteration(AllStates allStates)
        {
            _iterationCalculator.Calculate(allStates, _iteration);
        }

        private void UpdateShapshot(AllStates allStates)
        {
            if (Interlocked.CompareExchange(ref _snapshotUpdated, false, true))
            {
                _snapshot = new(allStates.Atoms, allStates.Rects);
                _snapshotUpdated = true;
            }
        }

        public void Toggle()
        {
            if (_isPaused)
            {
                _coreResetEvent.Set();
                _isPaused = false;
            }
            else
            {
                _coreResetEvent.Reset();
                _isPaused = true;
            }
        }

        public void Stop()
        {
            _disposed = true;
            _coreResetEvent.Set();
        }

        public AllStates GetSnapshot()
        {
            while (true)
            {
                if (_snapshotUpdated) return _snapshot;
            }
        }
    }
}
