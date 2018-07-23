using System;

namespace CratePusher.Input
{
    public interface IInputState
    {
        IInputState Advance(TimeSpan elapsedTime);
        InputAction GetInputAction();
    }
}