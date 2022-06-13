using System;
using System.Collections.Generic;
using System.Windows;

namespace Figures
{
    [Serializable]
    public class Circle : Figure
    {
        public double Top { get; set; }
        public double Right { get; set; }

        public override string Type { get; set; } = nameof(Circle);

        public Circle()
        {
            Random random = new();

            int radius = random.Next(50, 100);

            Pattern = GeneratePattern(radius);
        }

        public Circle(double x, double y) : base(x, y)
        {
            Random random = new();

            int radius = random.Next(50, 100);

            Pattern = GeneratePattern(radius);

            Top = y + radius;
            Right = x + radius;
        }

        public override List<UIElement> Draw()
        {
            return Pattern;
        }

        public override void Move(Point maxCoordinates)
        {
            if (Right <= 0 || Right >= maxCoordinates.X)
                Dx *= -1;

            if (Top <= 0 || Top >= maxCoordinates.Y)
                Dy *= -1;

            Right += Dx;
            Top += Dy;

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
