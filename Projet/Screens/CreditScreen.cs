using HypoluxAdventure.Core;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Screens
{
    internal class CreditScreen : AbstractScreen
    {
        private SpriteFont _titleFont;
        private SpriteFont _normalFont;

        private List<TextObject> _textList = new List<TextObject>();
        private List<TextObject> _textListActif = new List<TextObject>(0);
        private List<float> _timeList = new List<float>(); 
        private static Vector2 _startpoint = new Vector2(Application.SCREEN_WIDTH * 0.5f, Application.SCREEN_HEIGHT + 50);

        private int _textListStart = 0;
        private float _timer = 0;
        private const float MOVE_SPEED = 75;

        private float _lifetime = 30;

        private Song _music;
        private bool _isQuitting = false;

        private Button _menuButton;
        private SpriteFont _menuFont;
        private Vector2 _textPositionCenter;
        public CreditScreen(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            _titleFont = Content.Load<SpriteFont>("Font/TitleCredit");
            _normalFont = Content.Load<SpriteFont>("Font/CreditFont");

            //AddLine(_titleFont,"Crédits :", 1);
            AddLine(_titleFont,"Jeu créé par",1);
            AddLine(_normalFont, "Kévin GRANDJEAN", 1);
            AddLine(_normalFont, "Mathieu ROSTAING", 1);
            AddLine(_normalFont, "Noa GUILLOT" ,2);

            AddLine(_normalFont,"Level Design",1);
            AddLine(_normalFont, "Kévin GRANDJEAN", 1);
            AddLine(_normalFont, "Mathieu ROSTAING", 1);
            AddLine(_normalFont, "Noa GUILLOT", 1);
            AddLine(_normalFont, "IA aléatoire", 2);

            AddLine(_normalFont,"Direction artistique et graphismes",1);
            AddLine(_normalFont, "Mathieu ROSTAING", 1);
            AddLine(_normalFont, "Noa GUILLOT", 2);

            AddLine(_normalFont,"Sound design",1);
            AddLine(_normalFont, "Mathieu ROSTAING",3);

            AddLine(_titleFont, "Merci d'avoir joué.", 1);

            _music = Content.Load<Song>("sound/wanderingInTheDark");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.Play(_music);

            _menuFont = Content.Load<SpriteFont>("Font/MainMenuFont");
            _textPositionCenter = new Vector2(Application.SCREEN_WIDTH * 0.9f, Application.SCREEN_HEIGHT * 0.9f);
            _menuButton = new Button(Game, _menuFont, "SKIP", new Vector2(_textPositionCenter.X, _textPositionCenter.Y), () =>
            {
                Game.LoadMenu(1);
                _isQuitting = true;
            });
            _menuButton.Border = 5;
            ChangeButtonColor(_menuButton);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            MediaPlayer.Stop();

        }

        public override void Update(GameTime gameTime)
        {
            if(!_isQuitting) _menuButton.Update();
            if(_lifetime > 0)
            {
                _lifetime -= Time.DeltaTime;
                if (_lifetime < 0) Game.LoadMenu(2);
            }

            if(_textListStart < _textListActif.Count)
            {
                for (int i = _textListStart; i < _textListActif.Count; i++)
                {
                    TextObject line = _textListActif[i];

                    line.Position.Y -= MOVE_SPEED * Time.DeltaTime;
                    if (line.Position.Y < -100) _textListStart++;
                }
            }

            if (_textList.Count <= _textListActif.Count) return;
            _timer -= Time.DeltaTime;

            if (_timer <= 0)
            {
                _timer = _timeList[_textListActif.Count];
                _textListActif.Add(_textList[_textListActif.Count]);
            }

        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (_textListStart < _textListActif.Count)
            {
                for (int i = _textListStart; i < _textListActif.Count; i++)
                {
                    TextObject line = _textListActif[i];
                    line.Draw(Game.UICanvas);
                }
            }
            _menuButton.Draw();
        }

        public void AddLine(SpriteFont font, string text, float time)
        {
            _textList.Add(new TextObject(font, text, _startpoint));
            _timeList.Add(time);
        }
        private void ChangeButtonColor(Button button)
        {
            button.ColorNormal = new ButtonColor
            {
                TextColor = Color.White,
                BackgroundColor = Color.DarkGoldenrod,
                BorderColor = Color.Gold
            };

            button.ColorHover = new ButtonColor
            {
                TextColor = Color.White,
                BackgroundColor = Color.Goldenrod,
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

