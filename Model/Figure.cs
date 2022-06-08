using System;
using System.Windows;

namespace Figures
{
    public abstract class Figure
    {
        private int _dx;
        private int _dy;

        public int X { get; private set; }
        public int Y { get; private set; }      

        public Figure(int x, int y)
        {
            X = x;
            Y = y;

            Random random = new();

            do
            {
                _dx = random.Next(-5, 5);
                _dy = random.Next(-5, 5);
            }
            while (_dx == 0 && _dy == 0);
        }

        public abstract UIElement Draw(UIElement uIElement);

        public virtual void Move(Point maxCoordinates)
        {
            if (X <= 0 || X >= maxCoordinates.X)
                _dx = -1 * _dx;
            if (Y <= 0 || Y >= maxCoordinates.Y)
                _dy = -1 * _dy;

            X += _dx;
            Y += _dy;
        }
    }
}
