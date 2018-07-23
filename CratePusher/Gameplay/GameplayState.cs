using System;
using System.Collections.Generic;
using CratePusher.Gameplay.Levels;
using CratePusher.Gameplay.Logic;
using CratePusher.Graphics;
using CratePusher.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CratePusher.Gameplay
{
    public class GameplayState : ISceneState
    {
        private readonly LevelCollection levelCollection;
        private readonly CommandRunner commandRunner;
        private readonly LevelRenderer levelRenderer;

        private readonly InputStateManager inputStateManager;
        private readonly Queue<InputAction> inputBuffer;

        public GameplayState(LevelCollection levelCollection, LevelRenderer levelRenderer)
        {
            this.levelCollection = levelCollection;
            this.levelRenderer = levelRenderer;
            this.commandRunner = new CommandRunner();
            this.inputStateManager = new InputStateManager();
            this.inputBuffer = new Queue<InputAction>();
        }

        public ISceneState Advance(TimeSpan elapsedTime)
        {
            if (levelCollection.ShouldChangeLevel())
            {
                return new LevelTransitionState(levelCollection, levelRenderer);
            }

            var input = inputStateManager.Advance(elapsedTime);
            if (input != InputAction.None)
            {
                inputBuffer.Enqueue(input);
            }

            if (commandRunner.AdvanceAction(levelCollection.CurrentLevel, elapsedTime) || inputBuffer.Count == 0)
            {
                return this;
            }

            var nextInput = inputBuffer.Dequeue();
            commandRunner.BeginAction(nextInput, levelCollection.CurrentLevel);
            commandRunner.AdvanceAction(levelCollection.CurrentLevel, elapsedTime);
            return this;
        }

        public void DrawScene(SpriteBatch spriteBatch)
        {
            levelRenderer.Render(levelCollection.CurrentLevel, spriteBatch);
        }
    }
}