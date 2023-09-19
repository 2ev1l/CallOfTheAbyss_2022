using System.Collections.Generic;
using UnityEngine;

namespace Universal
{
    public abstract class StateMachine : MonoBehaviour
    {
        [SerializeField] protected List<StateChange> states = new List<StateChange>();
        protected StateChange currentState;

        protected virtual void Start()
        {
            ApplyDefaultState();
            SetStatesAvailability();
        }
        public virtual void ApplyDefaultState() => ApplyStateIgnore(states[0]);
        public virtual void ApplyState(StateChange choosedState)
        {
            if (currentState != null && choosedState == currentState && currentState != states[0]) return;
            ApplyStateIgnore(choosedState);
        }
        private void ApplyStateIgnore(StateChange choosedState)
        {
            currentState = choosedState;
            foreach (var state in states)
                state.SetActive(currentState == state);
        }
        public virtual void SetStatesAvailability() { }
    }

    public abstract class StateChange : MonoBehaviour
    {
        public abstract void SetActive(bool active);
    }
}