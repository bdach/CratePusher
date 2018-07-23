using System;
using CratePusher.Gameplay.Levels;
using CratePusher.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace CratePusher.Gameplay
{
    public class SceneStateManager
    {
        private readonly LevelCollection levelCollection;
        private readonly LevelRenderer levelRenderer;

        private ISceneState sceneState;

        public SceneStateManager(LevelCollection levelCollection, LevelRenderer levelRenderer)
        {
            this.levelCollection = levelCollection;
            this.levelRenderer = levelRenderer;
            this.sceneState = new GameplayState(levelCollection, levelRenderer);
        }

        public void Advance(TimeSpan timeSpan)
        {
            sceneState = sceneState.Advance(timeSpan);
        }

        public void DrawScene(SpriteBatch spriteBatch)
        {
            sceneState.DrawScene(spriteBatch);
        }
    }
}