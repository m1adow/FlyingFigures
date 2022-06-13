using Figures;
using FlyingFigures.Localization;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Serialization;
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

            if (name == Resource.RectangleButton)
                AddFigure(GetRectangle());
            else if (name == Resource.TriangleButton)
                AddFigure(GetTriangle());
            else if (name == Resource.CircleButton)
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

                foreach (var figure in _figures)
                    binaryFormatter.Serialize(stream, figure);
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
                serializer.Deserialize(reader);
        }

        private void DeserializeInBytes(string path)
        {
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
    }
}
