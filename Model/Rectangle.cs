using System.Windows;

namespace Figures
{
    public class Rectangle : Figure
    {
        public Rectangle(int x, int y) : base(x, y)
        {
        }

        public override UIElement Draw(UIElement uIElement)
        {
            return uIElement;
        }
    }
}
