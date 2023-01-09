using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Projectiles
{
    internal class Bomb : Projectile
    {
        public const float SPEED = 650f;
        private const float DECEL_RATE = 1.4f;

        public override Vector2 HitboxSize => new Vector2(20);
        private AnimatedSprite AnimatedSprite => (AnimatedSprite)Sprite;

        public Bomb(Game1 game, GameManager gameManager, bool isPlayerProj, Vector2 pos) : base(game, gameManager, isPlayerProj, pos)
        {
            Sprite = new AnimatedSprite(game.Content.Load<SpriteSheet>("img/bombAnimation.sf", new JsonContentLoader()));
            Sprite.Depth = 0.8f;
            GraphicsUtils.SetPixelSize(Sprite, 20, 20, ref scale);
        }

        private float _explosionTime = 4;

        public override void Update()
        {
            base.Update();
            if (DoUpdate)
            {
                Velocity -= Velocity * DECEL_RATE * Time.DeltaTime;
                _explosionTime -= Time.DeltaTime;

                if (_explosionTime < 0) Explose();
            }

            AnimatedSprite.Update(Time.DeltaTime);
        }

        public override bool OnEntityCollision(Entity entity)
        {
            entity.Damage(6);
            Explose();
            return true;
        }

        public override void OnRoomCollision()
        {
            Explose();
        }

        private void Explose()
        {
            DoUpdate = false;
            SoundPlayer.PlaySound("sound/explosionSound");
            AnimatedSprite.Play("explode", () =>
            {
                Despawn();
            });
        }
    }
}
