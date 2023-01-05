using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using System;
using HypoluxAdventure.Core;
using HypoluxAdventure.Models;
using HypoluxAdventure.Screens;
using MonoGame.Extended;

namespace HypoluxAdventure
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; private set; }

        /// <summary>Canvas for UI elements (text, icons). Not affected by camera</summary>
        public SpriteBatch UICanvas { get; private set; }

        /// <summary>Canvas for world elements.</summary>
        public SpriteBatch Canvas { get; private set; }

        /// <summary>Canvas for background (images, tiles). Not affected by LayerDepth.</summary>
        public SpriteBatch BackgroundCanvas { get; private set; }
        public Camera Camera { get; private set; }

        private ScreenManager _screenManager;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic 
            Graphics.PreferredBackBufferWidth = Application.SCREEN_WIDTH;
            Graphics.PreferredBackBufferHeight = Application.SCREEN_HEIGHT;
            Graphics.ApplyChanges();

            TargetElapsedTime = TimeSpan.FromSeconds(1d / Application.TARGET_FRAMERATE);

            _screenManager = new ScreenManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Canvas = new SpriteBatch(GraphicsDevice);
            BackgroundCanvas = new SpriteBatch(GraphicsDevice);

            Camera = new Camera();
            UICanvas = new SpriteBatch(GraphicsDevice);

            LoadWorld();
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            Inputs.Update();

            _screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Time.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Camera.CalculateMatrix();

            UICanvas.Begin(
                samplerState: SamplerState.PointWrap,
                sortMode: SpriteSortMode.FrontToBack);

            Canvas.Begin(
                samplerState: SamplerState.PointWrap,
                transformMatrix: Camera.ViewMatrix,
                sortMode: SpriteSortMode.FrontToBack);

            BackgroundCanvas.Begin(
                samplerState: SamplerState.PointWrap,
                transformMatrix: Camera.ViewMatrix,
                sortMode: SpriteSortMode.Deferred);

            _screenManager.Draw(gameTime);

            BackgroundCanvas.End();
            Canvas.End();
            UICanvas.End();
            base.Draw(gameTime);
        }

        private void LoadScreen(AbstractScreen screen)
        {
            _screenManager.LoadScreen(screen, new FadeTransition(GraphicsDevice, Color.Black, 1));
        }

        public void LoadWorld()
        {
            WorldScreen worldScreen = new WorldScreen(this);
            LoadScreen(worldScreen);
        }

        public void LoadCredit()
        {
            CreditScreen creditScreen = new CreditScreen(this);
            LoadScreen(creditScreen);
        }
    }
}
