using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Octavian.Runtime.StateMachines.Exceptions;
using UnityEngine;

namespace Octavian.Runtime.StateMachines.GenericStateMachine
{
    [Serializable]
    public class StateMachine<T> where T: Enum
    {
        [SerializeField] [ReadOnly] private T currentState = default;
        
        private List<IState<T>> _availableStates;
        private IState<T> _currentlyActiveState;
        
        public T CurrentState => currentState;

        public StateMachine(List<IState<T>> availableStates, T startingState = default)
        {
            _availableStates = availableStates;
            _availableStates.ForEach(s => s.Initialize(this));
            
            ActivateState(startingState);
        }

        public async void SetStateAsync(T newState)
        {
            if (_currentlyActiveState.StateName.Equals(newState)) return;
            await _currentlyActiveState.Deactivate();

            ActivateState(newState);
        }

        private void ActivateState(T newState)
        {
            _currentlyActiveState = _availableStates.FirstOrDefault(s => s.StateName.Equals(newState)) ?? throw new StateNotFoundExceptions();
            _currentlyActiveState.ActivateState();
            currentState = _currentlyActiveState.StateName;
        }

        public void SetNextStateAsync()
        {
            var nextStateIndex = _availableStates.IndexOf(_currentlyActiveState) + 1;
            if (nextStateIndex >= _availableStates.Count) return;
            
            SetStateAsync(_availableStates[nextStateIndex].StateName);
        }
        
        public void PrintAvailableStates()
        {
            _availableStates.ForEach(s => Debug.Log(s.StateName));
        }
    }
}