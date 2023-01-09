using HypoluxAdventure.Managers;
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
    internal class Potion : Item
    {
        public Potion(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            Label = "POTION";
        }

        public override float SlotScale => 7;

        protected override float distFromPlayer => 30;
        protected override float defaultOrientation => 0;
        protected override int pixelSize => 20;

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
            sprite.Effect = gameManager.Player.ShootDirection.X > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
        }

        public override void OnUse() 
        {
            if(gameManager.Player.Heal(9) > 0) IsUsed = true;
        }

        public override DropItem ToDropItem(bool startHover, Vector2 pos)
        {
            DropItem dropItem = new DropItem(game, gameManager, this, startHover);
            dropItem.CalculateHitbox(pos, new Vector2(22, 22));
            dropItem.SetTextureSize(24, 24);
            return dropItem;
        }

        protected override void LoadSprite()
        {
            Texture = game.Content.Load<Texture2D>("img/healingPotion");
            sprite = new Sprite(Texture);
        }

        public override void OnShoot() { }
        

    }
}
