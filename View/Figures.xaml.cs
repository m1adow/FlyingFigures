using Figures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Rectangle = Figures.Rectangle;

namespace FlyingFigures.View
{
    /// <summary>
    /// Interaction logic for Figures.xaml
    /// </summary>
    public partial class Figures : Window
    {
        private Dictionary<Figure, List<UIElement>> _figures;
        private Point _maxCoordinates;
        private Random _random;

        public Figures()
        {
            InitializeComponent();

            _figures = new Dictionary<Figure, List<UIElement>>();
            _random = new Random();

            InitializeTimer();
        }

        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            var rectangle = GetRectangle();

            foreach (var line in rectangle.Item1)
                figuresCanvas.Children.Add(line);

            figuresTreeView.Items.Add(rectangle.Item2);

            _figures.Add(rectangle.Item2, rectangle.Item1);
        }

        private void TriangleButton_Click(object sender, RoutedEventArgs e)
        {
            var triangle = GetTriangle();

            foreach (var line in triangle.Item1)
                figuresCanvas.Children.Add(line);

            figuresTreeView.Items.Add(triangle.Item2);

            _figures.Add(triangle.Item2, triangle.Item1);
        }

        private void CirlceButton_Click(object sender, RoutedEventArgs e)
        {
            var circle = GetCircle();

            foreach (var drawingCircle in circle.Item1)
                figuresCanvas.Children.Add(drawingCircle);

            figuresTreeView.Items.Add(circle.Item2);

            _figures.Add(circle.Item2, circle.Item1);
        }

        private Point FindMaxCoordinates()
        {
            Point point = new()
            {
                X = figuresCanvas.ActualWidth,
                Y = figuresCanvas.ActualHeight
            };

            return point;
        }

        private void InitializeTimer()
        {
            DispatcherTimer timer = new(); 

            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _maxCoordinates = FindMaxCoordinates();

            foreach (var figure in _figures)
            {
                figure.Key.Move(_maxCoordinates);
                MoveFigure(figure.Key.Draw(figure.Value), figure.Key.X, figure.Key.Y);
            }
        }

        private void MoveFigure(List<UIElement> sides, int x, int y)
        {
            foreach (var line in sides)
            {
                Canvas.SetLeft(line, x);
                Canvas.SetTop(line, y);
            }
        }

        private Tuple<List<UIElement>, Rectangle> GetRectangle()
        {
            int length = _random.Next(50, 100);

            int x = _random.Next(length, Convert.ToInt32(_maxCoordinates.X) - length);
            int y = _random.Next(length, Convert.ToInt32(_maxCoordinates.Y) - length);

            List<UIElement> drawingRectangle = new()
            {
                new Line()
                {
                    X2 = length,
                    Stroke = GetRandomColor()
                },
                new Line()
                {
                    X1 = 0,
                    Y2 = length / 2,
                    Stroke = GetRandomColor()
                },
                new Line()
                {
                    X2 = length,
                    Y1 = length / 2,
                    Y2 = length / 2,
                    Stroke = GetRandomColor()
                },
                new Line()
                {
                    X1 = length,
                    X2 = length,
                    Y2 = length / 2,
                    Stroke = GetRandomColor()
                }
            };

            Rectangle rectangle = new(x, y, length, length / 2);

            return new Tuple<List<UIElement>, Rectangle>(drawingRectangle, rectangle);
        }

        private Tuple<List<UIElement>, Triangle> GetTriangle()
        {
            int length = _random.Next(50, 100);

            int x = _random.Next(length, Convert.ToInt32(_maxCoordinates.X) - length);
            int y = _random.Next(length, Convert.ToInt32(_maxCoordinates.Y) - length);

            List<UIElement> drawingTriangle = new()
            {
                new Line()
                {
                    X2 = length / 2,
                    Stroke = GetRandomColor()
                },
                new Line()
                {
                    Y2 = length,
                    Stroke = GetRandomColor()
                },
                new Line()
                {
                    Y1 = length,
                    X2 = length / 2,
                    Stroke = GetRandomColor()
                }
            };

            Triangle triangle = new(x, y, length / 2, length);

            return new Tuple<List<UIElement>, Triangle>(drawingTriangle, triangle);
        }

        private Tuple<List<UIElement>, Circle> GetCircle()
        {
            int radius = _random.Next(50, 100);

            int x = _random.Next(radius, Convert.ToInt32(_maxCoordinates.X) - radius);
            int y = _random.Next(radius, Convert.ToInt32(_maxCoordinates.Y) - radius);

            List<UIElement> drawingCircle = new()
            {
                new System.Windows.Shapes.Ellipse()
                {
                    Width = radius,
                    Height = radius,
                    Stroke = GetRandomColor()
                }
            };

            Circle circle = new(x, y, radius);

            return new Tuple<List<UIElement>, Circle>(drawingCircle, circle);
        }

        private SolidColorBrush GetRandomColor()
        {
            return new SolidColorBrush(Color.FromRgb((byte)_random.Next(1, 255), (byte)_random.Next(1, 255), (byte)_random.Next(1, 255)));
        }
    }
}
