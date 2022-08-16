using System;

namespace Octavian.Runtime.StateMachines.Exceptions
{
    public class StateNotFoundExceptions : Exception
    {
        public StateNotFoundExceptions() { }

        public StateNotFoundExceptions(string message) : base(message) { }

        public StateNotFoundExceptions(string message, Exception inner) : base(message, inner) { }
    }
}