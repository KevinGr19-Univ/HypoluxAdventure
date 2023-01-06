using HypoluxAdventure.Core;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Screens
{
    internal class MenuScreen : AbstractScreen
    {
        private Sprite _title;
        private AnimatedSprite _hypoluxSprite;
        private AnimatedSprite _diaboluxEyes;

        public MenuScreen(Game1 game) : base(game) 
        {
            _title = game.Content.Load<Texture2D>("img/titleImage");
            
        }

        public override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BurlyWood);
        }

    }
}
