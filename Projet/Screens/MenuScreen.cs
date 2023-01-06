using HypoluxAdventure.Core;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
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


        public MenuScreen(Game1 game) : base(game) 
        {
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
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_backgroundColour);
            _title.Draw(Game.UICanvas, _titlePosition, _titleRotation, _titleScale);
            _hypoluxSprite.Draw(Game.UICanvas, _hypoluxPosition, _hypoluxRotation, _hypoluxScale);
            _diaboluxEyes.Draw(Game.UICanvas, _diaboluxPosition, _diaboluxRotation, _diaboluxScale);

        }

    }
}
