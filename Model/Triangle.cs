using System.Collections.Generic;
using System.Windows;

namespace Figures
{
    public class Triangle : Figure
    {
        private double _rightCorner;
        private double _bottomCorner;

        public Triangle(double x, double y, double rightCorner, double bottomCorner) : base(x, y)
        {
            _rightCorner = x + rightCorner;
            _bottomCorner = y + bottomCorner;
        }

        public override List<UIElement> Draw(List<UIElement> sides)
        {
            return sides;
        }

        public override void Move(Point maxCoordinates)
        {
            if (_rightCorner <= 0 || _rightCorner >= maxCoordinates.X)
                Dx *= -1;

            if (_bottomCorner <= 0 || _bottomCorner >= maxCoordinates.Y)
                Dy *= -1;

            _rightCorner += Dx;
            _bottomCorner += Dy;

            base.Move(maxCoordinates);
        }
    }
}
