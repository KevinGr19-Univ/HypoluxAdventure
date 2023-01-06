using HypoluxAdventure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Item
{
    internal class DropItem : GameObject
    {
        public static readonly Vector2 ORIGIN = new Vector2(0.5f);

        private Texture2D _texture;
        private Vector2 _scale = Vector2.One;
        private Item _item;

        public Vector2 Position { get; private set; }
        public RectangleF Hitbox { get; private set; }
        private bool _startHover, _hover;


        public DropItem(Game1 game, GameManager gameManager, Item item, bool startHover) : base(game, gameManager)
        {
            _item = item;
            _texture = _item.Texture;
            _startHover = startHover;
        }

        public override void Draw()
        {
            // Affichage Canvas (depth = 0.2f)
            game.Canvas.Draw(_texture, Position, null, Color.White, 0, ORIGIN, _scale, SpriteEffects.None, 0.2f);
        }

        public override void Update()
        {
            _hover = Hitbox.Intersects(gameManager.Player.Hitbox);

            if (_startHover && !_hover) _startHover = false;
            if (!_startHover && _hover) if (gameManager.InventoryManager.AddItem(_item)) gameManager.ItemManager.Despawn(this);
        }

        public void CalculateHitbox (Vector2 pos, Vector2 size)
        {
            Position = pos;
            Hitbox = new RectangleF(pos-size*0.5f, size);
        }

        public void SetTextureSize(int width, int height)
        {
            _scale.X = (float)width/_texture.Width;
            _scale.Y = (float)height / _texture.Height;
        }
    }
}
