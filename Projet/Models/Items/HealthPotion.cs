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
    internal class HealthPotion : Item
    {
        public HealthPotion(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            Label = "Potion de vie";
        }

        public override float SlotScale => 4;

        protected override float distFromPlayer => 20;
        protected override float defaultOrientation => 0;
        protected override int pixelSize => 20;

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
        }

        public override void OnUse() 
        {
            IsUsed = true;
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

        public override void OnShoot()
        {
            gameManager.Player.Heal(10);
        }
        

    }
}
