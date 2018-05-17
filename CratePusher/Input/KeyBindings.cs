using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace CratePusher.Input
{
    public class KeyBindings
    {
        private static readonly Dictionary<Keys, InputAction> KeyboardBindings = new Dictionary<Keys, InputAction>
        {
            {Keys.Left,  InputAction.MoveLeft},
            {Keys.Right, InputAction.MoveRight},
            {Keys.Down,  InputAction.MoveDown},
            {Keys.Up,    InputAction.MoveUp},
            {Keys.U,     InputAction.Undo},
            {Keys.R,     InputAction.ResetLevel}
        };

        public static InputAction GetBinding(Keys key)
        {
            KeyboardBindings.TryGetValue(key, out var action);
            return action;
        }

        public static HashSet<Keys> GetPressedKeys()
        {
            var keyboardState = Keyboard.GetState();
            return new HashSet<Keys>(KeyboardBindings.Keys.Where(key => keyboardState.IsKeyDown(key)));
        }
    }
}