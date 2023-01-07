using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace HypoluxAdventure.Screens
{
    internal class MenuScreen : AbstractScreen
    {
        private Sprite _title;
        private Vector2 _titlePosition;
        private float _titleRotation;
        private Vector2 _titleScale;

        private Color _backgroundColour;

        private Sprite _hypoluxSprite;
        private Vector2 _hypoluxPosition;
        private float _hypoluxRotation;
        private Vector2 _hypoluxScale;

        private AnimatedSprite _diaboluxEyes;
        private Vector2 _diaboluxPosition;
        private float _diaboluxRotation;
        private Vector2 _diaboluxScale;

        private Button _play;
        private Button _quit;
        private Button _settings;

        private SpriteFont _menuFont;
        private Vector2 _textPositionCenter;

        public MenuScreen(Game1 game) : base(game) 
        {

            _menuFont = Content.Load<SpriteFont>("Font/MainMenuFont");
            _textPositionCenter = new Vector2(Application.SCREEN_WIDTH * 0.5f, Application.SCREEN_HEIGHT * 0.9f);

            _play = new Button(game, _menuFont, "JOUER", new Vector2(_textPositionCenter.X + 30, _textPositionCenter.Y), () => { Game.LoadWorld(); });
            _quit = new Button(game, _menuFont, "QUITTER", new Vector2(_textPositionCenter.X+400, _textPositionCenter.Y), () => { Game.Exit(); });
            _settings = new Button(game, _menuFont, "PARAMETRES", new Vector2(_textPositionCenter.X-400, _textPositionCenter.Y), () => { Game.LoadCredit(); });
            
            _play.Depth = _quit.Depth = _settings.Depth = 0.6f;
            _play.Border = _quit.Border = _settings.Border = 5;
            _play.Padding = _quit.Padding = _settings.Padding = new Vector2(10);

            ChangeButtonColor(_play);
            ChangeButtonColor(_quit);
            ChangeButtonColor(_settings);


        }

        public override void LoadContent()
        {
            _title = new Sprite(Content.Load<Texture2D>("img/titleImage"));
            _titlePosition = Application.ScreenDimensions * 0.5f;
            _titleRotation = 0;
            _titleScale = Vector2.One;

            _backgroundColour = new Color(75, 75, 46);

            _hypoluxSprite = new Sprite(Content.Load<Texture2D>("img/perso"));
            _hypoluxPosition = new Vector2(225,199);
            _hypoluxRotation = 0;
            _hypoluxScale = new Vector2(2.3f,2.3f);

            SpriteSheet diaboluxEyesSpriteSheet = Content.Load<SpriteSheet>("img/diaboluxEyesAnimation.sf", new JsonContentLoader());
            _diaboluxEyes = new AnimatedSprite(diaboluxEyesSpriteSheet);
            _diaboluxEyes.Play("blink");
            _diaboluxPosition = new Vector2(550, 500);
            _diaboluxRotation = 0;
            _diaboluxScale = new Vector2(3f, 3f);


        }

        public override void Update(GameTime gameTime)
        {
            _diaboluxEyes.Update(Time.DeltaTime);

            _play.Update();
            _quit.Update();
            _settings.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_backgroundColour);
            _title.Draw(Game.UICanvas, _titlePosition, _titleRotation, _titleScale);
            _hypoluxSprite.Draw(Game.UICanvas, _hypoluxPosition, _hypoluxRotation, _hypoluxScale);
            _diaboluxEyes.Draw(Game.UICanvas, _diaboluxPosition, _diaboluxRotation, _diaboluxScale);

            _play.Draw();
            _quit.Draw();
            _settings.Draw();

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
    }
}
