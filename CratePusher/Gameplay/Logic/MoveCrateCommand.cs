using System.Collections.Generic;
using CratePusher.Gameplay.Levels;
using CratePusher.Input;
using Microsoft.Xna.Framework;

namespace CratePusher.Gameplay.Logic
{
    public class MoveCrateCommand : ICommand
    {
        private readonly Point initialPoint;
        private readonly Point destinationPoint;

        public bool CanExecute { get; }

        internal MoveCrateCommand(Level level, InputAction inputAction, Point initialPoint)
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
                         level.Fields[destinationPoint.Y, destinationPoint.X] != FieldType.Wall &&
                         !level.Crates[destinationPoint.Y, destinationPoint.X];
        }

        public ICollection<ICommand> Execute(Level level)
        {
            level.Crates[initialPoint.Y, initialPoint.X] = false;
            level.Crates[destinationPoint.Y, destinationPoint.X] = true;
            return new ICommand[0];
        }

        public void Rollback(Level level)
        {
            level.Crates[initialPoint.Y, initialPoint.X] = true;
            level.Crates[destinationPoint.Y, destinationPoint.X] = false;
        }
    }
}