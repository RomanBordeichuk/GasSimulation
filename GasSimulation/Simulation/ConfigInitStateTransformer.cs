using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.DTOs.Config;
using GasSimulation.Simulation.Mappers;

namespace GasSimulation.Simulation
{
    public static class ConfigInitStateTransformer
    {
        public static AllStates Transform(Config config, List<ConfigInitState> configInitStates)
        {
            List<AtomState> atoms = new();
            List<RectState> rects = new();

            foreach (var configInitState in configInitStates)
            {
                var atomInitStates = InitializeGas(config, configInitState.Gas);
                atomInitStates.AddRange(InitializeAtoms(configInitState.Atoms));

                var rectInitStates = InitializeRects(configInitState.Rects);

                atoms.AddRange(atomInitStates.MapToStates(config));
                rects.AddRange(rectInitStates.MapToStates(config));
            }

            return new(atoms, rects);
        }

        private static List<AtomConfigInitState> InitializeGas(Config config, List<GasConfigInitState> rawGasList)
        {
            List<AtomConfigInitState> atomInitStates = new();

            if (rawGasList != null)
            {
                foreach (var rawGas in rawGasList)
                {
                    var rawArea = rawGas.Area;
                    RectState area = new(rawArea[0], rawArea[1], rawArea[2], rawArea[3], rawArea[4]);

                    atomInitStates.AddRange(GasGenerator.GasGenerator.Generate(
                        config, area, rawGas.NumAtoms, rawGas.AtomSpeed));
                }
            }

            return atomInitStates;
        }

        private static List<AtomConfigInitState> InitializeAtoms(List<List<double>> rawAtomsList)
        {
            List<AtomConfigInitState> atomInitStates = new();

            if (rawAtomsList != null)
            {
                foreach (var rawAtom in rawAtomsList)
                {
                    atomInitStates.Add(new(rawAtom[0], rawAtom[1], rawAtom[2], rawAtom[3]));
                }
            }

            return atomInitStates;
        }

        private static List<RectState> InitializeRects(List<List<double>> rawRectsList)
        {
            List<RectState> rectInitStates = new();

            if (rawRectsList != null)
            {
                foreach (var rawRect in rawRectsList)
                {
                    rectInitStates.Add(new(rawRect[0], rawRect[1], rawRect[2], rawRect[3], rawRect[4]));
                }
            }

            return rectInitStates;
        }
    }
}
