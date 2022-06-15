using System;

namespace FlyingFigures.Model.Events
{
    public class CollisionEvent
    {
        public event EventHandler<CoordinateArgs>? CollisionHandler;

        public void CollisionRegistered(Figure figure)
            => CollisionHandler?.Invoke(this, new CoordinateArgs()
            {
                X = figure.X,
                Y = figure.Y,
            });
    }
}
