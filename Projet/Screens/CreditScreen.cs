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

        private float _lifetime = 38;

        private Song _music;
        public CreditScreen(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            _titleFont = Content.Load<SpriteFont>("Font/TitleCredit");
            _normalFont = Content.Load<SpriteFont>("Font/CreditFont");

            AddLine(_titleFont, "Crédits :", 2);
            AddLine(_normalFont,"Jeu créé par :",1);
            AddLine(_normalFont, "Kévin GRANDJEAN", 1);
            AddLine(_normalFont, "Mathieu ROSTAING", 1);
            AddLine(_normalFont,"Noa GUILLOT" ,2);

            AddLine(_normalFont,"Level Design fait par :",1);
            AddLine(_normalFont, "Kévin GRANDJEAN", 1);
            AddLine(_normalFont, "Mathieu ROSTAING", 1);
            AddLine(_normalFont, "Noa GUILLOT", 2);

            AddLine(_normalFont,"Direction artistique et graphismes conçuent par :",1);
            AddLine(_normalFont, "Kévin GRANDJEAN", 1);
            AddLine(_normalFont, "Mathieu ROSTAING", 1);
            AddLine(_normalFont, "Noa GUILLOT", 2);

            AddLine(_normalFont,"Sound design composé par :",1);
            AddLine(_normalFont,"Mathieu ROSTAING",3);

            AddLine(_titleFont, "Merci d'avoir joué.", 1);

            _music = Content.Load<Song>("sound/wanderingInTheDark");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.Play(_music);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            MediaPlayer.Stop();
        }

        public override void Update(GameTime gameTime)
        {
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
        }

        public void AddLine(SpriteFont font, string text, float time)
        {
            _textList.Add(new TextObject(font, text, _startpoint));
            _timeList.Add(time);
        }
        
    }
}

