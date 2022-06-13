using System;
using System.Collections.Generic;
using System.Windows;

namespace Figures
{
    [Serializable]
    public class Circle : Figure
    {
        private double _top;
        private double _right;

        public Circle()
        {

        }

        public Circle(double x, double y) : base(x, y)
        {
            Random random = new();

            int radius = random.Next(50, 100);

            Pattern = GeneratePattern(radius);

            _top = y + radius;
            _right = x + radius;
        }

        public override List<UIElement> Draw()
        {
            return Pattern;
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

        private List<UIElement> GeneratePattern(int radius)
        {
            List<UIElement> pattern = new()
            {
                new System.Windows.Shapes.Ellipse()
                {
                    Width = radius,
                    Height = radius,
                    Stroke = GetRandomColor()
                }
            };

            return pattern;
        }
    }
}
