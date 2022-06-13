using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Figures
{
    [Serializable]
    public class Rectangle : Figure
    {
        public double XCorner { get; set; }
        public double YCorner { get; set; }

        public override string Type { get; set; } = nameof(Rectangle);

        public Rectangle()
        {
            Random random = new();

            int length = random.Next(50, 100);

            Pattern = GeneratePattern(length);
        }

        public Rectangle(double x, double y) : base(x, y)
        {
            Random random = new();

            int length = random.Next(50, 100);

            Pattern = GeneratePattern(length);

            XCorner = x + length;
            YCorner = y + length / 2;          
        }

        public override List<UIElement> Draw()
        {            
            return Pattern;
        }

        public override void Move(Point maxCoordinates)
        {
            if (XCorner <= 0 || XCorner >= maxCoordinates.X)
                Dx *= -1;
            if (YCorner <= 0 || YCorner >= maxCoordinates.Y)
                Dy *= -1;

            XCorner += Dx;
            YCorner += Dy;

            base.Move(maxCoordinates);
        }

        private List<UIElement> GeneratePattern(int length)
        {
            List<UIElement> pattern = new()
            {
                new Line()
                {
                    X2 = length,
                    Stroke = GetRandomColor()
                },
                new Line()
                {
                    X1 = 0,
                    Y2 = length / 2,
                    Stroke = GetRandomColor()
                },
                new Line()
                {
                    X2 = length,
                    Y1 = length / 2,
                    Y2 = length / 2,
                    Stroke = GetRandomColor()
                },
                new Line()
                {
                    X1 = length,
                    X2 = length,
                    Y2 = length / 2,
                    Stroke = GetRandomColor()
                }
            };

            return pattern;
        }
    }
}
