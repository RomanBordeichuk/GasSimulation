using GasSimulation.Configuration;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.Simulation.InitStateTransformer.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Mappers
{
    public static class AtomMapper
    {
        public static AtomState MapToState(this AtomConfigInitState configInitState, Config config)
        {
            PosState newPos = MathHelper.TranslateField(
                configInitState.Pos, new(-config.Simulation.StartPosX, 
                -config.Simulation.StartPosY));

            var v = MathHelper.DecomposeVector(
                configInitState.Speed * config.Simulation.Mult, configInitState.Angle);

            return new(newPos, v);
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
