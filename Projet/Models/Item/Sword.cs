using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
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
            Label = "ÉPÉE";
        }

        public override float Cooldown => 0.4f;
        public override float SlotScale => 4;

        protected override float distFromPlayer => 55;
        protected override float defaultOrientation => 45;
        protected override int pixelSize => 40;

        private const float SWEEP_ANGLE = 75;
        private const float SWEEP_TIME = 0.4f;

        private float _sweepTimer;

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
            ProcessAnimation();
        }

        private void ProcessAnimation()
        {
            if (_sweepTimer > 0)
            {
                UpdateRotation = false;
                IsLocked = true;

                localRotation = MathUtils.LerpInCubic(-SWEEP_ANGLE, SWEEP_ANGLE, SWEEP_TIME, _sweepTimer);
                sprite.Color = Color.Red;
                _sweepTimer -= Time.DeltaTime;
            }
            else
            {
                UpdateRotation = true;
                IsLocked = false;

                localRotation = 0;
                sprite.Color = Color.White;
            }
        }

        public override void OnShoot()
        {
            TriggerCooldown();
            DamageMonsters();
            _sweepTimer = SWEEP_TIME;
        }

        private void DamageMonsters()
        {

        }

        public override void OnUse() { }

        public override void OnDrop()
        {
            base.OnDrop();
            _sweepTimer = 0;
        }

        public override DropItem ToDropItem(bool startHover, Vector2 pos)
        {
            DropItem dropItem = new DropItem(game,gameManager, this, startHover);
            dropItem.CalculateHitbox(pos, new Vector2(22,22));
            dropItem.SetTextureSize(24,24);
            return dropItem;
        }

        protected override Texture2D LoadTexture()
        {
            return game.Content.Load<Texture2D>("img/sword");
        }
    }
}
