using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.InitStateTransformer.DTOs;
using System.Diagnostics;
using System.Windows;

namespace GasSimulation.Logs
{
    public static class Logger
    {
        [Conditional("DEBUG")]
        public static void Log(string message)
        {
            Debug.WriteLine(message);
        }

        [Conditional("DEBUG")]
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

        [Conditional("DEBUG")]
        public static void LogData(this AllStates elems)
        {
            Log("Iteration data");

            for (int i = 0; i < elems.Atoms.Count; i++)
            {
                var atom = elems.Atoms[i];

                Log($"Atom {i}");
                Log($"x: {atom.X}, y: {atom.Y}, dx: {atom.Dx}, dy: {atom.Dy}");
            }

            for (int i = 0; i < elems.Rects.Count; i++)
            {
                var rect = elems.Rects[i];

                Log($"Rect {i}");
                Log($"x: {rect.X}, y: {rect.Y}, width: {rect.Width}, height: {rect.Height}, " +
                    $"angle: {rect.Angle}");
            }

            Log("---------------------");
        }
    }
}
