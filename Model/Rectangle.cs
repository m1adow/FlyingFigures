using System.Collections.Generic;
using System.Windows;

namespace Figures
{
    public class Rectangle : Figure
    {
        private int _xCorner;
        private int _yCorner;

        public Rectangle(int x, int y, int xCorner, int yCorner) : base(x, y)
        {
            _xCorner = x + xCorner;
            _yCorner = y + yCorner;
        }

        public override List<UIElement> Draw(List<UIElement> sides)
        {
            return sides;
        }

        public override void Move(Point maxCoordinates)
        {
            if (_xCorner <= 0 || _xCorner >= maxCoordinates.X)
                Dx *= -1;
            if (_yCorner <= 0 || _yCorner >= maxCoordinates.Y)
                Dy *= -1;

            _xCorner += Dx;
            _yCorner += Dy;

            base.Move(maxCoordinates);
        }
    }
}
