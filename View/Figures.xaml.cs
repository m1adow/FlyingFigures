using Figures;
using System;
using System.Collections.Generic;
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
            InitializeTimer();

            _figures = new Dictionary<Figure, List<UIElement>>();
            _random = new Random();
        }

        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            Point maxPoint = FindMaxCoordinates();

            int x = _random.Next(0, Convert.ToInt32(maxPoint.X));
            int y = _random.Next(0, Convert.ToInt32(maxPoint.Y));

            var drawingRectangle = GetRectangle();
            Rectangle rectangle = new(x, y, drawingRectangle.Item2);

            foreach (var line in drawingRectangle.Item1)
                figuresCanvas.Children.Add(line);

            figuresTreeView.Items.Add(rectangle);

            _figures.Add(rectangle, drawingRectangle.Item1);
        }

        private void TriangleButton_Click(object sender, RoutedEventArgs e)
        {
            Point maxPoint = FindMaxCoordinates();

            int x = _random.Next(0, Convert.ToInt32(maxPoint.X));
            int y = _random.Next(0, Convert.ToInt32(maxPoint.Y));

            Triangle triangle = new(x, y);
            List<UIElement> drawingTriangle = GetTriangle();

            foreach (var line in drawingTriangle)
                figuresCanvas.Children.Add(line);

            figuresTreeView.Items.Add(triangle);

            _figures.Add(triangle, drawingTriangle);
        }

        private void CirlceButton_Click(object sender, RoutedEventArgs e)
        {
            Point maxPoint = FindMaxCoordinates();

            int x = _random.Next(0, Convert.ToInt32(maxPoint.X));
            int y = _random.Next(0, Convert.ToInt32(maxPoint.Y));

            Circle circle = new(x, y);
            List<UIElement> drawingCircles = GetCircle();

            foreach (var drawingCircle in drawingCircles)
                figuresCanvas.Children.Add(drawingCircle);

            figuresTreeView.Items.Add(circle);

            _figures.Add(circle, drawingCircles);
        }

        private Point FindMaxCoordinates()
        {
            Point point = figuresCanvas.PointToScreen(new Point());

            //Point newP = (Point)(new Point(Application.Current.MainWindow.Width, Application.Current.MainWindow.Height) - point);

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
            Point maxPoint = FindMaxCoordinates();

            foreach (var figure in _figures)
            {
                figure.Key.Move(maxPoint);
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

        private Tuple<List<UIElement>, int> GetRectangle()
        {
            int length = _random.Next(50, 100);

            List<UIElement> rectangle = new()
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

            return new Tuple<List<UIElement>, int>(rectangle, length);
        }

        private List<UIElement> GetTriangle()
        {
            int length = _random.Next(50, 100);

            List<UIElement> triangle = new()
            {
                new Line()
                {
                    X2 = -1 * length / 2,
                    Stroke = GetRandomColor()
                },
                new Line()
                {
                    Y2 = -1 * length,
                    Stroke = GetRandomColor()
                },
                new Line()
                {
                    Y1 = -1 * length,
                    X2 = -1 * length / 2,
                    Stroke = GetRandomColor()
                }
            };

            return triangle;
        }

        private List<UIElement> GetCircle()
        {
            int radius = _random.Next(50, 100);

            List<UIElement> circle = new()
            {
                new System.Windows.Shapes.Ellipse()
                {
                    Width = radius,
                    Height = radius,
                    Stroke = GetRandomColor()
                }
            };

            return circle;
        }

        private SolidColorBrush GetRandomColor()
        {
            return new SolidColorBrush(Color.FromRgb((byte)_random.Next(1, 255), (byte)_random.Next(1, 255), (byte)_random.Next(1, 255)));
        }
    }
}
