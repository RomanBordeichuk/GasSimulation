using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Interfaces;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.InitStateTransformer.DTOs;
using System.Diagnostics;

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
