using System;
using Cysharp.Threading.Tasks;

namespace Octavian.Runtime.StateMachines.GenericStateMachine
{
    public interface IState<T> where T: Enum
    {
        T StateName { get; }

        void Initialize(StateMachine<T> stateMachine);

        void ActivateState();

        UniTask Deactivate();
    }
}