using HypoluxAdventure.Core;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HypoluxAdventure.Screens
{
    internal class VictoryScreen : AbstractScreen
    {

        private AnimatedSprite _sprite;
        private Vector2 _position;
        private float _rotation;
        private Vector2 _scale;

        private Button _menuButton;
        private SpriteFont _menuFont;
        private Vector2 _textPositionCenter;
        private SoundEffect _clickedSound;
        public VictoryScreen(Game1 game) : base(game)
        {
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("img/hypoluxWinAnimation.sf", new JsonContentLoader());
            _sprite = new AnimatedSprite(spriteSheet);
            _sprite.Play("victory");
            _position = Application.ScreenDimensions * 0.5f;
            _scale = Vector2.One * 4;
            _rotation = 0;

            _textPositionCenter = new Vector2 (Application.ScreenDimensions.X * 0.5f, Application.ScreenDimensions.Y *0.8f);
            _menuFont = Content.Load<SpriteFont>("Font/MainMenuFont");
            _clickedSound = Content.Load<SoundEffect>("sound/buttonSound");
            _menuButton = new Button(Game, _menuFont, "MENU", new Vector2(_textPositionCenter.X, _textPositionCenter.Y), () => { _clickedSound.Play(); Game.LoadMenu(); });
            ChangeButtonColor(_menuButton);
        }

        public override void Draw(GameTime gameTime)
        {
            _sprite.Draw(Game.UICanvas, _position, _rotation, _scale);
            _menuButton.Draw();
        }

        public override void Update(GameTime gameTime)
        {
            _sprite.Update(Time.DeltaTime);
            _menuButton.Update();
        }

        private void ChangeButtonColor(Button button)
        {
            button.ColorNormal = new ButtonColor
            {
                TextColor = Color.White,
                BackgroundColor = Color.Gold,
                BorderColor = Color.Goldenrod
            };

            button.ColorHover = new ButtonColor
            {
                TextColor = Color.White,
                BackgroundColor = Color.DarkGoldenrod,
                BorderColor = Color.PaleGoldenrod
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
