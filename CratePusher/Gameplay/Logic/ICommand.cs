using System;
using System.Collections.Generic;
using CratePusher.Gameplay.Levels;

namespace CratePusher.Gameplay.Logic
{
    public interface ICommand
    {
        bool CanExecute { get; }
        ICollection<ICommand> GetRamifications(Level level);
        void Advance(Level level, TimeSpan elapsedTime);
        void Finish(Level level);
        bool Done { get; }
        void Rollback(Level level);
    }
}