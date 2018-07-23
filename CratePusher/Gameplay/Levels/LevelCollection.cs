using System.Collections.Generic;

namespace CratePusher.Gameplay.Levels
{
    public class LevelCollection
    {
        public List<Level> Levels { get; }
        public Level CurrentLevel => Levels[levelNumber];
        private int levelNumber;

        public LevelCollection()
        {
            Levels = new List<Level>();
            levelNumber = 0;
        }

        public bool ShouldChangeLevel()
        {
            return CurrentLevel.LevelDone() && levelNumber < Levels.Count;
        }

        public void ChangeLevel()
        {
            levelNumber += 1;
        }
    }
}