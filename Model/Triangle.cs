using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace FlyingFigures.Model
{
    [Serializable]
    public class Triangle : Figure
    {
        public double RightCorner { get; set; }
        public double BottomCorner { get; set; }

        public override string Type { get; set; } = nameof(Triangle);

        public Triangle()
        {

        }

        public Triangle(double x, double y) : base(x, y)
        {
            RightCorner = x + Length / 2;
            BottomCorner = y + Length;
        }

        public override List<UIElement> Draw()
        {
            if (Pattern is null)
                Pattern = GeneratePattern(Length);

            return Pattern;
        }

        public override void Move(Point maxCoordinates)
        {
            if (RightCorner <= 0 || RightCorner >= maxCoordinates.X)
                Dx *= -1;

            if (BottomCorner <= 0 || BottomCorner >= maxCoordinates.Y)
                Dy *= -1;

            RightCorner += Dx;
            BottomCorner += Dy;

            base.Move(maxCoordinates);
        }

        private List<UIElement> GeneratePattern(int length)
        {
            List<UIElement> pattern = new()
            {
                new Line()
                {
                    X2 = length / 2,
                    Stroke = GetRandomColor()
                },
                new Line()
                {
                    Y2 = length,
                    Stroke = GetRandomColor()
                },
                new Line()
                {
                    Y1 = length,
                    X2 = length / 2,
                    Stroke = GetRandomColor()
                }
            };

            return pattern;
        }

        public override string ToString()
        {
            return $"Triangle\n\tlength {Length}";
        }
    }
}
