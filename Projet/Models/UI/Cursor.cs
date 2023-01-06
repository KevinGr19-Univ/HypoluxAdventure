using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;

namespace HypoluxAdventure.Models
{
    internal class Cursor : GameObject
    {
        public Sprite Sprite { get; private set; }
        private Vector2 _position;
        private Vector2 _scale;

        public Cursor(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            Sprite = new Sprite(game.Content.Load<Texture2D>("img/targetPointer"));
            GraphicsUtils.SetPixelSize(Sprite, 32, 32, ref _scale);

            Sprite.Depth = 0.99f;
        }

        public override void Update()
        {
            _position = Inputs.MousePosition;
        }

        public override void Draw()
        {
            game.UICanvas.Draw(Sprite, _position, 0, _scale);
        }
    }
}
