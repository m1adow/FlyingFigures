using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Figures
{
    public class Triangle : Figure
    {
        private int _rightCorner;
        private int _bottomCorner;

        public Triangle(int x, int y, int rightCorner, int bottomCorner) : base(x, y)
        {
            _rightCorner = rightCorner;
            _bottomCorner = bottomCorner;
        }

        public override List<UIElement> Draw(List<UIElement> sides)
        {
            return sides;
        }

        public override void Move(Point maxCoordinates)
        {
            if (_rightCorner <= 0 || _rightCorner >= maxCoordinates.X)
                Dx = -1 * Dx;

            if (_bottomCorner <= 0 || _bottomCorner >= maxCoordinates.Y)
                Dy = -1 * Dy;

            base.Move(maxCoordinates);
        }
    }
}
