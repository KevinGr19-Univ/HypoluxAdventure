using HypoluxAdventure.Core;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        private float _timer = 0;
        private const float MOVE_SPEED = 75;

        public CreditScreen(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            _titleFont = Content.Load<SpriteFont>("Font/TitleCredit");
            _normalFont = Content.Load<SpriteFont>("Font/CreditFont");

            AddLines(_titleFont, "Crédits : ");
            AddLines(_normalFont,
                "Jeu créé par :",
                "Mathieu 'HazelSoul' ROSTAING",
                "Kévin 'FrancePVP' GRANDJEAN",
                "Noa 'ShakraSasukeXxxD4RK_Sn4keX223xxXNaruto' GUILLOT" ,
                "Level Design fait par :",
                "Mathieu 'HazelSoul' ROSTAING",
                "Kévin 'FrancePVP' GRANDJEAN",
                "Noa 'ShakraSasukeXxxD4RK_Sn4keX223xxXNaruto' GUILLOT" ,
                "Direction artistique et graphismes conçuent par :",
                "Mathieu 'HazelSoul' ROSTAING (Majeure partie)",
                "Kévin 'FrancePVP' GRANDJEAN",
                "Noa 'ShakraSasukeXxxD4RK_Sn4keX223xxXNaruto' GUILLOT" ,
                "Sound design composé par :",
                "Mathieu 'HazelSoul' ROSTAING",
                "Kévin 'FrancePVP' GRANDJEAN",
                "Noa 'ShakraSasukeXxxD4RK_Sn4keX223xxXNaruto' GUILLOT" ,
                "-... --- -... .. -. ..- ... ",
                "-.. .-. .- --. --- -. ..- ...");

        }

        public override void Update(GameTime gameTime)
        {
            Logger.Debug(_timer);
            foreach(TextObject line in _textListActif) line.Position.Y -= MOVE_SPEED * Time.DeltaTime;

            if (_textList.Count <= _textListActif.Count) return;
            _timer -= Time.DeltaTime;

            if (_timer <= 0)
            {
                _textListActif.Add(_textList[_textListActif.Count]);
                //_timer = TIME_BETWEEN_TEXTS;
            }

        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            foreach(TextObject line in _textListActif)
            {
                line.Draw(Game.UICanvas);
            }
      
        }

        public void AddLines(SpriteFont font, params string[] lines)
        {
            foreach (string line in lines) _textList.Add(new TextObject(font, line, _startpoint));
        }
        
    }
}
