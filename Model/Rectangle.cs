using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace FlyingFigures.Model
{
    [Serializable]
    public class Rectangle : Figure
    {
        public double XCorner { get; set; }
        public double YCorner { get; set; }

        public override string Type { get; set; } = nameof(Rectangle);

        public Rectangle()
        {

        }

        public Rectangle(double x, double y) : base(x, y)
        {
            XCorner = x + Length;
            YCorner = y + Length / 2;          
        }

        public override List<UIElement> Draw()
        {            
            if (Pattern is null)
                Pattern = GeneratePattern(Length);

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
