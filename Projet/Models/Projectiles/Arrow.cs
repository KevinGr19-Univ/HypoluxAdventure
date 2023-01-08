using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
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

namespace HypoluxAdventure.Models.Projectiles
{
    internal class Arrow : Projectile
    {
        public const float SPEED = 1000f;
        public override Vector2 HitboxSize => new Vector2(8);

        public Arrow(Game1 game, GameManager gameManager, bool isPlayerProj, Vector2 pos) : base(game, gameManager, isPlayerProj, pos)
        {
            Sprite = new Sprite(game.Content.Load<Texture2D>("img/arrow"));
            Sprite.Origin = new Vector2(13, 7);
            Sprite.Depth = 0.8f;
            GraphicsUtils.SetPixelSize(Sprite, 40, 40, ref scale);
        }

        private const float FADE_TIME = 0.3f;
        private float _fadeTimer;

        public override void Update()
        {
            base.Update();
            rotation = Velocity != Vector2.Zero ? -MathF.Atan2(Velocity.X, Velocity.Y) + MathHelper.PiOver2 : 0;

            if(_fadeTimer > 0)
            {
                _fadeTimer -= Time.DeltaTime;

                if (_fadeTimer <= 0) Despawn();
                else Sprite.Alpha = _fadeTimer / FADE_TIME;
            }
        }

        public override bool OnEntityCollision(Entity entity)
        {
            entity.Damage(7);
            Despawn();

            return false;
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void OnRoomCollision()
        {
            DoUpdate = false;
            _fadeTimer = FADE_TIME;
        }
    }
}
