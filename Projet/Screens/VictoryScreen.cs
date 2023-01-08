using HypoluxAdventure.Core;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Screens
{
    internal class VictoryScreen : AbstractScreen
    {

        private AnimatedSprite _sprite;
        private Vector2 _position;
        private float _rotation;
        private Vector2 _scale;
        public VictoryScreen(Game1 game) : base(game)
        {
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("img/hypoluxWinAnimation.sf", new JsonContentLoader());
            _sprite = new AnimatedSprite(spriteSheet);
            _sprite.Play("victory");
            _position = Application.ScreenDimensions * 0.5f;
            _scale = Vector2.One;
            _rotation = 0;
        }

        public override void Draw(GameTime gameTime)
        {
            _sprite.Draw(Game.UICanvas, _position, _rotation, _scale);
        }

        public override void Update(GameTime gameTime)
        {
            _sprite.Update(Time.DeltaTime);
        }
    }
}
