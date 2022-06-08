using System.Collections.Generic;
using System.Windows;

namespace Figures
{
    public class Rectangle : Figure
    {
        private int _length;

        public Rectangle(int x, int y, int length) : base(x, y)
        {
            _length = length;
        }

        public override List<UIElement> Draw(List<UIElement> sides)
        {
            return sides;
        }

        public override void Move(Point maxCoordinates)
        {
            base.Move(maxCoordinates);
        }
    }
}
