using FlyingFigures.Localization;
using FlyingFigures.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Serialization;
using Rectangle = FlyingFigures.Model.Rectangle;

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

            if (name == Resource.RectangleButton || name == Resource1.RectangleButton)
                AddFigure(GetRectangle());
            else if (name == Resource.TriangleButton || name == Resource1.TriangleButton)
                AddFigure(GetTriangle());
            else if (name == Resource.CircleButton || name == Resource1.CircleButton)
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

                if (figure.CollisionEvents is not null)
                {
                    if (_figures.Any(f => f.GetHashCode() != figure.GetHashCode() && 
                    f.X == figure.X && 
                    f.Y == figure.Y &&
                    f.Type == figure.Type))
                        figure.CollisionEvents.ForEach(c => c.CollisionRegistered(figure));
                }
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
            Point figureCoordinates = GetSpawnCoordinates();

            Rectangle rectangle = new(figureCoordinates.X, figureCoordinates.Y);

            return rectangle;
        }

        private Triangle GetTriangle()
        {
            Point figureCoordinates = GetSpawnCoordinates();

            Triangle triangle = new(figureCoordinates.X, figureCoordinates.Y);

            return triangle;
        }

        private Circle GetCircle()
        {
            Point figureCoordinates = GetSpawnCoordinates();

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

        private Point GetSpawnCoordinates()
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
                    SerializeInJson(format.ToLower());
                    break;
                case "XML":
                    SerializeInXml(format.ToLower());
                    break;
                case "BIN":
                    SerializeInBytes(format.ToLower());
                    break;
            }
        }

        private void SerializeInJson(string format)
        {
            string json = JsonSerializer.Serialize((object)_figures, new JsonSerializerOptions()
            {
                WriteIndented = true,
            });

            var saveFileDialog = OpenSaveFileDialog(format);

            using (var writer = new StreamWriter(saveFileDialog.FileName))
                writer.Write(json);
        }

        private void SerializeInXml(string format)
        {
            var saveFileDialog = OpenSaveFileDialog(format);

            XmlSerializer serializer = new(typeof(List<Figure>));

            using (var writer = new StreamWriter(saveFileDialog.FileName))
                serializer.Serialize(writer, _figures);
        }

        private void SerializeInBytes(string format)
        {
            var saveFileDialog = OpenSaveFileDialog(format);

            using (var stream = File.Open(saveFileDialog.FileName, FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();

                binaryFormatter.Serialize(stream, _figures);
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
                    DeserializeInJson(openFileDialog.FileName);
                    break;
                case "XML":
                    DeserializeInXml(openFileDialog.FileName);
                    break;
                case "BIN":
                    DeserializeInBytes(openFileDialog.FileName);
                    break;
            }
        }

        private void DeserializeInJson(string path)
        {
            using (var reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();

                _figures = JsonSerializer.Deserialize<List<Figure>>(json);               
            }

            AddFiguresAfterDeserialization();
        }

        private void DeserializeInXml(string path)
        {
            XmlSerializer serializer = new(typeof(List<Figure>));

            using (var reader = new StreamReader(path))
                _figures = (List<Figure>)serializer.Deserialize(reader);

            AddFiguresAfterDeserialization();
        }

        private void DeserializeInBytes(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var binaryFormatter = new BinaryFormatter();

                _figures = (List<Figure>)binaryFormatter.Deserialize(stream);
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

        private void CollisionEvent_CollisionHandler(object? sender, Figure e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate ()
                    {
                        coordinatesTextBlock.Text = $"X: {e.X}\tY: {e.Y}";
                    });
        }
    }
}
