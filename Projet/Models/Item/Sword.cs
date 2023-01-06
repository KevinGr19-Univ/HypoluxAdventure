using HypoluxAdventure.Core;
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
            Texture = game.Content.Load<Texture2D>("img/sword");
            Label = "ÉPÉE";
        }

        public override float SlotScale => 4;
        public override Vector2 DefaultDirection => new Vector2(1, 1);
        public override float Cooldown => 2f;
        public override float DistFromPlayer => 40;

        private float _timer = 3;

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
        }

        public override void OnShoot()
        {
            Logger.Debug("Hit");
            TriggerCooldown();
        }

        public override void OnUse() { }

        public override DropItem ToDropItem(bool startHover, Vector2 pos)
        {
            DropItem dropItem = new DropItem(game,gameManager, this, startHover);
            dropItem.CalculateHitbox(pos, new Vector2(22,22));
            dropItem.SetTextureSize(24,24);
            return dropItem;
        }
    }
}
