using System;
using System.Collections.Generic;
using System.Windows;

namespace FlyingFigures.Model
{
    [Serializable]
    public class Circle : Figure
    {
        public double Top { get; set; }
        public double Right { get; set; }

        public override string Type { get; set; } = nameof(Circle);

        public Circle()
        {

        }

        public Circle(double x, double y) : base(x, y)
        {
            Top = y + Length;
            Right = x + Length;
        }

        public override List<UIElement> Draw()
        {
            if (Pattern is null)
                Pattern = GeneratePattern(Length);

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

        public override string ToString()
        {
            return $"Circle\n\tlength {Length}";
        }
    }
}
