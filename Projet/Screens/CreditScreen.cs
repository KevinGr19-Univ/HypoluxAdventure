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

        public CreditScreen(Game game) : base(game) { }

        public override void LoadContent()
        {
            _titleFont = Content.Load<SpriteFont>("Font/TitleCredit.spritefont");
            _normalFont = Content.Load<SpriteFont>("Font/CreditFont.spritefont");
        }

        public override void Update(GameTime gameTime)
        {
            _titleFont.MeasureString()
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

        }
        
    }
}
