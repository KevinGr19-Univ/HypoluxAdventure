﻿using HypoluxAdventure.Core;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Screens
{
    internal class SettingsScreen : AbstractScreen
    {
        private static Color BACKGROUND_COLOUR = new Color(30, 30, 30);

        private Sprite _titleSprite;
        private Vector2 _titlePosition;
        private float _titleRotation;
        private Vector2 _titleScale;

        private TextObject _name;
        private Button _inputButtonLayout;

        private Button _menuButton;
        private SpriteFont _menuFont;
        private Vector2 _textPositionCenter;

        private Button _qwerty;
        private Button _azerty;


        public SettingsScreen(Game1 game) : base(game)
        {

        }

        public override void LoadContent()
        {
            _titleSprite = new Sprite(Content.Load<Texture2D>("img/titleBackgroundImage"));
            _titlePosition = new Vector2(175, 100);
            _titleRotation = 0;
            _titleScale = Vector2.One * 0.3f;

            _menuFont = Content.Load<SpriteFont>("Font/MainMenuFont");
            _textPositionCenter = new Vector2(Application.SCREEN_WIDTH * 0.117f, Application.SCREEN_HEIGHT * 0.1f);
            _menuButton = new Button(Game, _menuFont, "MENU", new Vector2(_textPositionCenter.X+30, _textPositionCenter.Y), () => { Game.LoadMenu(); });
            ChangeButtonColor(_menuButton);

            _name = new TextObject(_menuFont, "CHANGER LES CONTROLES :", new Vector2(Application.SCREEN_WIDTH * 0.5f, Application.SCREEN_HEIGHT * 0.2f));

            _azerty = new Button(Game, _menuFont, "AZERTY", new Vector2(Application.SCREEN_WIDTH * 0.5f, Application.SCREEN_HEIGHT * 0.4f),() => {
                Inputs.ChangeInputLayout(Inputs.AZERTY);
                ChangeButtonColor(_azerty);
                IsUnactive(_qwerty);
            });
            _qwerty = new Button(Game, _menuFont, "QWERTY", new Vector2(Application.SCREEN_WIDTH * 0.5f, Application.SCREEN_HEIGHT * 0.6f), () => {
                Inputs.ChangeInputLayout(Inputs.QWERTY);
                ChangeButtonColor(_qwerty);
                IsUnactive(_azerty);
            });

            IsUnactive(_qwerty);
            ChangeButtonColor(_azerty);

            _qwerty.Depth = _azerty.Depth = 0.6f;
            _qwerty.Border = _azerty.Border = _menuButton.Border = 5;



        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(SettingsScreen.BACKGROUND_COLOUR);
            _titleSprite.Draw(Game.UICanvas, _titlePosition, _titleRotation, _titleScale);
            _menuButton.Draw();
            _name.Draw(Game.UICanvas);
            _qwerty.Draw();
            _azerty.Draw();

        }

        public override void Update(GameTime gameTime)
        {
            _menuButton.Update();
            _qwerty.Update();
            _azerty.Update();
        }

        private void ChangeButtonColor(Button button)
        {
            button.ColorNormal = new ButtonColor
            {
                TextColor = Color.White,
                BackgroundColor = Color.DarkGreen,
                BorderColor = Color.DarkOliveGreen
            };

            button.ColorHover = new ButtonColor
            {
                TextColor = Color.White,
                BackgroundColor = Color.GreenYellow,
                BorderColor = Color.Green
            };

            button.ColorDown = new ButtonColor
            {
                TextColor = Color.Black,
                BackgroundColor = Color.LightGray,
                BorderColor = Color.DarkGray
            };

        }
        private void IsUnactive(Button button)
        {
            button.ColorNormal = new ButtonColor
            {
                TextColor = Color.Black,
                BackgroundColor = Color.Crimson,
                BorderColor = Color.DarkRed
            };

            button.ColorHover = new ButtonColor
            {
                TextColor = Color.White,
                BackgroundColor = Color.GreenYellow,
                BorderColor = Color.Green
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
