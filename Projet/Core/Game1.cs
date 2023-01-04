using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using System;
using HypoluxAdventure.Core;
using HypoluxAdventure.Models;
using HypoluxAdventure.Screens;

namespace HypoluxAdventure
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; private set; }
        public SpriteBatch Canvas { get; private set; }
        public SpriteBatch UICanvas { get; private set; }
        public Camera Camera { get; private set; }

        private ScreenManager _screenManager;
        private Transition _screenTransition;

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
            _screenTransition = new FadeTransition(GraphicsDevice, Color.Black, 1);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Canvas = new SpriteBatch(GraphicsDevice);
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

            UICanvas.Begin(samplerState: SamplerState.PointWrap);
            Canvas.Begin(samplerState: SamplerState.PointWrap, transformMatrix: Camera.ViewMatrix);

            _screenManager.Draw(gameTime);

            Canvas.End();
            UICanvas.End();
            base.Draw(gameTime);
        }

        public void LoadWorld()
        {
            WorldScreen worldScreen = new WorldScreen(this);
            _screenManager.LoadScreen(worldScreen, _screenTransition);
        }
    }
}
