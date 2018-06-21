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
        private List<ICommand> currentCommands = null;

        public CommandRunner()
        {
            commandHistory = new Stack<List<ICommand>>();
        }

        public void BeginAction(InputAction inputAction, Level level)
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
                case InputAction.None:
                    return;
            }
            currentCommands = GetCommandChain(commands, level);
        }

        public void AdvanceAction(Level level, TimeSpan elapsedTime)
        {
            if (currentCommands == null) return;
            if (currentCommands.All(command => command.Done))
            {
                foreach (var command in currentCommands)
                {
                    command.Finish(level);
                }
                commandHistory.Push(currentCommands);
                currentCommands = null;
                return;
            }
            foreach (var command in currentCommands)
            {
                command.Advance(level, elapsedTime);
            }
        }

        private static List<ICommand> GetCommandChain(List<ICommand> commands, Level level)
        {
            if (commands.Any(command => !command.CanExecute))
            {
                return null;
            }

            var nextInChain = commands.SelectMany(command => command.GetRamifications(level)).ToList();
            if (nextInChain.Count == 0)
            {
                return commands;
            }

            var executedChain = GetCommandChain(nextInChain, level);
            if (executedChain != null)
            {
                commands.AddRange(executedChain);
                return commands;
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