using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Screens
{
    internal class GameOverScreen : AbstractScreen
    {
        private TextObject _floorText;
        private readonly int _reachedFloor;

        private AnimatedSprite _eyes;
        private Vector2 _eyesPosition;
        private float _eyesRotation;
        private Vector2 _eyesScale;

        private Button _menuButton;
        private SpriteFont _menuFont;
        private Vector2 _textPositionCenter;

        private Song _music;

        public GameOverScreen(Game1 game, int reachedFloor) : base(game)
        {
            _reachedFloor = reachedFloor;
        }

        public override void LoadContent()
        {
            _music = Content.Load<Song>("sound/itIsSadToGo");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.Play(_music);
            _menuFont = Content.Load<SpriteFont>("Font/MainMenuFont");
            _textPositionCenter = new Vector2(Application.SCREEN_WIDTH * 0.5f, Application.SCREEN_HEIGHT * 0.8f);
            _menuButton = new Button(Game, _menuFont, "MENU", new Vector2(_textPositionCenter.X, _textPositionCenter.Y), () =>
            {
                Game.LoadMenu(1);
            });
            _menuButton.Border = 5;
            ChangeButtonColor(_menuButton);

            SpriteSheet eyesSpriteSheet = Content.Load<SpriteSheet>("img/diaboluxEyesAnimation.sf", new JsonContentLoader());
            _eyes = new AnimatedSprite(eyesSpriteSheet);
            _eyes.Play("blink");
            _eyesPosition = Application.ScreenDimensions * 0.5f;
            _eyesRotation = 0;
            _eyesScale = new Vector2(30);

            Color floorTextColor = _reachedFloor == GameManager.FINAL_FLOOR ? Color.DarkRed : Color.Gray;
            Vector2 floorTextPos = new Vector2(Application.SCREEN_WIDTH * 0.5f, 100);

            _floorText = new TextObject(_menuFont, $"Étage atteint : {_reachedFloor}", floorTextPos, floorTextColor);
            _floorText.Scale = new Vector2(0.4f);
            _floorText.Depth = 1;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            MediaPlayer.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            _menuButton.Update();
            _eyes.Update(Time.DeltaTime*0.6f);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); // Color.Black
            _eyes.Draw(Game.UICanvas, _eyesPosition, _eyesRotation, _eyesScale);
            _floorText.Draw(Game.UICanvas);
            _menuButton.Draw();
        }

        private void ChangeButtonColor(Button button)
        {
            button.ColorNormal = new ButtonColor
            {
                TextColor = Color.White,
                BackgroundColor = Color.Firebrick,
                BorderColor = Color.DarkRed
            };

            button.ColorHover = new ButtonColor
            {
                TextColor = Color.White,
                BackgroundColor = Color.Crimson,
                BorderColor = Color.Red
            };

            button.ColorDown = new ButtonColor
            {
                TextColor = Color.Black,
                BackgroundColor = Color.LightGray,
                BorderColor = Color.DarkGray
            };
        }
    }
}
