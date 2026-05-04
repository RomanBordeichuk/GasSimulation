using GasSimulation.Debuggers.DTOs;
using GasSimulation.Exceptions;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Interfaces;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Simulation.IterationCalculator.Helpers;
using GasSimulation.UIRendering.DTOs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GasSimulation.UIRendering
{
    public static class DebugCanvas
    {
        private static Config _config = null!;
        private static Canvas _canvas = null!;
        private static List<ElemGroup> _groups = new();

        public static void Initialize(Config config, Canvas canvas)
        {
            _config = config;
            _canvas = canvas;
        }

        public static void ClearGroup(string name)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                if (!CheckGroup(name)) throw new IncorrectGroupException();

                var group = _groups.First(g => g.Name == name);

                foreach (var elem in group.Elems)
                {
                    _canvas.Children.Remove(elem);
                }

                _groups.Remove(group);
            });
        }

        public static void ClearAll()
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                foreach (var group in _groups)
                {
                    foreach (var elem in group.Elems)
                    {
                        _canvas.Children.Remove(elem);
                    }
                }
            });

            _groups.Clear();
        }

        public static void Draw<T>(string groupName, T elemState, SolidColorBrush brush)
            where T : IElemState
        {
            if (elemState is AtomState atomState)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    var atom = new Ellipse();

                    AddToGroup(groupName, atom);

                    atom.Width = _config.AtomDiameter;
                    atom.Height = _config.AtomDiameter;
                    atom.Fill = brush;

                    Canvas.SetLeft(atom, atomState.X - _config.AtomDiameter / 2);
                    Canvas.SetTop(atom, atomState.Y - _config.AtomDiameter / 2);

                    _canvas.Children.Add(atom);
                });
            }
            else if (elemState is RectState rectState)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    var rect = new Rectangle();

                    AddToGroup(groupName, rect);

                    rect.Width = rectState.Width;
                    rect.Height = rectState.Height;

                    rect.RenderTransformOrigin = new Point(0.5, 0.5);
                    rect.RenderTransform = new RotateTransform(MathHelper.TransformAngleToDEG(rectState.Angle));

                    rect.Fill = brush;

                    Canvas.SetLeft(rect, rectState.X - rect.Width / 2);
                    Canvas.SetTop(rect, rectState.Y - rect.Height / 2);

                    _canvas.Children.Add(rect);
                });
            }
            else if (elemState is VectorState)
            {
                throw new NotImplementedException();
            }
            else throw new IncorrectTypeException();
        }

        public static void DrawHollow<T>(string groupName, T elemState, double border, SolidColorBrush brush)
    where T : IElemState
        {
            if (elemState is AtomState atomState)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    var atom = new Ellipse();

                    AddToGroup(groupName, atom);

                    atom.Width = _config.AtomDiameter;
                    atom.Height = _config.AtomDiameter;
                    atom.Stroke = brush;

                    Canvas.SetLeft(atom, atomState.X - _config.AtomDiameter / 2);
                    Canvas.SetTop(atom, atomState.Y - _config.AtomDiameter / 2);

                    _canvas.Children.Add(atom);
                });
            }
            else if (elemState is RectState rectState)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    var rect = new Rectangle();

                    AddToGroup(groupName, rect);

                    rect.Width = rectState.Width;
                    rect.Height = rectState.Height;

                    rect.RenderTransformOrigin = new Point(0.5, 0.5);
                    rect.RenderTransform = new RotateTransform(MathHelper.TransformAngleToDEG(rectState.Angle));

                    rect.Stroke = brush;

                    Canvas.SetLeft(rect, rectState.X - rect.Width / 2);
                    Canvas.SetTop(rect, rectState.Y - rect.Height / 2);

                    _canvas.Children.Add(rect);
                });
            }
            else if (elemState is VectorState)
            {
                throw new NotImplementedException();
            }
            else throw new IncorrectTypeException();
        }

        private static void AddToGroup<T>(string groupName, T elem)
            where T : Shape
        {
            var group = GetGroup(groupName);
            group.Elems.Add(elem);
        }

        private static bool CheckGroup(string name)
        {
            return _groups.Any(g => g.Name == name);
        }

        private static ElemGroup GetGroup(string name)
        {
            if (!CheckGroup(name)) _groups.Add(new(name));

            return _groups.FirstOrDefault(g => g.Name == name);
        }
    }
}
