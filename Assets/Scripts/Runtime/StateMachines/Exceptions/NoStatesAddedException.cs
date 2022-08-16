using System;

namespace Octavian.Runtime.StateMachines.Exceptions
{
    public class NoStatesAddedException : Exception
    {
        public NoStatesAddedException() { }

        public NoStatesAddedException(string message) : base(message) { }

        public NoStatesAddedException(string message, Exception inner) : base(message, inner) { }
    }
}