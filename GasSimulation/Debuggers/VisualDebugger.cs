using GasSimulation.GeneralDTOs.Interfaces;
using GasSimulation.UIRendering;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;

namespace GasSimulation.Debuggers
{
    public static class VisualDebugger
    {
        private static Config _config = null!;

        [Conditional("DEBUG")]
        public static void Initialize(Config config, Canvas canvas)
        {
            _config = config;
            DebugCanvas.Initialize(config, canvas);
        }

        [Conditional("DEBUG")]
        public static void ClearGroup(string name)
        {
            DebugCanvas.ClearGroup(name);
        }

        [Conditional("DEBUG")]
        public static void ClearAll()
        {
            DebugCanvas.ClearAll();
        }

        [Conditional("DEBUG")]
        public static void Draw<T>(string groupName, T item, SolidColorBrush brush)
            where T : IElemState
        {
            DebugCanvas.Draw<T>(groupName, item, brush);
        }

        [Conditional("DEBUG")]
        public static void DrawHollow<T>(string groupName, T item, 
            double border, SolidColorBrush brush)
            where T : IElemState
        {
            DebugCanvas.DrawHollow<T>(groupName, item, border, brush);
        }

        public static async Task Stop()
        {
#if DEBUG
            _config.DebuggerWaitHandler = new();
            await _config.DebuggerWaitHandler.Task;
#endif
        }
    }
}
