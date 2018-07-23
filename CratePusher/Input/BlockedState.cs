using System;

namespace CratePusher.Input
{
    public class BlockedState : IInputState
    {
        public IInputState Advance(TimeSpan elapsedTime)
        {
            var keysPressed = KeyBindings.GetPressedKeys();
            if (keysPressed.Count > 0)
            {
                return this;
            }
            return new IdleState();
        }

        public InputAction GetInputAction()
        {
            return InputAction.None;
        }
    }
}