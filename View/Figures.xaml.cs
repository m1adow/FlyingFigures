﻿using FlyingFigures.Localization;
using FlyingFigures.Model.Events;
using FlyingFigures.Model.Exceptions;
using FlyingFigures.Model.Figures;
using FlyingFigures.Model.Helpers.DeserializationTools;
using FlyingFigures.Model.Helpers.FigureCollision;
using FlyingFigures.Model.Helpers.SerializationTools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Rectangle = FlyingFigures.Model.Figures.Rectangle;

namespace FlyingFigures.View
{
    /// <summary>
    /// Interaction logic for Figures.xaml
    /// </summary>
    public partial class Figures : Window
    {
        private List<Figure> _figures;

        public Figures()
        {
            InitializeComponent();

            _figures = new List<Figure>();

            DispatcherTimer timer = InitializeTimer();

            Thread movement = new(new ThreadStart(() =>
            {
                timer.Tick += Timer_Move;
                Dispatcher.Run();
            }));
            movement.Start();

            Thread drawing = new(new ThreadStart(() =>
            {
                timer.Tick += Timer_Draw;
                Dispatcher.Run();
            }));
            drawing.Start();
        }

        private void CreateFigure_Click(object sender, RoutedEventArgs e)
        {
            string? name = (sender as Button)?.Content.ToString();

            if (name == Resource.RectangleButton || name == Resource1.RectangleButton)
                AddFigure(GetRectangle());
            else if (name == Resource.TriangleButton || name == Resource1.TriangleButton)
                AddFigure(GetTriangle());
            else if (name == Resource.CircleButton || name == Resource1.CircleButton)
                AddFigure(GetCircle());
        }

        private void AddFigure(Figure figure)
        {
            ChangeCanvasPositionOfFigure(figure.Draw(), figure.X, figure.Y);

            foreach (var line in figure.Draw())
                figuresCanvas.Children.Add(line);

            figuresTreeView.Items.Add(figure);

            _figures.Add(figure);
        }

        private DispatcherTimer InitializeTimer()
        {
            DispatcherTimer timer = new();

            timer.Interval = TimeSpan.FromSeconds(0.001);
            timer.Start();

            return timer;
        }

        private void Timer_Move(object? sender, EventArgs e)
        {
            foreach (var figure in _figures)
                MoveFigure(figure);
        }

        private void MoveFigure(Figure figure)
        {
            try
            {
                figure.Move(GetMaxCoordinates(figuresCanvas));                

                if (figure.CollisionEvents is not null)
                {
                    switch (figure.Type)
                    {
                        case nameof(Rectangle):
                            Rectangle? rectangle = figure as Rectangle;

                            if (_figures.Any(f => f.GetHashCode() != figure.GetHashCode() &&
                            f.Type == figure.Type &&
                            FigureCollision.IsRectanglesCollided(f, rectangle)))
                                figure.CollisionEvents.ForEach(c => c.CollisionRegistered(figure));
                            break;
                        case nameof(Triangle):
                            Triangle? triangle = figure as Triangle;

                            if (_figures.Any(f => f.GetHashCode() != figure.GetHashCode() &&
                            f.Type == figure.Type &&
                            FigureCollision.IsTrianglesCollided(f, triangle)))
                                figure.CollisionEvents.ForEach(c => c.CollisionRegistered(figure));
                            break;
                        case nameof(Circle):
                            Circle? circle = figure as Circle;

                            if (_figures.Any(f => f.GetHashCode() != figure.GetHashCode() &&
                            f.Type == figure.Type &&
                            FigureCollision.IsCirclesCollided(f, circle)))
                                figure.CollisionEvents.ForEach(c => c.CollisionRegistered(figure));
                            break;
                    }
                }
            }
            catch (BehindBorderException exception)
            {
                Point randomCoordinations = GetRandomCoordinates();

                figure.ResetFigurePosition(randomCoordinations);

                LogBehindBorderException(exception);
            }
        }

        private void LogBehindBorderException(BehindBorderException exception)
        {
            using (var writer = new StreamWriter($"{Environment.CurrentDirectory}\\LogChannel.txt"))
                writer.Write($"{DateTime.Now}\n{exception.Message}");
        }

        private void Timer_Draw(object? sender, EventArgs e)
        {
            foreach (var figure in _figures)
                DrawFigure(figure);
        }

        private void DrawFigure(Figure figure)
        {
            ChangeCanvasPositionOfFigure(figure.Draw(), figure.X, figure.Y);
        }

