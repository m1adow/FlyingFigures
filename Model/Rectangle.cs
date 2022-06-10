using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Figures
{
    public class Rectangle : Figure
    {
        private double _xCorner;
        private double _yCorner;
        private List<UIElement> _pattern;

        public Rectangle(double x, double y) : base(x, y)
        {
            Random random = new();

            int length = random.Next(50, 100);

            _pattern = GeneratePattern(length);

            _xCorner = x + length;
            _yCorner = y + length / 2;          
        }

        public override List<UIElement> Draw()
        {            
            return _pattern;
        }

        public override void Move(Point maxCoordinates)
        {
            if (_xCorner <= 0 || _xCorner >= maxCoordinates.X)
                Dx *= -1;
            if (_yCorner <= 0 || _yCorner >= maxCoordinates.Y)
                Dy *= -1;

            _xCorner += Dx;
            _yCorner += Dy;

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
