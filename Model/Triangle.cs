using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Figures
{
    public class Triangle : Figure
    {
        private double _rightCorner;
        private double _bottomCorner;
        private List<UIElement> _pattern;

        public Triangle(double x, double y) : base(x, y)
        {
            Random random = new();

            int length = random.Next(50, 100);

            _pattern = GeneratePattern(length);

            _rightCorner = x + length / 2;
            _bottomCorner = y + length;
        }

        public override List<UIElement> Draw()
        {           
            return _pattern;
        }

        public override void Move(Point maxCoordinates)
        {
            if (_rightCorner <= 0 || _rightCorner >= maxCoordinates.X)
                Dx *= -1;

            if (_bottomCorner <= 0 || _bottomCorner >= maxCoordinates.Y)
                Dy *= -1;

            _rightCorner += Dx;
            _bottomCorner += Dy;

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
    }
}
