using GasSimulation.Configuration;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Mappers;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.InitStateTransformer.DTOs;

namespace GasSimulation.Simulation.InitStateTransformer
{
    public class ConfigInitStateTransformer
    {
        private readonly Config _config;
        private readonly GasGenerator.GasGenerator _gasGenerator;
        private readonly SectorTransformer _sectorTransformer;

        public ConfigInitStateTransformer(Config config, GasGenerator.GasGenerator gasGenerator, 
            SectorTransformer sectorTransformer)
        {
            _config = config;
            _gasGenerator = gasGenerator;
            _sectorTransformer = sectorTransformer;
        }

        public async ValueTask<SectorStates> Transform(List<ConfigInitState> configInitStates)
        {
            List<AtomState> atoms = new();
            List<RectState> rects = new();

            foreach (var configInitState in configInitStates)
            {
                var atomInitStates = await InitializeGas(configInitState.Gas);
                atomInitStates.AddRange(InitializeAtoms(configInitState.Atoms));

                var rectInitStates = InitializeRects(configInitState.Rects);

                atoms.AddRange(atomInitStates.MapToStates(_config));
                rects.AddRange(rectInitStates.MapToStates(_config));
            }

            var allStates = new AllStates(atoms, rects);

            return await _sectorTransformer.Transform(allStates);
        }

        private async ValueTask<List<AtomConfigInitState>> InitializeGas(
            List<GasConfigInitState> rawGasList)
        {
            List<AtomConfigInitState> atomInitStates = new();

            if (rawGasList != null)
            {
                foreach (var rawGas in rawGasList)
                {
                    var rawArea = rawGas.Area;
                    RectState area = new(rawArea[0], rawArea[1], rawArea[2], rawArea[3], rawArea[4]);

                    atomInitStates.AddRange(await _gasGenerator.Generate(
                        area, rawGas.NumAtoms, rawGas.AtomSpeed));
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
