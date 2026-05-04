using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.Simulation.InitStateTransformer.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.Mappers
{
    public static class AtomMapper
    {
        public static AtomState MapToState(this AtomConfigInitState configInitState, Config config)
        {
            PosState newPos = MathHelper.TranslateField(
                configInitState.Pos, new(-config.StartPosX, -config.StartPosY));

            (double dx, double dy) = MathHelper.DecomposeVelocity(
                configInitState.Speed * config.Mult, configInitState.Angle);

            return new(newPos, new(dx, dy));
        }

        public static List<AtomState> MapToStates(this List<AtomConfigInitState> initStates, Config config)
        {
            List<AtomState> atomStates = new();

            foreach (var initState in initStates)
            {
                atomStates.Add(initState.MapToState(config));
            }

            return atomStates;
        }
    }
}
