using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Projectiles
{
    internal class Bullet : Projectile
    {
        public const float MIN_START_SPEED = 700f;
        public const float MAX_START_SPEED = 1000f;

        private const float DECEL_RATE = 2.5f;
        private const float MIN_SPEED = 100f;

        private const float MIN_DAMAGE = 1;
        private const float MAX_DAMAGE = 3;

        public Bullet(Game1 game, GameManager gameManager, bool isPlayerProj, Vector2 pos) : base(game, gameManager, isPlayerProj, pos)
        {
            Sprite = new Sprite(game.Content.Load<Texture2D>("img/bullet"));
            Sprite.Depth = 0.8f;
        }

        public override Vector2 HitboxSize => new Vector2(8);

        public override void Update()
        {
            base.Update();
            if (Velocity.LengthSquared() < MIN_SPEED * MIN_SPEED) Despawn();
            else Velocity -= Velocity * DECEL_RATE * Time.DeltaTime;
        }

        public override bool OnEntityCollision(Entity entity)
        {
            float speedLerp = MathF.Pow(Velocity.Length() / MAX_START_SPEED, 3);
            int damage = (int)Math.Round(MathUtils.Lerp(MIN_DAMAGE, MAX_DAMAGE, speedLerp));

            entity.Damage(damage);
            Despawn();

            return false;
        }

        public override void OnRoomCollision()
        {
            Despawn();
        }
    }
}
