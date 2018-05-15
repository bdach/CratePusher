using System;

namespace CratePusher.Input
{
    public interface IState
    {
        IState Advance(TimeSpan elapsedTime);
        InputAction GetInputAction();
    }
}