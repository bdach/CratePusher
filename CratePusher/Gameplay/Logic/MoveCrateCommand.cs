using System;
using System.Collections.Generic;
using CratePusher.Gameplay.Levels;
using CratePusher.Graphics;
using CratePusher.Input;
using Microsoft.Xna.Framework;

namespace CratePusher.Gameplay.Logic
{
    public class MoveCrateCommand : ICommand
    {
        private static readonly TimeSpan AnimationDuration = CratePusher.AnimationDuration;
        private readonly Vector2 initialPoint;
        private Vector2 intermediatePoint;
        private readonly Vector2 destinationPoint;
        private TimeSpan elapsedAnimation = TimeSpan.Zero;

        public bool CanExecute { get; }

        internal MoveCrateCommand(Level level, InputAction inputAction, Vector2 initialPoint)
        {
            this.initialPoint = initialPoint;
            this.intermediatePoint = initialPoint;
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

        public ICollection<ICommand> GetRamifications(Level level)
        {
            return new ICommand[0];
        }

        public void Advance(Level level, TimeSpan elapsedTime)
        {
            this.elapsedAnimation += elapsedTime;
            if (elapsedAnimation > AnimationDuration)
            {
                elapsedAnimation = AnimationDuration;
            }

            float t = (float) (elapsedAnimation.TotalMilliseconds / AnimationDuration.TotalMilliseconds);
            float tt = AnimationCurves.Movement(t);
            level.Crates.Remove(intermediatePoint);
            intermediatePoint = (1 - tt) * initialPoint + tt * destinationPoint;
            level.Crates.Add(intermediatePoint);
        }

        public void Finish(Level level)
        {
            level.Crates.Remove(intermediatePoint);
            level.Crates.Add(destinationPoint);
        }

        public bool Done => elapsedAnimation >= AnimationDuration;

        public void Rollback(Level level)
        {
            level.Crates.Remove(destinationPoint);
            level.Crates.Add(initialPoint);
        }
    }
}