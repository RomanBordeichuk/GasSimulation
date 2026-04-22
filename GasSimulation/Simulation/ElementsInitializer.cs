using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;
using GasSimulation.Simulation.UIElements;

namespace GasSimulation.Simulation
{
    static class ElementsInitializer
    {
        private static readonly double _mult = Constants.SpeedMult / Constants.FPS;

        private static MainWindow _mainWindow = null!;
        private static AllElems _uiElems = new();
        private static List<AtomState> _atomStates = new();
        private static List<RectState> _rectStates = new();

        public static (AllElems uiElems, AllStates elemStates) 
            Initialize(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            //AddAtom(300, 100, 95, 135);
            //AddAtom(100, 100, 105, 45);
            //AddAtom(200, 100, 100, 0);
            //AddAtom(120, 150, 100, -45);
            //AddAtom(200, 600, 130, -90);
            //AddAtom(600, 300, 80, -155);
            //AddAtom(500, 200, 100, 180);
            //AddAtom(440, 550, 120, -120);

            //AddRect(350, 300, 100, 20, 30);



            AddAtom(40, 52.2, 70, 0);
            AddAtom(70, 72, 70, 0);
            AddAtom(100, 80, 70, 0);
            AddAtom(100, 60, 70, 0);
            AddAtom(100, 120, 70, 0);
            AddAtom(100, 140, 70, 0);

            AddRect(300, 100, 150, 15, 30);
            AddRect(300, 150, 150, 15, 90);



            //AddAtom(100, 100, 100, 74);
            //AddAtom(104, 200, 90, -90);

            return (_uiElems, new(_atomStates, _rectStates));
        }

        private static void AddAtom(double x, double y, double speed, double angle)
        {
            (x, y) = TransformPos(x, y);

            (double dx, double dy) = MathHelper.DecomposeVelocity(speed * _mult, angle);

            AtomState atomState = new(x, y, dx, dy);
            Atom uiAtom = new();

            uiAtom.UpdatePos(new(x, y));

            _atomStates.Add(atomState);
            _uiElems.Atoms.Add(uiAtom);

            AddUIElem(uiAtom);
        }

        private static void AddRect(double x, double y, double width, double height, double angle)
        {
            (x, y) = TransformPos(x, y);
            double angleRad = TransformAngle(angle);

            RectState rectState = new(x, y, width, height, angleRad);
            Rect uiRect = new(width, height, angle);

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
            return new(x + Constants.StartPosX, y + Constants.StartPosY);
        }

        private static double TransformAngle(double angle)
        {
            return angle * Math.PI / 180;
        }
    }
}
