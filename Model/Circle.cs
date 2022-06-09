using System.Collections.Generic;
using System.Windows;

namespace Figures
{
    public class Circle : Figure
    {
        private double _top;
        private double _right;

        public Circle(double x, double y, double radius) : base(x, y)
        {
            _top = y + radius;
            _right = x + radius;
        }

        public override List<UIElement> Draw(List<UIElement> sides)
        {
            return sides;
        }

        public override void Move(Point maxCoordinates)
        {
            if (_right <= 0 || _right >= maxCoordinates.X)
                Dx *= -1;

            if (_top <= 0 || _top >= maxCoordinates.Y)
                Dy *= -1;

            _right += Dx;
            _top += Dy;

            base.Move(maxCoordinates);
        }
    }
}
