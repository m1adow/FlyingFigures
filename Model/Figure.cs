using System;
using System.Collections.Generic;
using System.Windows;

namespace Figures
{
    public abstract class Figure
    {
        protected int Dx;
        protected int Dy;

        public int X { get; private set; }
        public int Y { get; private set; }      

        public Figure(int x, int y)
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

        public abstract List<UIElement> Draw(List<UIElement> sides);

        public virtual void Move(Point maxCoordinates)
        {
            if (X <= 0 || X >= maxCoordinates.X)
                Dx *= -1;
            if (Y <= 0 || Y >= maxCoordinates.Y)
                Dy *= -1;

            X += Dx;
            Y += Dy;
        }
    }
}
