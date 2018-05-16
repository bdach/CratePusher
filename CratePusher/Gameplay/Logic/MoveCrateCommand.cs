using System.Collections.Generic;
using CratePusher.Gameplay.Levels;
using CratePusher.Input;
using Microsoft.Xna.Framework;

namespace CratePusher.Gameplay.Logic
{
    public class MoveCrateCommand : ICommand
    {
        // TODO: fields will get erased now
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
                         level.Fields[destinationPoint.Y, destinationPoint.X] != FieldType.Stone;
        }

        public ICollection<ICommand> Execute(Level level)
        {
            level.Fields[initialPoint.Y, initialPoint.X] = FieldType.Nothing;
            level.Fields[destinationPoint.Y, destinationPoint.X] = FieldType.Stone;
            return new ICommand[0];
        }

        public void Rollback(Level level)
        {
            level.Fields[initialPoint.Y, initialPoint.X] = FieldType.Stone;
            level.Fields[destinationPoint.Y, destinationPoint.X] = FieldType.Nothing;
        }
    }
}