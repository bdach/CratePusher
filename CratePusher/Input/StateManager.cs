using System;

namespace CratePusher.Input
{
    public class StateManager
    {
        private IState currentState;

        public StateManager()
        {
            currentState = new IdleState();
        }

        public InputAction Advance(TimeSpan elapsedTime)
        {
            currentState = currentState.Advance(elapsedTime);
            return currentState.GetInputAction();
        }
    }
}