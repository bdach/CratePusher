using System.Collections.Generic;
using CratePusher.Gameplay.Levels;

namespace CratePusher.Gameplay.Logic
{
    public interface ICommand
    {
        bool CanExecute { get; }
        ICollection<ICommand> Execute(Level level);
        void Rollback(Level level);
    }
}