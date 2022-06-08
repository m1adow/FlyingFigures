using Figures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace FlyingFigures.View
{
    /// <summary>
    /// Interaction logic for Figures.xaml
    /// </summary>
    public partial class Figures : Window
    {
        private Dictionary<Figure, UIElement> _figures;
        private Random _random;

        public Figures()
        {
            InitializeComponent();
            InitializeTimer();

            _figures = new Dictionary<Figure, UIElement>();
            _random = new Random();
        }

        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            Point maxPoint = FindMaxCoordinates();

            int x = _random.Next(0, Convert.ToInt32(maxPoint.X));
            int y = _random.Next(0, Convert.ToInt32(maxPoint.Y));

            Rectangle rectangle = new(x, y);
            System.Windows.Shapes.Rectangle drawingRectangle = GetRectangle();

            Canvas.SetLeft(drawingRectangle, x);
            Canvas.SetTop(drawingRectangle, y);

            figuresCanvas.Children.Add(drawingRectangle);
            figuresTreeView.Items.Add(rectangle);

            _figures.Add(rectangle, drawingRectangle);
        }

        private void TriangleButton_Click(object sender, RoutedEventArgs e)
        {
            Point maxPoint = FindMaxCoordinates();

            int x = _random.Next(0, Convert.ToInt32(maxPoint.X));
            int y = _random.Next(0, Convert.ToInt32(maxPoint.Y));

            Triangle triangle = new(x, y);
            System.Windows.Shapes.Polygon drawingTriangle = GetTriangle();

            Canvas.SetLeft(drawingTriangle, x);
            Canvas.SetTop(drawingTriangle, y);

            figuresCanvas.Children.Add(drawingTriangle);
            figuresTreeView.Items.Add(triangle);

            _figures.Add(triangle, drawingTriangle);
        }

        private void CirlceButton_Click(object sender, RoutedEventArgs e)
        {
            Point maxPoint = FindMaxCoordinates();

            int x = _random.Next(0, Convert.ToInt32(maxPoint.X));
            int y = _random.Next(0, Convert.ToInt32(maxPoint.Y));

            Circle circle = new(x, y);
            System.Windows.Shapes.Ellipse drawingCircle = GetCircle();

            figuresCanvas.Children.Add(drawingCircle);
            figuresTreeView.Items.Add(circle);

            _figures.Add(circle, drawingCircle);
        }

        private Point FindMaxCoordinates()
        {
            Point point = figuresCanvas.PointToScreen(new Point());

            Point newP = (Point)(new Point(Application.Current.MainWindow.Width, Application.Current.MainWindow.Height) - point);

            return newP;
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

        private void MoveFigure(UIElement uIElement, int x, int y)
        {
            Canvas.SetLeft(uIElement, x);
            Canvas.SetTop(uIElement, y);
        }

        private System.Windows.Shapes.Rectangle GetRectangle()
        {
            int length = _random.Next(50, 100);

            System.Windows.Shapes.Rectangle rectangle = new()
            {
                Height = length / 2,
                Width = length,
                Fill = GetRandomColor(),
                StrokeThickness = 3,
                Stroke = Brushes.Black
            };

            return rectangle;
        } 
        
        private System.Windows.Shapes.Polygon GetTriangle()
        {
            int length = _random.Next(50, 100);

            System.Windows.Shapes.Polygon triangle = new()
            {
                Fill = GetRandomColor(),
                StrokeThickness = 3,
                Stroke = Brushes.Black
            };

            triangle.Points = new PointCollection()
            {
                new Point(0, 0),
                new Point(0, length),
                new Point(length, length)
            };

            return triangle;
        } 
        
        private System.Windows.Shapes.Ellipse GetCircle()
        {
            int radius = _random.Next(50, 100);

            System.Windows.Shapes.Ellipse circle = new()
            {
                Height = radius,
                Width = radius,
                Fill = GetRandomColor(),
                StrokeThickness = 3,
                Stroke = Brushes.Black
            };

            return circle;
        } 

        private SolidColorBrush GetRandomColor()
        {
            return new SolidColorBrush(Color.FromRgb((byte)_random.Next(1, 255), (byte)_random.Next(1, 255), (byte)_random.Next(1, 255)));
        }
    }
}
