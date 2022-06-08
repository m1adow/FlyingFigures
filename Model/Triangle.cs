using System.Windows;

namespace Figures
{
    public class Triangle : Figure
    {
        public Triangle(int x, int y) : base(x, y)
        {
        }

        public override UIElement Draw(UIElement uIElement)
        {
            return uIElement;
        }
    }
}
