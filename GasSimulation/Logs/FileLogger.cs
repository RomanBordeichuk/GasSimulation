using GasSimulation.Simulation.DTOs;
using System.IO;
using System.Text;

namespace GasSimulation.Logs
{
    public sealed class FileLogger : IDisposable
    {
        private readonly StreamWriter? _streamWriter;

        public FileLogger(string? filePath)
        {
            if (filePath != null)
            {
                var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write,
                    FileShare.Read, bufferSize: 1024 * 1024);

                _streamWriter = new StreamWriter(fileStream, new UTF8Encoding(false));
            }
        }

        public void LogAllStates(AllStates allStates, int iteration)
        {
            if (_streamWriter == null) return;

            _streamWriter.WriteLine($"iteration: {iteration}");
            _streamWriter.WriteLine("x,y,dx,dy");

            foreach (var atom in allStates.Atoms)
            {
                _streamWriter.WriteLine($"{atom.X},{atom.Y},{atom.Dx},{atom.Dy}");
            }

            _streamWriter.WriteLine("x,y,w,h");

            foreach (var rect in allStates.Rects)
            {
                _streamWriter.WriteLine($"{rect.X},{rect.Y},{rect.Width},{rect.Height}");
            }

            _streamWriter.Flush();
        }

        public void Dispose()
        {
            _streamWriter?.Dispose();
        }
    }
}
