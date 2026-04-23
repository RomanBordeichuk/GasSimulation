using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.DTOs.Interfaces;
using System.Diagnostics;

namespace GasSimulation.Simulation.Loggers
{
    public static class Logger
    {
        public static void Log(string message)
        {
            //Debug.WriteLine(message);
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
