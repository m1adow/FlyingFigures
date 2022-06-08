using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Figures
{
    public class Circle : Figure
    {
        private int _radius;

        public Circle(int x, int y, int radius) : base(x, y)
        {
            _radius = radius;
        }

        public override List<UIElement> Draw(List<UIElement> sides)
        {
            return sides;
        }

        public override void Move(Point maxCoordinates)
        {
            if (_radius <= 0 || _radius >= maxCoordinates.X)
                Dx = -1 * Dx;
            if (_radius <= 0 || _radius >= maxCoordinates.Y)
                Dx = -1 * Dx;

            if (-1 * _radius <= 0 || -1 * _radius >= maxCoordinates.X)
                Dx = -1 * Dx;
            if (-1 * _radius <= 0 || -1 * _radius >= maxCoordinates.Y)
                Dx = -1 * Dx;

            base.Move(maxCoordinates);
        }
    }
}
