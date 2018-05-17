using System.Collections.Generic;
using CratePusher.Gameplay.Levels;
using CratePusher.Input;
using Microsoft.Xna.Framework;

namespace CratePusher.Gameplay.Logic
{
    public class MovePlayerCommand : ICommand
    {
        private readonly Point initialPoint;
        private readonly Point destinationPoint;
        private readonly InputAction inputAction;

        public bool CanExecute { get; }

        internal MovePlayerCommand(Level level, InputAction inputAction)
        {
            this.initialPoint = level.PlayerPosition;
            this.destinationPoint = level.PlayerPosition;
            this.inputAction = inputAction;
            switch (inputAction)
            {
                case InputAction.MoveLeft:
                    destinationPoint.X -= 1;
                    break;
                case InputAction.MoveRight:
                    destinationPoint.X += 1;
                    break;
                case InputAction.MoveUp:
                    destinationPoint.Y -= 1;
                    break;
                case InputAction.MoveDown:
                    destinationPoint.Y += 1;
                    break;
            }
            CanExecute = level.InBounds(destinationPoint) &&
                         !level.Walls.Contains(destinationPoint);
        }
        public ICollection<ICommand> Execute(Level level)
        {
            level.PlayerPosition = destinationPoint;
            var commands = new List<ICommand>();
            if (level.Crates.Contains(destinationPoint))
            {
                commands.Add(new MoveCrateCommand(level, inputAction, destinationPoint));
            }
            return commands;
        }

        public void Rollback(Level level)
        {
            level.PlayerPosition = initialPoint;
        }
    }
}