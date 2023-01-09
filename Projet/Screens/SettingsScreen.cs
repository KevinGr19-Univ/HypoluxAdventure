using HypoluxAdventure.Core;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    internal class SettingsScreen : AbstractScreen
    {
        private static Color BACKGROUND_COLOUR = new Color(30, 30, 30);

        private Sprite _titleSprite;
        private Vector2 _titlePosition;
        private float _titleRotation;
        private Vector2 _titleScale;

        private TextObject _name;

        private Button _menuButton;
        private SpriteFont _menuFont;
        private Vector2 _textPositionCenter;

        private Button _qwerty;
        private Button _azerty;
        private Button _credit;

        private TextObject[] _text;

        private Texture2D _key;
        private SpriteSheet _shoot;
        private SpriteSheet _use;
        private Texture2D _changeSlot1;
        private Texture2D _changeSlot2;
        private Texture2D _stop;
        private Texture2D _tab;

        private bool _contole = true;

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
            _menuButton = new Button(Game, _menuFont, "MENU", new Vector2(_textPositionCenter.X+30, _textPositionCenter.Y), () => 
            { 
                Game.LoadMenu();
                _contole = false;
            });
            ChangeButtonColor(_menuButton);

            _text = new TextObject[]
            {
                new TextObject(_menuFont, "CONTROLES :", new Vector2(Application.SCREEN_WIDTH*0.2f, Application.SCREEN_HEIGHT*0.3f)),
                new TextObject(_menuFont, "CHANGER LES CONTROLES :", new Vector2(Application.SCREEN_WIDTH * 0.7f, Application.SCREEN_HEIGHT * 0.3f)),
                new TextObject(_menuFont, "BOUGER :", new Vector2(Application.SCREEN_WIDTH*0.2f, Application.SCREEN_HEIGHT*0.4f)),
                new TextObject(_menuFont, "TAPER/TIRER :", new Vector2(Application.SCREEN_WIDTH*0.2f, Application.SCREEN_HEIGHT*0.5f)),
                new TextObject(_menuFont, "UTILISER :", new Vector2(Application.SCREEN_WIDTH*0.2f, Application.SCREEN_HEIGHT*0.6f)),
                new TextObject(_menuFont, "CHANGER DE SLOT :", new Vector2(Application.SCREEN_WIDTH*0.2f, Application.SCREEN_HEIGHT*0.7f)),
                new TextObject(_menuFont, "PAUSE :", new Vector2(Application.SCREEN_WIDTH*0.2f, Application.SCREEN_HEIGHT*0.8f)),
                new TextObject(_menuFont, "POSER UN ITEM :", new Vector2(Application.SCREEN_WIDTH*0.2f, Application.SCREEN_HEIGHT*0.9f))
            };

            
            _shoot = Game.Content.Load<SpriteSheet>("img/leftClickAnimation.sf", new JsonContentLoader());
            _use = Game.Content.Load<SpriteSheet>("img/rightClickAnimation.sf", new JsonContentLoader());
            _changeSlot1 = Game.Content.Load<Texture2D>("img/slotsKeys");

            _stop = Game.Content.Load<Texture2D>("img/spaceKey");
            _tab = Game.Content.Load<Texture2D>("img/tabKey");

            _azerty = new Button(Game, _menuFont, "AZERTY", new Vector2(Application.SCREEN_WIDTH * 0.7f, Application.SCREEN_HEIGHT * 0.5f),() => {
                Inputs.ChangeInputLayout(Inputs.AZERTY);
                ChangeButtonColor(_azerty);
                IsUnactive(_qwerty);
            });
            _qwerty = new Button(Game, _menuFont, "QWERTY", new Vector2(Application.SCREEN_WIDTH * 0.7f, Application.SCREEN_HEIGHT * 0.7f), () => {
                Inputs.ChangeInputLayout(Inputs.QWERTY);
                ChangeButtonColor(_qwerty);
                IsUnactive(_azerty);
            });

            _credit = new Button(Game, _menuFont, "CREDITS", new Vector2(Application.SCREEN_WIDTH - (float)_menuFont.MeasureString("CREDIT").X * 0.8f, _textPositionCenter.Y), () => { Game.LoadCredit(); });

            
            if (Inputs.InputLayout.Name == "AZERTY")
            {
                IsUnactive(_qwerty);
                ChangeButtonColor(_azerty);
                _key = Game.Content.Load<Texture2D>("img/azertyLayoutKeys");
            }
            else
            {
                IsUnactive(_azerty);
                ChangeButtonColor(_qwerty);
                _key = Game.Content.Load<Texture2D>("img/qwertyLayoutKeys");
            }

            
            ChangeButtonColor(_credit);

            _qwerty.Depth = _azerty.Depth = _menuButton.Depth = _credit.Depth = 0.6f;
            _qwerty.Border = _azerty.Border = _menuButton.Border = _credit.Border = 5;
            _text[0].Scale = _text[1].Scale = new Vector2(0.7f);
            
            for (int i = 2; i<_text.Length; i++)
                _text[i].Scale = new Vector2(0.5f);


        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(SettingsScreen.BACKGROUND_COLOUR);
            _titleSprite.Draw(Game.UICanvas, _titlePosition, _titleRotation, _titleScale);
            _menuButton.Draw();
            _qwerty.Draw();
            _azerty.Draw();
            _credit.Draw();
            foreach (TextObject text in _text)
            {
                text.Draw(Game.UICanvas);
            }

        }

        public override void Update(GameTime gameTime)
        {
            if (_contole)
            {
                _menuButton.Update();
                _qwerty.Update();
                _azerty.Update();
                _credit.Update();
            }
            
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
