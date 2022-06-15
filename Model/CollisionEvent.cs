using System;

namespace FlyingFigures.Model
{
    public class CollisionEvent
    {
        public event EventHandler<Figure>? CollisionHandler;

        public void CollisionRegistered(Figure figure) 
            => CollisionHandler?.Invoke(this, figure);
    }
}
