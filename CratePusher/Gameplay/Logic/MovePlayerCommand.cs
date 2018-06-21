using System;
using System.Collections.Generic;
using CratePusher.Gameplay.Levels;
using CratePusher.Input;
using Microsoft.Xna.Framework;

namespace CratePusher.Gameplay.Logic
{
    public class MovePlayerCommand : ICommand
    {
        private static readonly TimeSpan AnimationDuration = CratePusher.AnimationDuration;

        private readonly Vector2 initialPoint;
        private readonly Vector2 destinationPoint;
        private readonly Direction initialDirection;
        private readonly Direction finalDirection;
        private readonly InputAction inputAction;

        private TimeSpan elapsedAnimation = TimeSpan.Zero;

        public bool CanExecute { get; }

        internal MovePlayerCommand(Level level, InputAction inputAction)
        {
            this.initialPoint = level.PlayerPosition;
            this.destinationPoint = level.PlayerPosition;
            this.initialDirection = level.PlayerDirection;
            this.inputAction = inputAction;
            switch (inputAction)
            {
                case InputAction.MoveLeft:
                    destinationPoint.X -= 1;
                    finalDirection = Direction.Left;
                    break;
                case InputAction.MoveRight:
                    destinationPoint.X += 1;
                    finalDirection = Direction.Right;
                    break;
                case InputAction.MoveUp:
                    destinationPoint.Y -= 1;
                    finalDirection = Direction.Up;
                    break;
                case InputAction.MoveDown:
                    destinationPoint.Y += 1;
                    finalDirection = Direction.Down;
                    break;
            }
            CanExecute = level.InBounds(destinationPoint) &&
                         !level.Walls.Contains(destinationPoint);
        }

        public ICollection<ICommand> GetRamifications(Level level)
        {
            var commands = new List<ICommand>();
            if (level.Crates.Contains(destinationPoint))
            {
                commands.Add(new MoveCrateCommand(level, inputAction, destinationPoint));
            }
            return commands;
        }

        public void Advance(Level level, TimeSpan elapsedTime)
        {
            this.elapsedAnimation += elapsedTime;
            if (elapsedAnimation > AnimationDuration)
            {
                elapsedAnimation = AnimationDuration;
            }

            float t = (float) (elapsedAnimation.TotalMilliseconds / AnimationDuration.TotalMilliseconds);
            level.PlayerPosition = (1 - t) * initialPoint + t * destinationPoint;
            level.PlayerFrameCycle = (int) (elapsedAnimation.TotalMilliseconds / 150) % 2 + 1;
            level.PlayerDirection = finalDirection;
        }

        public void Finish(Level level)
        {
            level.PlayerFrameCycle = 0;
            level.PlayerPosition = destinationPoint;
        }

        public bool Done => elapsedAnimation >= AnimationDuration;

        public void Rollback(Level level)
        {
            level.PlayerPosition = initialPoint;
            level.PlayerDirection = initialDirection;
        }
    }
}