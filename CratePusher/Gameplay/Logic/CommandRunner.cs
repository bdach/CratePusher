using System.Collections.Generic;
using System.Linq;
using CratePusher.Gameplay.Levels;
using CratePusher.Input;

namespace CratePusher.Gameplay.Logic
{
    public class CommandRunner
    {
        public void RunCommandsForAction(InputAction inputAction, Level level)
        {
            ICollection<ICommand> commands = new List<ICommand>();
            switch (inputAction)
            {
                case InputAction.MoveLeft:
                case InputAction.MoveRight:
                case InputAction.MoveUp:
                case InputAction.MoveDown:
                    commands.Add(new MovePlayerCommand(level, inputAction));
                    break;
            }
            RunCommandChain(commands, level);
        }

        private bool RunCommandChain(ICollection<ICommand> commands, Level level)
        {
            if (commands.Any(command => !command.CanExecute))
            {
                return false;
            }
            var nextInChain = commands.SelectMany(command => command.Execute(level)).ToList();
            if (nextInChain.Count == 0 || RunCommandChain(nextInChain, level))
            {
                return true;
            }
            foreach (var command in commands)
            {
                command.Rollback(level);
            }

            return false;
        }
    }
}