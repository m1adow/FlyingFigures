namespace FlyingFigures.Model
{
    public class CollisionEvent
    {
        public delegate void CollisionDelegate(Figure left, Figure right);

        public event CollisionDelegate? CollisionHandler;
    }
}
