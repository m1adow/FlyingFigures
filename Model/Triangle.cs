using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Figures
{
    public class Triangle : Figure
    {
        public Triangle(int x, int y) : base(x, y)
        {
        }

        public override List<UIElement> Draw(List<UIElement> sides)
        {
            return sides;
        }
    }
}
