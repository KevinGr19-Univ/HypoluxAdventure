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
using System.Runtime.CompilerServices;
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

        private float _firstLaughTime = 4;

        public override Vector2 HitboxSize => new Vector2(100, 200);
        public override int MaxHealth => 300;

        public override void Update()
        {
            base.Update();

            // Laugh
            if(_firstLaughTime > 0)
            {
                _firstLaughTime -= Time.DeltaTime;
                gameManager.CameraManager.TargetPosition = Position; // Overwrites targetPosition

                if (_firstLaughTime <= 0)
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
            Orientate();
        }

        #region Animation
        private bool _movingAnim = false;
        private int _spriteAnimation = 2;

        private void Orientate()
        {
            bool moving = Velocity.LengthSquared() > 1f;
            int orientation = GetOrientation(moving ? Velocity : TowardsPlayer());

            if (orientation == _spriteAnimation && moving == _movingAnim) return;

            switch (orientation)
            {
                case 0:
                    AnimatedSprite.Play(moving ? "walkNorth" : "idleNorth");
                    break;

                case 1:
                    AnimatedSprite.Play(moving ? "walkSide" : "idleSide");
                    Sprite.Effect = SpriteEffects.FlipHorizontally;
                    break;

                case 2:
                    AnimatedSprite.Play(moving ? "walkSouth" : "idleSouth");
                    break;

                case 3:
                    AnimatedSprite.Play(moving ? "walkSide" : "idleSide");
                    Sprite.Effect = SpriteEffects.None;
                    break;
            }

            _spriteAnimation = orientation;
            _movingAnim = moving;
        }

        #endregion

        #region Patterns
        private const float SPEED = 110f;
        private const float FOLLOW_PLAYER_TIME_MARK = 1.1f;

        private int _patternAttack = 0;
        private float _nextPatternTime;

        private bool _initialAttack = false;

        private void PatternUpdate()
        {
            // First attack frame
            if (!_initialAttack)
            {
                _initialAttack = true;
                SummonFireballRing(32, 5.625f, 300f);
                _nextPatternTime = 7;
            }


            // Pattern choice
            if(_nextPatternTime > 0)
            {
                if (_nextPatternTime > FOLLOW_PLAYER_TIME_MARK) Velocity = TowardsPlayer() * SPEED;
                else Velocity = Vector2.Zero;

                _nextPatternTime -= Time.DeltaTime;
                if (_nextPatternTime < 0) ChooseNewPattern();
            }

            // Pattern update
            switch (_patternAttack)
            {
                case 1:
                    FireballArcs();
                    break;
            }
        }

        private void ChooseNewPattern()
        {
            _patternAttack = 1;

            switch (_patternAttack)
            {
                case 1:
                    _fireballArcIndex = 0;
                    break;
            }
        }

        private void ResetPattern(float idleTime)
        {
            _patternAttack = 0;
            _nextPatternTime = idleTime;
        }

        private float _fireballArcTimer;
        private int _fireballArcIndex;

        private void FireballArcs()
        {
            const int ARC_AMOUNT = 3;
            const int FIREBALL_AMOUNT = 12;

            const float MIN_SPEED = 320f;
            const float MAX_SPEED = 220f;

            const int MIN_ANGLE = 20;
            const float MAX_ANGLE = 75;

            // Summon arc
            if (_fireballArcTimer <= 0)
            {
                _fireballArcTimer = 1f;
                float spreadAngle = MathUtils.Lerp(MIN_ANGLE, MAX_ANGLE, (float)_fireballArcIndex / ARC_AMOUNT);
                float speed = MathUtils.Lerp(MIN_SPEED, MAX_SPEED, (float)_fireballArcIndex / ARC_AMOUNT);

                Vector2 centerShootDir = TowardsPlayer(0.2f);
                float centerAngle = MathF.Atan2(centerShootDir.Y, centerShootDir.X);

                for (int i = 0; i < FIREBALL_AMOUNT; i++)
                {
                    float shootAngle = MathUtils.Lerp(-spreadAngle, spreadAngle, (float)i / FIREBALL_AMOUNT);
                    shootAngle = MathHelper.ToRadians(shootAngle) + centerAngle;

                    Fireball fireball = new Fireball(game, gameManager, false, Position);
                    fireball.Velocity = speed * new Vector2(MathF.Cos(shootAngle), MathF.Sin(shootAngle));

                    fireball.Spawn();
                }

                if (++_fireballArcIndex >= ARC_AMOUNT) ResetPattern(7);
            }
            else _fireballArcTimer -= Time.DeltaTime;
        }

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

            gameManager.RoomManager.CurrentRoom.ProjectileHolder.Clear();

            AnimatedSprite.Play("death", () =>
            {
                gameManager.StartNextFloorTransition();
                gameManager.CameraManager.TargetPosition = Position;
            });
        }
    }
}
