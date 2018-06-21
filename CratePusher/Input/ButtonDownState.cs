using System;
using Microsoft.Xna.Framework.Input;

namespace CratePusher.Input
{
    public class ButtonDownState : IState
    {
        private static readonly TimeSpan RepeatInterval = CratePusher.AnimationDuration;

        private readonly Keys keyPressed;
        private TimeSpan lastRepeat;

        public ButtonDownState(Keys keyPressed)
        {
            this.keyPressed = keyPressed;
            this.lastRepeat = RepeatInterval; // to trigger the first movement immediately
        }

        public IState Advance(TimeSpan elapsedTime)
        {
            var keysPressed = KeyBindings.GetPressedKeys();
            if (!keysPressed.Contains(keyPressed))
            {
                return new IdleState();
            }
            if (keysPressed.Count > 1)
            {
                return new BlockedState();
            }
            lastRepeat += elapsedTime;
            return this;
        }

        public InputAction GetInputAction()
        {
            if (lastRepeat < RepeatInterval)
            {
                return InputAction.None;
            }
            lastRepeat = TimeSpan.Zero;
            return KeyBindings.GetBinding(keyPressed);
        }
    }
}