using System;
using CratePusher.Gameplay.Levels;
using CratePusher.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace CratePusher.Gameplay
{
    public class LevelTransitionState : ISceneState
    {
        private readonly LevelCollection levelCollection;
        private readonly LevelRenderer levelRenderer;
        private readonly TimeSpan TransitionTime = TimeSpan.FromSeconds(2);

        private TimeSpan totalTime;
        private bool levelChanged;

        public LevelTransitionState(LevelCollection levelCollection, LevelRenderer levelRenderer)
        {
            this.levelCollection = levelCollection;
            this.levelRenderer = levelRenderer;
        }

        public ISceneState Advance(TimeSpan elapsedTime)
        {
            totalTime += elapsedTime;
            if (totalTime >= TransitionTime + TransitionTime)
            {
                return new GameplayState(levelCollection, levelRenderer);
            }
            if (totalTime >= TransitionTime && !levelChanged)
            {
                levelCollection.ChangeLevel();
                levelChanged = true;
            }
            return this;
        }

        public void DrawScene(SpriteBatch spriteBatch)
        {
            var transitionPoint = 1 - Math.Abs(totalTime.TotalMilliseconds / TransitionTime.TotalMilliseconds - 1);
            levelRenderer.Render(levelCollection.CurrentLevel, spriteBatch, transitionPoint);
        }
    }
}