using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Items;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Rooms
{
    internal class Chest : GameObject
    {
        private AnimatedSprite _sprite;
        public RectangleF Hitbox { get; private set; }

        private bool _opened = false;

        public Chest(Game1 game, GameManager gameManager, Room room, Point pos) : base(game, gameManager)
        {
            // LOAD SPRITE ETC...
            Hitbox = new RectangleF(pos.ToVector2() * Room.TILE_SIZE + room.Position, new Vector2(Room.TILE_SIZE));
        }

        public override void Draw()
        {
            float yDepth = GameManager.GetYDepth(Hitbox.Center.Y);
            //_sprite.Draw(game.Canvas, Hitbox.Center, 0, Vector2.One); // Créer var _scale si besoin (idem pour exit)
            game.Canvas.FillRectangle(Hitbox, Color.Blue, yDepth); // DEBUG
        }

        public override void Update()
        {
            //_sprite.Update();
        }

        public void Open()
        {
            if (_opened) return;
            _opened = true;

            // PLAY ANIMATION
            DropItem dropItem = LootItem().ToDropItem(false, gameManager.Player.Position);
            gameManager.ItemManager.Summon(dropItem);
        }

        private Item LootItem()
        {
            // TODO: Faire item random
            return new Sword(game, gameManager);
        }
    }
}
