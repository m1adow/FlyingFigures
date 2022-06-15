using System;

namespace FlyingFigures.Model.Events
{
    public class CoordinateArgs : EventArgs
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
