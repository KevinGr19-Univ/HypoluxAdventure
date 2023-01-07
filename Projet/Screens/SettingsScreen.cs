using HypoluxAdventure.Core;
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
        private static Color BACKGROUND_COLOUR = new Color(30,30,30);

        private Sprite _titleSprite;
        private Vector2 _titlePosition;
        private float _titleRotation;
        private Vector2 _titleScale;

        public string InputLayoutChoice;
        private Button _inputButtonLayout;

        public SettingsScreen(Game1 game) : base(game)
        {

        }

        public override void LoadContent()
        {
            _titleSprite = new Sprite(Content.Load<Texture2D>("img/titleBackgroundImage"));
            _titlePosition = new Vector2(175,100);
            _titleRotation = 0;
            _titleScale = Vector2.One * 0.3f;


        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(SettingsScreen.BACKGROUND_COLOUR);
            _titleSprite.Draw(Game.UICanvas, _titlePosition, _titleRotation, _titleScale);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
