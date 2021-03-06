﻿using System;
using CratePusher.Gameplay;
using CratePusher.Gameplay.Levels;
using CratePusher.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CratePusher
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class CratePusher : Game
    {
        public static readonly TimeSpan AnimationDuration = TimeSpan.FromMilliseconds(200);

        private readonly GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        private LevelRenderer levelRenderer;
        private LevelCollection levelCollection;
        private SceneStateManager sceneStateManager;

        public CratePusher()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
//            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
//            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var tileSheetTexture = Content.Load<Texture2D>("tilesheet");
            var tileSheet = new TileSheet(tileSheetTexture);
            levelRenderer = new LevelRenderer(graphics, tileSheet);
            using (var importer = new LevelImporter("Content/levels.txt"))
            {
                levelCollection = importer.LoadLevels();
            }
            sceneStateManager = new SceneStateManager(levelCollection, levelRenderer);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            sceneStateManager.Advance(gameTime.ElapsedGameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            sceneStateManager.DrawScene(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
