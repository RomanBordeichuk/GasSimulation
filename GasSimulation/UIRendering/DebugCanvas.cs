using GasSimulation.Configuration;
using GasSimulation.Debuggers.DTOs;
using GasSimulation.Debuggers.DTOs.Interfaces;
using GasSimulation.Exceptions;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Interfaces;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.IterationCalculator.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GasSimulation.UIRendering
{
    public class DebugCanvas
    {
        private readonly Config _config;
        private readonly Canvas _canvas;

        private readonly List<(IElemState elemState, Shape elem)> _elems = new();

        public DebugCanvas(Config config, Canvas canvas)
        {
            _config = config;
            _canvas = canvas;
        }

        public void ClearAll()
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                _elems.Clear();
                _canvas.Children.Clear();
            });
        }

        public void Clear(List<IDrawCommand> group)
        {
            foreach (var cmd in group) Clear(cmd);
        }

        public void Clear(IDrawCommand command)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                var elem = _elems.First(e => e.elemState.Equals(command.Elem));

                _elems.Remove(elem);
                _canvas.Children.Remove(elem.elem);
            });
        }

        public void Draw(DrawCommand command)
        {
            if (command.Elem is AtomState atomState)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    var atom = new Ellipse();

                    _elems.Add((atomState, atom));

                    atom.Width = _config.Simulation.AtomDiameter;
                    atom.Height = _config.Simulation.AtomDiameter;
                    atom.Fill = command.Brush;

                    if (command.Zindex != null) Panel.SetZIndex(atom, command.Zindex.Value);

                    Canvas.SetLeft(atom, atomState.X - _config.Simulation.AtomDiameter / 2);
                    Canvas.SetTop(atom, atomState.Y - _config.Simulation.AtomDiameter / 2);

                    _canvas.Children.Add(atom);
                });
            }
            else if (command.Elem is RectState rectState)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    var rect = new Rectangle();

                    _elems.Add((rectState, rect));

                    rect.Width = rectState.Width;
                    rect.Height = rectState.Height;

                    rect.RenderTransformOrigin = new Point(0.5, 0.5);
                    rect.RenderTransform = new RotateTransform(
                        MathHelper.TransformAngleToDEG(rectState.Angle));

                    rect.Fill = command.Brush;

                    if (command.Zindex != null) Panel.SetZIndex(rect, command.Zindex.Value);

                    Canvas.SetLeft(rect, rectState.X - rect.Width / 2);
                    Canvas.SetTop(rect, rectState.Y - rect.Height / 2);

                    _canvas.Children.Add(rect);
                });
            }
            else if (command.Elem is VectorState vectorState)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    var vector = new Line();

                    _elems.Add((vectorState, vector));

                    vector.X1 = vectorState.X1;
                    vector.Y1 = vectorState.Y1;
                    vector.X2 = vectorState.X2;
                    vector.Y2 = vectorState.Y2;
                    vector.Stroke = command.Brush;
                    vector.StrokeThickness = vectorState.Thickness;

                    if (command.Zindex != null) Panel.SetZIndex(vector, command.Zindex.Value);

                    _canvas.Children.Add(vector);
                });
            }
            else throw new IncorrectTypeException();
        }

        public void DrawHollow(DrawHollowCommand command)
        {
            if (command.Elem is AtomState atomState)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    var atom = new Ellipse();

                    _elems.Add((atomState, atom));

                    atom.Width = _config.Simulation.AtomDiameter + command.Border * 2;
                    atom.Height = _config.Simulation.AtomDiameter + command.Border * 2;
                    atom.Stroke = command.Brush;
                    atom.StrokeThickness = command.Border;

                    if (command.Zindex != null) Panel.SetZIndex(atom, command.Zindex.Value);

                    Canvas.SetLeft(atom, atomState.X - (_config.Simulation.AtomDiameter + command.Border));
                    Canvas.SetTop(atom, atomState.Y - (_config.Simulation.AtomDiameter + command.Border));

                    _canvas.Children.Add(atom);
                });
            }
            else if (command.Elem is RectState rectState)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    var rect = new Rectangle();

                    _elems.Add((rectState, rect));

                    rect.Width = rectState.Width + command.Border;
                    rect.Height = rectState.Height + command.Border;

                    rect.RenderTransformOrigin = new Point(0.5, 0.5);
                    rect.RenderTransform = new RotateTransform(MathHelper.TransformAngleToDEG(rectState.Angle));

                    rect.Stroke = command.Brush;
                    rect.StrokeThickness = command.Border;

                    if (command.Zindex != null) Panel.SetZIndex(rect, command.Zindex.Value);

                    Canvas.SetLeft(rect, rectState.X - (rect.Width + command.Border) / 2);
                    Canvas.SetTop(rect, rectState.Y - (rect.Height + command.Border) / 2);

                    _canvas.Children.Add(rect);
                });
            }
            else if (command.Elem is VectorState)
            {
                throw new NotImplementedException();
            }
            else throw new IncorrectTypeException();
        }
    }
}
