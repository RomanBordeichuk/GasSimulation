using GasSimulation.Debuggers.DTOs.Interfaces;

namespace GasSimulation.Debuggers.DTOs
{
    public struct ClearGroupCommand : IDebugCommand
    {
        public string GroupName { get; }

        public ClearGroupCommand(string name)
        {
            GroupName = name;
        }
    }
}
