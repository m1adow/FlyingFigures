using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Figures
{
    [Serializable]
    [XmlInclude(typeof(Rectangle))]
    [XmlInclude(typeof(Triangle))]
    [XmlInclude(typeof(Circle))]
    public abstract class Figure
    {
        [NonSerialized]
        protected List<UIElement>? Pattern;

        public double X { get; set; }
        public double Y { get; set; }

        public double Dx { get; set; }
        public double Dy { get; set; }
       
        public Figure()
        {

        }

        public Figure(double x, double y)
        {
            X = x;
            Y = y;

            Random random = new();

            do
            {
                Dx = random.Next(-3, 3);
                Dy = random.Next(-3, 3);
            }
            while (Dx == 0 && Dy == 0);
        }

        public abstract List<UIElement> Draw();

        public virtual void Move(Point maxCoordinates)
        {
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
    }
}
