using System.Collections.Generic;
using CratePusher.Gameplay.Levels;
using CratePusher.Input;
using Microsoft.Xna.Framework;

namespace CratePusher.Gameplay.Logic
{
    public class MoveCrateCommand : ICommand
    {
        private readonly Vector2 initialPoint;
        private readonly Vector2 destinationPoint;

        public bool CanExecute { get; }

        internal MoveCrateCommand(Level level, InputAction inputAction, Vector2 initialPoint)
        {
            this.initialPoint = initialPoint;
            this.destinationPoint = initialPoint;
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
                         !level.Walls.Contains(destinationPoint) &&
                         !level.Crates.Contains(destinationPoint);
        }

        public ICollection<ICommand> Execute(Level level)
        {
            level.Crates.Remove(initialPoint);
            level.Crates.Add(destinationPoint);
            return new ICommand[0];
        }

        public void Rollback(Level level)
        {
            level.Crates.Remove(destinationPoint);
            level.Crates.Add(initialPoint);
        }
    }
}