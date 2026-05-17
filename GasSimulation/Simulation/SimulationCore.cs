using GasSimulation.Configuration;
using GasSimulation.GeneralDTOs.Atom;
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
            List<ConfigInitState> rawInitState)
        {
            _iterationCalculator = iterationCalculator;
            _configInitStateTransformer = configInitStateTransformer;

            _coreResetEvent = coreResetEvent;
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

                while (!_disposed)
                {
                    startTime = sw.Elapsed.TotalMilliseconds;

                    Logger.Log($"Running... Iteration: {_iteration}");
                    Logger.Log($"Selected atom: {_snapshot.SelectedAtom}");
                    UpdateShapshot(allStates);
                    ProcessIteration(allStates);

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
            _iterationCalculator.Calculate(allStates);
        }

        private void UpdateShapshot(AllStates allStates)
        {
            if (Interlocked.CompareExchange(ref _snapshotUpdated, false, true))
            {
                _snapshot = new(allStates.Atoms, allStates.Rects, _snapshot.SelectedAtom);
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

        public void UnfocusAtom()
        {
            _snapshot.SelectedAtom = null;
        }

        public void FocusRandomAtom()
        {
            _snapshot.SelectedAtom = Random.Shared.Next(_snapshot.Atoms.Length);
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
