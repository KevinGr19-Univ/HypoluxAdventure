using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Projectiles;
using HypoluxAdventure.Models.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Items
{
    internal class KnifeItem : Item
    {
        public KnifeItem(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            Label = "COUTEAU";
        }

        public override float SlotScale => 4;
        protected override float distFromPlayer => 50;
        protected override float defaultOrientation => 0;
        protected override int pixelSize => 35;

        public override void OnShoot()
        {
            Throw();
        }

        public override void OnUse()
        {
            Throw();
        }

        public void Throw()
        {
            Room currentRoom = gameManager.RoomManager.CurrentRoom;
            Point pointPos = ((position - currentRoom.Position) / Room.TILE_SIZE).ToPoint();

            // Can't throw if knife spawn point in wall
            if(!currentRoom.IsWall(pointPos.X, pointPos.Y))
            {
                IsUsed = true;
                Knife knife = new Knife(game, gameManager, true, position);
                knife.Velocity = gameManager.Player.ShootDirection * Knife.SPEED;
                knife.Spawn();
            }
        }

        public override DropItem ToDropItem(bool startHover, Vector2 pos)
        {
            DropItem dropItem = new DropItem(game, gameManager, this, startHover);
            dropItem.CalculateHitbox(pos, new Vector2(24, 12));
            dropItem.SetTextureSize(24, 24);
            return dropItem;
        }

        protected override void LoadSprite()
        {
            Texture = game.Content.Load<Texture2D>("img/knife");
            sprite = new Sprite(Texture);
        }
    }
}
