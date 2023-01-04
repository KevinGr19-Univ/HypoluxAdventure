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
    internal class CreditScreen : GameScreen
    {
        private SpriteFont _titleFont;
        private SpriteFont _normalFont;
        private TextObject _title;

        private List<TextObject> _textList = new List<TextObject>();
        private static Vector2 _startpoint = new Vector2(Application.SCREEN_WIDTH * 0.5f, Application.SCREEN_HEIGHT + 100);

        private float _timer = 0;
        private const float TIME_BETWEEN_TEXTS = 3;
        private const float MOVE_SPEED = 75;

        public CreditScreen(Game game) : base(game) { }

        public override void LoadContent()
        {
            _titleFont = Content.Load<SpriteFont>("Font/TitleCredit.spritefont");
            _normalFont = Content.Load<SpriteFont>("Font/CreditFont.spritefont");
        }

        public override void Update(GameTime gameTime)
        {
            //_textList[0] = new TextObject(_titleFont, "Crédits : ", _startpoint);
            //_textList[1] = new TextObject(_normalFont, "Jeu créé par :\nMathieu 'HazelSoul' ROSTAING\nKévin 'FrancePVP' GRANDJEAN\nNoa 'ShakraSasukeXxxD4RK_Sn4keX223xxXNaruto' GUILLOT", _startpoint2);
            //_textList[2] = new TextObject(_normalFont, "Level Design fait par :\nMathieu 'HazelSoul' ROSTAING\nKévin 'FrancePVP' GRANDJEAN\nNoa 'ShakraSasukeXxxD4RK_Sn4keX223xxXNaruto' GUILLOT", _startpoint3);
            //_textList[3] = new TextObject(_normalFont, "Direction artistique et graphismes conçuent par:\nMathieu 'HazelSoul' ROSTAING (carry)\nKévin 'FrancePVP' GRANDJEAN\nNoa 'ShakraSasukeXxxD4RK_Sn4keX223xxXNaruto' GUILLOT", _startpoint4);
            //_textList[4] = new TextObject(_normalFont, "Sound Design ")
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

        }

        public void AddLines(SpriteFont font, params string[] lines)
        {
            foreach (string line in lines) _textList.Add(new TextObject(font, line, _startpoint));
        }
        
    }
}
