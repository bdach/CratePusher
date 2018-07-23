using System;
using System.Linq;

namespace CratePusher.Input
{
    public class IdleState : IInputState
    {
        public IInputState Advance(TimeSpan elapsedTime)
        {
            var keysPressed = KeyBindings.GetPressedKeys();
            switch (keysPressed.Count)
            {
                case 0:
                    return this;
                case 1:
                    return new ButtonDownState(keysPressed.Single());
                default:
                    return new BlockedState();
            }
        }

        public InputAction GetInputAction()
        {
            return InputAction.None;
        }
    }
}