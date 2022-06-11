using Figures;
using FlyingFigures.Localization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Rectangle = Figures.Rectangle;

namespace FlyingFigures.View
{
    /// <summary>
    /// Interaction logic for Figures.xaml
    /// </summary>
    public partial class Figures : Window
    {
        private List<Figure> _figures;
        private Random _random;

        public Figures()
        {
            InitializeComponent();

            _figures = new List<Figure>();
            _random = new Random();

            Thread movement = new(new ThreadStart(() =>
            {
                InitializeTimer();
                Dispatcher.Run();
            }));
            movement.Start();
        }

        private void CreateFigure_Click(object sender, RoutedEventArgs e)
        {
            string? name = (sender as Button)?.Content.ToString();

            if (name == Resource_en.RectangleButton)
                AddFigure(GetRectangle());
            else if (name == Resource_en.TriangleButton)
                AddFigure(GetTriangle());
            else if (name == Resource_en.CircleButton)
                AddFigure(GetCircle());
        }

        private void AddFigure(Figure figure)
        {
            MoveFigure(figure.Draw(), figure.X, figure.Y);

            foreach (var line in figure.Draw())
                figuresCanvas.Children.Add(line);

            figuresTreeView.Items.Add(figure);

            _figures.Add(figure);
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
                figure.Move(GetMaxCoordinates(figuresCanvas));
                MoveFigure(figure.Draw(), figure.X, figure.Y);
            }
        }

        private void MoveFigure(List<UIElement> sides, double x, double y)
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

        private Rectangle GetRectangle()
        {
            Point figureCoordinates = GetFigureCoordinates(GetMaxCoordinates(figuresCanvas));

            Rectangle rectangle = new(figureCoordinates.X, figureCoordinates.Y);

            return rectangle;
        }

        private Triangle GetTriangle()
        {
            Point figureCoordinates = GetFigureCoordinates(GetMaxCoordinates(figuresCanvas));

            Triangle triangle = new(figureCoordinates.X, figureCoordinates.Y);

            return triangle;
        }

        private Circle GetCircle()
        {
            Point figureCoordinates = GetFigureCoordinates(GetMaxCoordinates(figuresCanvas));

            Circle circle = new(figureCoordinates.X, figureCoordinates.Y);

            return circle;
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

        private Point GetFigureCoordinates(Point maxPoint)
        {
            int length = 100; //max length of side

            double x = _random.Next(length, Convert.ToInt32(maxPoint.X) - length);
            double y = _random.Next(length, Convert.ToInt32(maxPoint.Y) - length);

            return new Point(x, y);
        }
    }
}
