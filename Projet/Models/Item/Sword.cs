using HypoluxAdventure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Item
{
    internal class Sword : Item
    {
        public Sword(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _texture = game.Content.Load<Texture2D>("img/sword");
            _sprite = new Sprite(_texture);
        }

        public override float SlotScale => 4;
        public override Vector2 StartingPoint => new Vector2(1, -1);
    }
}
