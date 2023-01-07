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
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Item;
using System.IO;

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
            
            RoomManager.CreateTileFrames();

            LoadInputLayout();

            LoadMenu();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            SaveInputLayout();
        }

        private void LoadInputLayout()
        {
            if (!File.Exists(Inputs.INPUT_FILE_PATH)) File.WriteAllText(Inputs.INPUT_FILE_PATH, "0");

            string optionLine = File.ReadAllText(Inputs.INPUT_FILE_PATH);
            bool ok = int.TryParse(optionLine, out int option);

            if (ok)
            {
                switch (option)
                {
                    case 0:
                        Inputs.ChangeInputLayout(Inputs.AZERTY);
                        return;

                    case 1:
                        Inputs.ChangeInputLayout(Inputs.QWERTY);
                        return;
                }
            }

            File.WriteAllText(Inputs.INPUT_FILE_PATH, "0");
            Inputs.ChangeInputLayout(Inputs.AZERTY);
        }

        private void SaveInputLayout()
        {
            int id = 0;

            switch (Inputs.InputLayout.Name)
            {
                case "QWERTY":
                    id = 1;
                    break;

                // Could add more
            }

            File.WriteAllText(Inputs.INPUT_FILE_PATH, id.ToString());
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            Inputs.Update();

            // Screen fade and switch
            if (_load)
            {
                _fadeTimer += Time.RealDeltaTime;
                if (_fadeTimer >= _targetTime)
                {
                    _fadeTimer = _targetTime;
                    _load = false;
                    _screenManager.LoadScreen(screenToLoad);
                    screenToLoad = null;
                }
            }
            else if (_fadeTimer > 0) _fadeTimer -= Time.RealDeltaTime;

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

            _screenManager.Draw(gameTime); // FadeTransition was being drawn underneath all the other spriteBatches

            BackgroundCanvas.End();
            Canvas.End();

            if(_fadeTimer > 0)
            {
                Color fadeColor = new Color(_fadeColor, _fadeTimer / _targetTime);
                UICanvas.FillRectangle(0, 0, Application.SCREEN_WIDTH, Application.SCREEN_HEIGHT, fadeColor, 1); // Custom FadeTransition on top
            }

            UICanvas.End();

            base.Draw(gameTime);
        }

        private void LoadScreen(AbstractScreen screen, float transitionTime)
        {
            SetScreenToLoad(screen, Color.Black, transitionTime);
        }

        public void LoadWorld() => LoadScreen(new WorldScreen(this), 2);
        public void LoadCredit() => LoadScreen(new CreditScreen(this), 1);
        public void LoadMenu() => LoadScreen(new MenuScreen(this), 2);
        public void LoadGameOver() => LoadScreen(new GameOverScreen(this), 4);

        #region FadeTransition
        private float _targetTime, _fadeTimer;
        private Color _fadeColor;
        private bool _load;
        private Screen screenToLoad;

        private void SetScreenToLoad(Screen screen, Color fadeColor, float time)
        {
            if (_load) return;

            _fadeColor = fadeColor;
            _targetTime = time;
            _fadeTimer = 0;

            screenToLoad = screen;
            _load = true;
        }
        #endregion
    }
}
