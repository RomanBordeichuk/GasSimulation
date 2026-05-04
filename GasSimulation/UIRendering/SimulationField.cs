using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;
using System.Windows;
using System.Windows.Media;

namespace GasSimulation.UIRendering
{
    public class SimulationField : FrameworkElement
    {
        private readonly DrawingVisual _visual = new DrawingVisual();

        public SimulationField()
        {
            AddVisualChild(_visual);
        }

        public void RenderFrame(Config config, AllStates allStates)
        {
            using (DrawingContext context = _visual.RenderOpen())
            {
                context.DrawRectangle(config.BlackBrush, null, new Rect(0, 0, ActualWidth, ActualHeight));

                var atomPoint = new Point();

                foreach (var atom in allStates.Atoms)
                {
                    atomPoint.X = atom.X;
                    atomPoint.Y = atom.Y;

                    context.DrawEllipse(config.ElemBrush, null, atomPoint,
                        config.AtomDiameter / 2, config.AtomDiameter / 2);
                }

                var rectPoint = new Rect();

                foreach (var rect in allStates.Rects)
                {
                    rectPoint.X = rect.X - rect.Width / 2;
                    rectPoint.Y = rect.Y - rect.Height / 2;
                    rectPoint.Width = rect.Width;
                    rectPoint.Height = rect.Height;

                    context.PushTransform(new RotateTransform(MathHelper.TransformAngleToDEG(rect.Angle),
                        rect.X, rect.Y));

                    context.DrawRectangle(config.ElemBrush, null, rectPoint);

                    context.Pop();
                }
            }
        }

        protected override int VisualChildrenCount => 1;
        protected override Visual GetVisualChild(int index) => _visual;
    }
}
