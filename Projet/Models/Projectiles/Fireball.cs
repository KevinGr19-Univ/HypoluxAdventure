using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
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
    internal class Fireball : Projectile
    {
        public const int SIZE = 28;

        public override Vector2 HitboxSize => new Vector2(22);
        private AnimatedSprite AnimatedSprite => (AnimatedSprite)Sprite;

        public Fireball(Game1 game, GameManager gameManager, bool isPlayerProj, Vector2 pos) : base(game, gameManager, isPlayerProj, pos)
        {
            Sprite = new AnimatedSprite(game.Content.Load<SpriteSheet>("img/fireballAnimation.sf", new JsonContentLoader()));
            Sprite.Depth = 0.81f;
            GraphicsUtils.SetPixelSize(Sprite, SIZE, SIZE, ref scale);
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            SoundPlayer.PlaySound("sound/fireballSound");
            AnimatedSprite.Play("launched");
        }

        public override void Update()
        {
            base.Update();
            AnimatedSprite.Update(Time.DeltaTime);
            rotation = Velocity != Vector2.Zero ? -MathF.Atan2(Velocity.X, Velocity.Y) : 0;
        }

        public override bool OnEntityCollision(Entity entity)
        {
            entity.Damage(6);
            Extinguish();

            return false;
        }

        public override void OnRoomCollision()
        {
            Extinguish();
        }

        private void Extinguish()
        {
            DoUpdate = false;
            AnimatedSprite.Play("extinguish", () =>
            {
                Despawn();
            });
        }
    }
}
