using System.Collections.Generic;

namespace CratePusher.Gameplay.Levels
{
    public class LevelCollection
    {
        public List<Level> Levels { get; }

        public LevelCollection()
        {
            Levels = new List<Level>();
        }
    }
}