        private void ChangeCanvasPositionOfFigure(List<UIElement> sides, double x, double y)
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
            Point figureCoordinates = GetRandomCoordinates();

            Rectangle rectangle = new(figureCoordinates.X, figureCoordinates.Y);

            return rectangle;
        }

        private Triangle GetTriangle()
        {
            Point figureCoordinates = GetRandomCoordinates();

            Triangle triangle = new(figureCoordinates.X, figureCoordinates.Y);

            return triangle;
        }

        private Circle GetCircle()
        {
            Point figureCoordinates = GetRandomCoordinates();

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

        private Point GetRandomCoordinates()
        {
            Point maxPoint = GetMaxCoordinates(figuresCanvas);

            System.Drawing.Point figureCoordinates = RandomGenerations.RandomValues.GetRandomPoint(new System.Drawing.Point(Convert.ToInt32(maxPoint.X), Convert.ToInt32(maxPoint.Y)));

            return new Point(figureCoordinates.X, figureCoordinates.Y);
        }

        private void SaveFigures_Click(object sender, RoutedEventArgs e)
        {
            string? format = (sender as MenuItem)?.Header.ToString();

            switch (format)
            {
                case "JSON":
                    SerializationJSON.Serialize(_figures, OpenSaveFileDialog(format.ToLower()).FileName);
                    break;
                case "XML":
                    SerializationXML.Serialize(_figures, OpenSaveFileDialog(format.ToLower()).FileName);
                    break;
                case "BIN":
                    SerializationBIN.Serialize(_figures, OpenSaveFileDialog(format.ToLower()).FileName);
                    break;
            }
        }

        private SaveFileDialog OpenSaveFileDialog(string format)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // Default directory
            saveFileDialog.FileName = "FlyingFigures"; // Default file name
            saveFileDialog.DefaultExt = $".{format}"; // Default file extension
            saveFileDialog.Filter = $"{format.ToUpper()} (.{format})|*.{format}"; // Filter files by extension

            saveFileDialog.ShowDialog();

            return saveFileDialog;
        }

        private void LoadFigures_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = OpenFileDialog();

            string format = Path.GetExtension(openFileDialog.FileName).Replace(".", "");

            ClearFigures();

            switch (format.ToUpper())
            {
                case "JSON":
                    DeserializationJSON.Deserialize(out _figures, openFileDialog.FileName);
                    break;
                case "XML":
                    DeserializationXML.Deserialize(out _figures, openFileDialog.FileName);
                    break;
                case "BIN":
                    DeserializationBIN.Deserialize(out _figures, openFileDialog.FileName);
                    break;
            }

            AddFiguresAfterDeserialization();
        }

        private OpenFileDialog OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // Default directory
            openFileDialog.Filter = $"Flying Figures formats (*.json; *.bin; *.xml)|*.json;*.bin;*.xml"; // Filter files by extension

            openFileDialog.ShowDialog();

            return openFileDialog;
        }

        private void ClearFigures()
        {
            _figures.Clear();
            figuresTreeView.Items.Clear();
            figuresCanvas.Children.Clear();
        }

        private void AddFiguresAfterDeserialization()
        {
            foreach (var figure in _figures)
            {
                foreach (var line in figure.Draw())
                    figuresCanvas.Children.Add(line);

                figuresTreeView.Items.Add(figure);
            }
        }

        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            if (figuresTreeView.SelectedItem is null)
                return;

            Figure? figure = figuresTreeView.SelectedItem as Figure;

            if (figure is null)
                return;

            CollisionEvent collisionEvent = new();
            collisionEvent.CollisionHandler += CollisionEvent_CollisionHandler;
            figure.CollisionEvents?.Add(collisionEvent);
        }

        private void RemoveEvent_Click(object sender, RoutedEventArgs e)
        {
            if (figuresTreeView.SelectedItem is null)
                return;

            Figure? figure = figuresTreeView.SelectedItem as Figure;

            if (figure is null)
                return;

            if (figure.CollisionEvents?.Count != 0)
                figure.CollisionEvents?.RemoveAt(figure.CollisionEvents.Count - 1);
        }

        private void CollisionEvent_CollisionHandler(object? sender, CoordinateArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate ()
                    {
                        coordinatesTextBlock.Text = $"X: {e.X}\tY: {e.Y}";
                    });

            SystemSounds.Beep.Play();
        }
    }
}
