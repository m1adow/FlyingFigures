using FlyingFigures.Model.Figures;

namespace FlyingFigures.Model.Helpers.FigureCollision
{
    public class FigureCollision
    {
        public static bool IsRectanglesCollided(Figure figure, Rectangle rectangle)
        {
            if (figure.X <= rectangle.XCorner &&
                            figure.Y <= rectangle.YCorner &&
                            figure.X + figure.Length >= rectangle.X &&
                            figure.Y + figure.Length / 2 >= rectangle.Y)
                return true;

            return false;
        }

        public static bool IsTrianglesCollided(Figure figure, Triangle triangle)
        {
            if (figure.X <= triangle.RightCorner &&
                            figure.Y <= triangle.BottomCorner &&
                            figure.X + figure.Length / 2 >= triangle.X &&
                            figure.Y + figure.Length >= triangle.Y)
                return true;

            return false;
        }

        public static bool IsCirclesCollided(Figure figure, Circle circle)
        {
            if (figure.X <= circle.Right &&
                            figure.Y <= circle.Top &&
                            figure.X + figure.Length >= circle.Right &&
                            figure.Y + figure.Length >= circle.Top)
                return true;

            return false; 
        }
    }
}
