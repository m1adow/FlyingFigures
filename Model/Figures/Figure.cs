using FlyingFigures.Model.Converter;
using FlyingFigures.Model.Events;
using FlyingFigures.Model.Exceptions;
using RandomGenerations;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace FlyingFigures.Model.Figures
{
    [Serializable]
    [XmlInclude(typeof(Rectangle))]
    [XmlInclude(typeof(Triangle))]
    [XmlInclude(typeof(Circle))]
    [JsonConverter(typeof(FigureConverter))]
    public abstract class Figure
    {
        [NonSerialized]
        protected List<UIElement>? Pattern;
        [NonSerialized]
        public List<CollisionEvent>? CollisionEvents;

        public abstract string Type { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public double Dx { get; set; }
        public double Dy { get; set; }

        public int Length { get; set; }


        public Figure()
        {
            Length = RandomValues.GetRandomLength();

            CollisionEvents = new List<CollisionEvent>();
        }

        public Figure(double x, double y)
        {
            X = x;
            Y = y;

            Length = RandomValues.GetRandomLength();

            Random random = new();

            do
            {
                Dx = random.Next(-3, 3);
                Dy = random.Next(-3, 3);
            }
            while (Dx == 0 && Dy == 0);

            CollisionEvents = new List<CollisionEvent>();
        }

        public abstract List<UIElement> Draw();

        public virtual void Move(Point maxCoordinates)
        {
            if (X < 0 - Length / 2 || Y < 0 - Length / 2 || X > maxCoordinates.X + Length / 2 || Y > maxCoordinates.Y + Length / 2)
                throw new BehindBorderException($"Your figure was behind border.\n\tFigure: {Type};\n\t\t(x;y) - ({X};{Y})");

            if (X <= 0 || X >= maxCoordinates.X)
                Dx *= -1;
            if (Y <= 0 || Y >= maxCoordinates.Y)
                Dy *= -1;

            X += Dx;
            Y += Dy;
        }

        public virtual SolidColorBrush GetRandomColor()
        {
            Random random = new();

            return new SolidColorBrush(Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255)));
        }

        public virtual void ResetFigurePosition(Point coordinates)
        {
            X = coordinates.X;
            Y = coordinates.Y;
        }
    }
}
