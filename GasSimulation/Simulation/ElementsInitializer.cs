using GasSimulation.Exceptions;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.DTOs.Interfaces;
using GasSimulation.Simulation.IterationCalculator.Helpers;
using GasSimulation.Simulation.UIElements;

namespace GasSimulation.Simulation
{
    static class ElementsInitializer
    {
        private static Config _config = null!;
        private static MainWindow _mainWindow = null!;
        private static AllElems _uiElems = new();
        private static List<AtomState> _atomStates = new();
        private static List<RectState> _rectStates = new();

        public static (AllElems uiElems, AllStates elemStates) 
            Initialize(Config config, List<AllConfigInitState> configInitStates, MainWindow mainWindow)
        {
            _config = config;
            _mainWindow = mainWindow;

            foreach (var configInitState in configInitStates)
            {
                InitializeGas(configInitState.Gas);
                InitializeAtoms(configInitState.Atoms);
                InitializeRect(configInitState.Rects);
            }

            return (_uiElems, new(_atomStates, _rectStates));
        }

        private static void InitializeGas(List<GasConfigInitState> rawGasList)
        {
            if (rawGasList != null)
            {
                foreach (var rawGas in rawGasList)
                {
                    var rawArea = rawGas.Area;
                    RectState area = new(rawArea[0], rawArea[1], rawArea[2], rawArea[3], rawArea[4]);

                    List<AtomInitState> atoms = GasGenerator.GasGenerator.Generate(
                        _config, area, rawGas.NumAtoms, rawGas.AtomSpeed);

                    foreach (var atom in atoms) AddAtom(atom);
                }
            }
        }

        private static void InitializeAtoms(List<List<double>> rawAtomsList)
        {
            if (rawAtomsList != null)
            {
                foreach (var rawAtom in rawAtomsList)
                {
                    AddAtom(new(rawAtom[0], rawAtom[1], rawAtom[2], rawAtom[3]));
                }
            }
        }

        private static void InitializeRect(List<List<double>> rawRectsList)
        {
            if (rawRectsList != null)
            {
                foreach (var rawRect in rawRectsList)
                {
                    AddRect(new(rawRect[0], rawRect[1], rawRect[2], rawRect[3], rawRect[4]));
                }
            }
        } 

        private static void AddAtom(AtomInitState atom)
        {
            (double x, double y) = TransformPos(atom.X, atom.Y);

            (double dx, double dy) = MathHelper.DecomposeVelocity(atom.Speed * _config.Mult, atom.Angle);

            AtomState atomState = new(x, y, dx, dy);
            Atom uiAtom = new(_config);

            uiAtom.UpdatePos(new(x, y));

            _atomStates.Add(atomState);
            _uiElems.Atoms.Add(uiAtom);

            AddUIElem(uiAtom);
        }

        private static void AddRect(RectState rect)
        {
            (double x, double y) = TransformPos(rect.X, rect.Y);
            double angleRad = TransformAngle(rect.Angle);

            RectState rectState = new(x, y, rect.Width, rect.Height, angleRad);
            Rect uiRect = new(_config, rect.Width, rect.Height, rect.Angle);

            uiRect.UpdatePos(new(x, y));

            _rectStates.Add(rectState);
            _uiElems.Rects.Add(uiRect);

            AddUIElem(uiRect);
        }

        private static void AddUIElem(Element uiElem)
        {
            _mainWindow.SimulationField.Children.Add(uiElem.Obj);
        }

        private static (double x, double y) TransformPos(double x, double y)
        {
            return new(x + _config.StartPosX, y + _config.StartPosY);
        }

        private static double TransformAngle(double angle)
        {
            return angle * Math.PI / 180;
        }
    }
}
