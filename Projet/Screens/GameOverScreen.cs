using HypoluxAdventure.Core;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Screens
{
    internal class GameOverScreen : AbstractScreen
    {
        private AnimatedSprite _eyes;
        private Vector2 _eyesPosition;
        private float _eyesRotation;
        private Vector2 _eyesScale;

        public GameOverScreen(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            SpriteSheet eyesSpriteSheet = Content.Load<SpriteSheet>("img/diaboluxEyesAnimation.sf", new JsonContentLoader());
            _eyes = new AnimatedSprite(eyesSpriteSheet);
            _eyes.Play("blink");
            _eyesPosition = Application.ScreenDimensions * 0.5f;
            _eyesRotation = 0;
            _eyesScale = new Vector2(30,30);
        }

        public override void Update(GameTime gameTime)
        {
            _eyes.Update(Time.DeltaTime*0.6f);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); // Color.Black
            _eyes.Draw(Game.UICanvas, _eyesPosition, _eyesRotation, _eyesScale);
        }
    }
}
