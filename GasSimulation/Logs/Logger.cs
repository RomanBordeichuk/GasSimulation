using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.DTOs.Config;
using GasSimulation.Simulation.DTOs.Interfaces;
using System.Diagnostics;

namespace GasSimulation.Logs
{
    public static class Logger
    {
        private static bool _stateSetted = false;
        private static bool _enabled = false;

        public static bool Enabled
        {
            get => _enabled;
            set
            {
                if (_stateSetted)
                    throw new InvalidOperationException("Logger state already setted");
                else
                {
                    _enabled = value;
                    _stateSetted = true;
                }
            }
        }

        public static void Log(string message)
        {
            if (_enabled) Debug.WriteLine(message);
        }

        public static void LogInitState(this List<AtomConfigInitState> atoms)
        {
            Log("Init state:");

            for (int i = 0; i < atoms.Count; i++)
            {
                Log($"Atom {i}");
                Log($"x: {atoms[i].X}, y: {atoms[i].Y}, " +
                    $"speed: {atoms[i].Speed}, angle: {atoms[i].Angle}");
                Log("----------------");
            }

            Log("----------------");
        }

        public static void LogData(this List<IElemState> elems)
        {
            Log("Iteration data");

            for (int i = 0; i < elems.Count; i++)
            {
                if (elems[i] is AtomState atom)
                {
                    Log($"Atom {i}");
                    Log($"x: {atom.X}, y: {atom.Y}, dx: {atom.Dx}, dy: {atom.Dy}");
                }
                else if (elems[i] is RectState rect)
                {
                    Log($"Rect {i}");
                    Log($"x: {rect.X}, y: {rect.Y}, width: {rect.Width}, height: {rect.Height}, " +
                        $"angle: {rect.Angle}");
                }
            }

            Log("---------------------");
        }
    }
}
