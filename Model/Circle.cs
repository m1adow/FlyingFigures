using System.Windows;

namespace Figures
{
    public class Circle : Figure
    {
        public Circle(int x, int y) : base(x, y)
        {
        }

        public override UIElement Draw(UIElement uIElement)
        {
            return uIElement;
        }
    }
}
