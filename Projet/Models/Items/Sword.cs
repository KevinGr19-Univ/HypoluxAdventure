using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Monsters;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Items
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

        private const float SWEEP_ANGLE = 90;
        private const float ANIM_SWEEP_ANGLE = 75;
        private const float SWEEP_TIME = 0.3f;
        private const float SWEEP_RANGE = 85;

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

                localRotation = MathUtils.LerpInToPower(-ANIM_SWEEP_ANGLE, ANIM_SWEEP_ANGLE, SWEEP_TIME, _sweepTimer, 6);
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
            // Closest point of hitbox
            Vector2 DistanceFromHitbox(RectangleF hitbox, Vector2 pos)
            {
                float x = (float)Math.Clamp(pos.X, hitbox.Left, hitbox.Right);
                float y = (float)Math.Clamp(pos.Y, hitbox.Top, hitbox.Bottom);

                return new Vector2(x, y) - pos;
            }

            Player player = gameManager.Player;

            foreach(Monster monster in gameManager.RoomManager.CurrentRoom.GetAliveMonsters())
            {
                Vector2 diff = DistanceFromHitbox(monster.Hitbox, player.Position);
                float length = diff.Length();
                if (length > SWEEP_RANGE) continue;

                float angle = MathHelper.ToDegrees(MathF.Acos(diff.Dot(player.ShootDirection) / length));
                if (angle > -SWEEP_ANGLE && angle < SWEEP_ANGLE) monster.Damage(3);
            }
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

        protected override void LoadSprite()
        {
            Texture = game.Content.Load<Texture2D>("img/sword");
            sprite = new Sprite(Texture);
        }
    }
}
