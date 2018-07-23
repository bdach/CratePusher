using System;

namespace CratePusher.Input
{
    public class InputStateManager
    {
        private IInputState currentInputState;

        public InputStateManager()
        {
            currentInputState = new IdleState();
        }

        public InputAction Advance(TimeSpan elapsedTime)
        {
            currentInputState = currentInputState.Advance(elapsedTime);
            return currentInputState.GetInputAction();
        }
    }
}