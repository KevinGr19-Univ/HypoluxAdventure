using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Projectiles;
using HypoluxAdventure.Models.Rooms;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Monsters
{
    internal class Diabolux : Monster
    {
        public Diabolux(Game1 game, GameManager gameManager, Room room, Vector2 spawnPos) : base(game, gameManager, room, spawnPos)
        {
            SpriteSheet spriteSheet = game.Content.Load<SpriteSheet>("img/diaboluxAnimation.sf", new JsonContentLoader());
            Sprite = new AnimatedSprite(spriteSheet);
            GraphicsUtils.SetPixelSize(Sprite, 200, 200, ref Scale);

            SoundPlayer.PlaySound("sound/diaboluxLaughSound",0.5f);
            AnimatedSprite.Play("laugh");
        }

        private float _laughTime = 4;

        public override Vector2 HitboxSize => new Vector2(100, 200);
        public override int MaxHealth => 150;

        private bool _flipped = false;

        public override void Update()
        {
            base.Update();

            // Laugh
            if(_laughTime > 0)
            {
                _laughTime -= Time.DeltaTime;
                gameManager.CameraManager.TargetPosition = Position; // Overwrites targetPosition

                if (_laughTime <= 0)
                {
                    AnimatedSprite.Play("idleSouth");
                    gameManager.CanMove = true;
                }
                return;
            }

            if (IsDead)
            {
                gameManager.CameraManager.TargetPosition = Position;
                return;
            }

            PatternUpdate();

            Sprite.Effect = _flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        #region Patterns
        private bool _initialAttack = false;

        private void PatternUpdate()
        {
            // First attack frame
            if (!_initialAttack)
            {
                _initialAttack = true;
                SummonFireballRing(16, 11.25f, 300f);
            }
        }
        #endregion

        #region Fireball throws
        private void SummonFireballRing(int amount, float angleOffset, float speed)
        {
            float offset = MathHelper.ToRadians(angleOffset);

            for(int i = 0; i < amount; i++)
            {
                Vector2 velocity = new Vector2((float)i / amount) * MathF.PI * 2;
                velocity = new Vector2(MathF.Cos(velocity.X + angleOffset), MathF.Sin(velocity.Y + angleOffset)) * speed;

                Fireball fireball = new Fireball(game, gameManager, false, Position);
                fireball.Velocity = velocity;
                fireball.Spawn();
            }
        }

        #endregion

        public override void OnPlayerCollision()
        {
            gameManager.Player.Damage(10);
        }

        public override void OnDeath()
        {
            base.OnDeath();
            SoundPlayer.PlaySound("sound/diaboluxDefeatedSound");
            AnimatedSprite.Play("death", () =>
            {
                gameManager.StartNextFloorTransition();
                gameManager.CameraManager.TargetPosition = Position;
            });
        }
    }
}
