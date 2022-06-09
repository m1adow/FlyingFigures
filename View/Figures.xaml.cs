using Figures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
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
        private Random _random;

        public Figures()
        {
            InitializeComponent();

            _figures = new Dictionary<Figure, List<UIElement>>();
            _random = new Random();

            Thread movement = new(new ThreadStart(() =>
            {
                InitializeTimer();
                Dispatcher.Run();
            }));
            movement.Start();
        }

        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            var rectangle = GetRectangle(GetMaxCoordinates(figuresCanvas));

            MoveFigure(rectangle.Item1, rectangle.Item2.X, rectangle.Item2.Y);

            foreach (var line in rectangle.Item1)
                figuresCanvas.Children.Add(line);

            figuresTreeView.Items.Add(rectangle.Item2);

            _figures.Add(rectangle.Item2, rectangle.Item1);
        }

        private void TriangleButton_Click(object sender, RoutedEventArgs e)
        {
            var triangle = GetTriangle(GetMaxCoordinates(figuresCanvas));

            MoveFigure(triangle.Item1, triangle.Item2.X, triangle.Item2.Y);

            foreach (var line in triangle.Item1)
                figuresCanvas.Children.Add(line);

            figuresTreeView.Items.Add(triangle.Item2);

            _figures.Add(triangle.Item2, triangle.Item1);
        }

        private void CirlceButton_Click(object sender, RoutedEventArgs e)
        {
            var circle = GetCircle(GetMaxCoordinates(figuresCanvas));

            MoveFigure(circle.Item1, circle.Item2.X, circle.Item2.Y);

            foreach (var drawingCircle in circle.Item1)
                figuresCanvas.Children.Add(drawingCircle);

            figuresTreeView.Items.Add(circle.Item2);

            _figures.Add(circle.Item2, circle.Item1);
        }

        private Point GetMaxCoordinates(Canvas canvas)
        {
            Point point = new()
            {
                X = canvas.ActualWidth,
                Y = canvas.ActualHeight
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
            foreach (var figure in _figures)
            {
                figure.Key.Move(GetMaxCoordinates(figuresCanvas));
                MoveFigure(figure.Key.Draw(figure.Value), figure.Key.X, figure.Key.Y);
            }
        }

        private void MoveFigure(List<UIElement> sides, int x, int y)
        {
            foreach (var line in sides)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate ()
                    {
                        Canvas.SetLeft(line, x);
                        Canvas.SetTop(line, y);
                    });
            }
        }

        private Tuple<List<UIElement>, Rectangle> GetRectangle(Point maxPoint)
        {
            int length = _random.Next(50, 100);

            int x = _random.Next(length, Convert.ToInt32(maxPoint.X) - length);
            int y = _random.Next(length, Convert.ToInt32(maxPoint.Y) - length);

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

        private Tuple<List<UIElement>, Triangle> GetTriangle(Point maxPoint)
        {
            int length = _random.Next(50, 100);

            int x = _random.Next(length, Convert.ToInt32(maxPoint.X) - length);
            int y = _random.Next(length, Convert.ToInt32(maxPoint.Y) - length);

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

        private Tuple<List<UIElement>, Circle> GetCircle(Point maxPoint)
        {
            int radius = _random.Next(50, 100);

            int x = _random.Next(radius, Convert.ToInt32(maxPoint.X) - radius);
            int y = _random.Next(radius, Convert.ToInt32(maxPoint.Y) - radius);

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
