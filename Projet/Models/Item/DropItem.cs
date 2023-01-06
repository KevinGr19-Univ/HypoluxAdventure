using HypoluxAdventure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Item
{
    internal class DropItem : GameObject
    {

        private Texture2D _texture;
        private Item _item;

        public Rectangle Hitbox;
        private bool _startHover, _hover;


        public DropItem(Game1 game, GameManager gameManager, Item item) : base(game, gameManager)
        {
            _item = item;
        }

        public override void Draw()
        {
            // Affichage Canvas (depth = 0.2f)
            game.UICanvas.Draw(_texture, gameManager.Player.Position, null, Color.White, 0, Vector2.Zero, new Vector2(30,30), SpriteEffects.None, 0.02f);
        }

        public override void Update()
        {
            if (_startHover && !_hover) _startHover = false;
            if (!_startHover && _hover) if (gameManager.InventoryManager.AddItem(_item)) gameManager.ItemManager.Despawn(this);
            // Collision avec joueur (Kévin)
        }
    }
}
