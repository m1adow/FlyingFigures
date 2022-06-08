using System.Drawing;

namespace Figures
{
    public abstract class Figure
    {
        private int _x;
        private int _y;

        private int _dx;
        private int _dy;

        //public abstract void Draw(Graphics graphics);

        public virtual void Move(Point maxCoordinates)
        {
            if (_x <= 0 || _x >= maxCoordinates.X)
                _dx = -1 * _dx;
            if (_y <= 0 || _y >= maxCoordinates.Y)
                _dy = -1 * _dy;
        }
    }
}
