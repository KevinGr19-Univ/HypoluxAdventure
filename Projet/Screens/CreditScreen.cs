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
        private TextObject[] _textList = new TextObject[10];
        public Vector2 startpoint = new Vector2(Application.SCREEN_WIDTH * 0.5f, Application.SCREEN_HEIGHT + 100); 
        //public float 

        public CreditScreen(Game game) : base(game) { }

        public override void LoadContent()
        {
            _titleFont = Content.Load<SpriteFont>("Font/TitleCredit.spritefont");
            _normalFont = Content.Load<SpriteFont>("Font/CreditFont.spritefont");
        }

        public override void Update(GameTime gameTime)
        {
            _textList[0] = new TextObject(_titleFont, "Crédits : ", startpoint);
            //_textList[1] = new TextObject(_normalFont, "Jeu créé par Mathieu 'HazelSoul' ROSTAING, Kévin 'FrancePVP' GRANDJEAN, Noa 'ShakraSasukeXxxD4RK_Sn4keX223xxXNaruto' GUILLOT");
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

        }
        
    }
}
