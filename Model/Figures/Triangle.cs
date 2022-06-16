using FlyingFigures.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace FlyingFigures.Model.Figures
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
            if (RightCorner < 0 - Length / 2 || BottomCorner < 0 - Length / 2 || RightCorner > maxCoordinates.X + Length / 2 || BottomCorner > maxCoordinates.Y + Length / 2)
                throw new BehindBorderException($"Your figure was behind border.\n\tFigure: {Type};\n\t\t(x;y) - ({X};{Y})");

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

        public override void ResetFigurePosition(Point coordinates)
        {
            base.ResetFigurePosition(coordinates);

            RightCorner = coordinates.X + Length / 2;
            BottomCorner = coordinates.Y + Length;
        }

        public override string ToString()
        {
            return $"Triangle\n\tlength {Length}";
        }
    }
}
