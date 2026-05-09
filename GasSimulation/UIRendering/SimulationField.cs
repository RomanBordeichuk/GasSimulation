using GasSimulation.Configuration;
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

        public void RenderFrame(Config config, SectorStates sectorStates)
        {
            using (DrawingContext context = _visual.RenderOpen())
            {
                context.DrawRectangle(config.Brushes!.Black, null, new Rect(0, 0, ActualWidth, ActualHeight));

                var atomPoint = new Point();
                var rectPoint = new Rect();

                foreach (var allStates in sectorStates.Sectors.Select(s => s.AllStates))
                {
                    foreach (var atom in allStates.Atoms)
                    {
                        atomPoint.X = atom.X;
                        atomPoint.Y = atom.Y;

                        context.DrawEllipse(config.Brushes.Elem, null, atomPoint,
                            config.Simulation.AtomDiameter / 2, config.Simulation.AtomDiameter / 2);
                    }

                    foreach (var rect in allStates.Rects)
                    {
                        rectPoint.X = rect.X - rect.Width / 2;
                        rectPoint.Y = rect.Y - rect.Height / 2;
                        rectPoint.Width = rect.Width;
                        rectPoint.Height = rect.Height;

                        context.PushTransform(new RotateTransform(MathHelper.TransformAngleToDEG(rect.Angle),
                            rect.X, rect.Y));

                        context.DrawRectangle(config.Brushes.Elem, null, rectPoint);

                        context.Pop();
                    }
                }
            }
        }

        protected override int VisualChildrenCount => 1;
        protected override Visual GetVisualChild(int index) => _visual;
    }
}
