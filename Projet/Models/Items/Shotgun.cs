using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Projectiles;
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
    internal class Shotgun : Item
    {
        public Shotgun(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            Label = "FUSIL À POMPE";
        }

        public override float Cooldown => 5f;

        public override float SlotScale => 3.8f;
        protected override float distFromPlayer => 50;
        protected override float defaultOrientation => 0;
        protected override int pixelSize => 40;

        private const int SHOOT_ANGLE = 15;
        private const int BULLET_COUNT = 5;

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
            sprite.Effect = gameManager.Player.ShootDirection.X > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
        }

        public override void OnShoot()
        {
            TriggerCooldown();
            Random r = new Random();
            float centerAngle = MathF.Atan2(gameManager.Player.ShootDirection.Y, gameManager.Player.ShootDirection.X);

            for(int i = 0; i < BULLET_COUNT; i++)
            {
                float offsetAngle = MathHelper.ToRadians(MathUtils.Lerp(-SHOOT_ANGLE, SHOOT_ANGLE, r.NextSingle()));
                float angle = centerAngle + offsetAngle;

                float speed = MathUtils.Lerp(Bullet.MIN_START_SPEED, Bullet.MAX_START_SPEED, r.NextSingle());
                Vector2 velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;

                Bullet bullet = new Bullet(game, gameManager, true, position);
                bullet.Velocity = velocity;
                bullet.Spawn();
            }

            SoundPlayer.PlaySound("sound/shotgunSound", 0.7f);
        }

        public override void OnUse() { }

        public override DropItem ToDropItem(bool startHover, Vector2 pos)
        {
            DropItem dropItem = new DropItem(game, gameManager, this, startHover);
            dropItem.CalculateHitbox(pos, new Vector2(16, 24));
            dropItem.SetTextureSize(24, 24);
            return dropItem;
        }

        protected override void LoadSprite()
        {
            Texture = game.Content.Load<Texture2D>("img/Shotgun");
            sprite = new Sprite(Texture);
        }
    }
}
