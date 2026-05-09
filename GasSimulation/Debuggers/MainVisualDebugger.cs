using GasSimulation.Configuration;
using System.Diagnostics;

namespace GasSimulation.Debuggers
{
    public class MainVisualDebugger : ConcreteVisualDebugger
    {
        public MainVisualDebugger(Config config, VisualDebugger debugger)
            : base (config, debugger) { }

        public void StepNext()
        {
            if (_disabled) return;
            
            _debugger.StepNext();
        }

        [Conditional("DEBUG")]
        public void StepBack()
        {
            if (_disabled) return;

            _debugger.StepBack();
        }
    }
}
