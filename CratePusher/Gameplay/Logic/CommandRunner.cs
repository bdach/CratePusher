using System.Collections.Generic;
using System.Linq;
using CratePusher.Gameplay.Levels;
using CratePusher.Input;

namespace CratePusher.Gameplay.Logic
{
    public class CommandRunner
    {
        private Stack<List<ICommand>> CommandHistory;

        public CommandRunner()
        {
            CommandHistory = new Stack<List<ICommand>>();
        }

        public void RunCommandsForAction(InputAction inputAction, Level level)
        {
            var commands = new List<ICommand>();
            switch (inputAction)
            {
                case InputAction.Undo:
                    UndoLastCommand(level);
                    return;
                case InputAction.MoveLeft:
                case InputAction.MoveRight:
                case InputAction.MoveUp:
                case InputAction.MoveDown:
                    commands.Add(new MovePlayerCommand(level, inputAction));
                    break;
            }
            var executedCommands = RunCommandChain(commands, level);
            if (executedCommands?.Count > 0)
            {
                CommandHistory.Push(executedCommands);
            }
        }

        private void UndoLastCommand(Level level)
        {
            if (CommandHistory.Count == 0) return;
            var lastAction = CommandHistory.Pop();
            foreach (var command in lastAction)
            {
                command.Rollback(level);
            }
        }

        private List<ICommand> RunCommandChain(List<ICommand> commands, Level level)
        {
            if (commands.Any(command => !command.CanExecute))
            {
                return null;
            }

            var nextInChain = commands.SelectMany(command => command.Execute(level)).ToList();
            if (nextInChain.Count == 0)
            {
                return commands;
            }

            var executedChain = RunCommandChain(nextInChain, level);
            if (executedChain != null)
            {
                commands.AddRange(executedChain);
                return commands;
            }
            foreach (var command in commands)
            {
                command.Rollback(level);
            }
            return null;
        }

        public void ClearHistory()
        {
            CommandHistory.Clear();
        }
    }
}