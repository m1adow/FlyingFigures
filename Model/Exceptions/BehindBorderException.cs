using System;

namespace FlyingFigures.Model.Exceptions
{
    public class BehindBorderException : Exception
    {
        public BehindBorderException(string? message) : base(message)
        {
        }
    }
}
