using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Figures
{
    public abstract class Figure
    {
        protected double Dx;
        protected double Dy;

        public double X { get; private set; }
        public double Y { get; private set; }

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
