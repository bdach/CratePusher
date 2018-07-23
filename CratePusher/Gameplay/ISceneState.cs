using System;
using Microsoft.Xna.Framework.Graphics;

namespace CratePusher.Gameplay
{
    public interface ISceneState
    {
        ISceneState Advance(TimeSpan elapsedTime);
        void DrawScene(SpriteBatch spriteBatch);
    }
}