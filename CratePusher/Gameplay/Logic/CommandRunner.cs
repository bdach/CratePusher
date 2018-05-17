using System;
using System.Collections.Generic;
using System.Linq;
using CratePusher.Gameplay.Levels;
using CratePusher.Input;

namespace CratePusher.Gameplay.Logic
{
    public class CommandRunner
    {
        private readonly Stack<List<ICommand>> commandHistory;

        public CommandRunner()
        {
            commandHistory = new Stack<List<ICommand>>();
        }

        public void RunCommandsForAction(InputAction inputAction, Level level)
        {
            var commands = new List<ICommand>();
            switch (inputAction)
            {
                case InputAction.ResetLevel:
                    ResetLevel(level);
                    return;
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
                commandHistory.Push(executedCommands);
            }
        }

        private static List<ICommand> RunCommandChain(List<ICommand> commands, Level level)
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

        private void ResetLevel(Level level)
        {
            foreach (var action in commandHistory)
            {
                foreach (var command in action)
                {
                    command.Rollback(level);
                }
            }
            ClearHistory();
        }

        private void UndoLastCommand(Level level)
        {
            if (commandHistory.Count == 0) return;
            var lastAction = commandHistory.Pop();
            foreach (var command in lastAction)
            {
                command.Rollback(level);
            }
        }

        public void ClearHistory()
        {
            commandHistory.Clear();
        }
    }
}