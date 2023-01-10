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

            SoundPlayer.PlaySound("sound/diaboluxLaughSound", 0.8f);
            AnimatedSprite.Play("laugh");
        }

        private float _firstLaughTime = 4;

        public override Vector2 HitboxSize => new Vector2(100, 200);
        public override int MaxHealth => 400;

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
        private bool _orientate = true;
        private bool _movingAnim = false;
        private int _spriteAnimation = 2;

        private void Orientate()
        {
            if (!_orientate) return;

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
        private const float DASH_SPEED = 390f;
        private const float FOLLOW_PLAYER_TIME_MARK = 0.9f;

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

                case 2:
                    DashAttack();
                    break;

                case 3:
                    FireballWalls();
                    break;

                case 4:
                    HellRain();
                    break;
            }
        }

        private void ChooseNewPattern()
        {
            _patternAttack = !_hellRainDone && Health <= 150 ? 4 : new Random().Next(1, 4);

            switch (_patternAttack)
            {
                case 1:
                    _fireballArcIndex = 0;
                    break;

                case 2:
                    _dashed = false;
                    break;

                case 3:
                    _fireballWall = false;
                    break;

                case 4:
                    _hellRainState = 0;
                    _hellRainTimer = 0;
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

            const float MIN_SPEED = 280f;
            const float MAX_SPEED = 220f;

            const int MIN_ANGLE = 20;
            const float MAX_ANGLE = 75;

            // Summon arc
            if (_fireballArcTimer <= 0)
            {
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

                PlayFireballSound();

                if (++_fireballArcIndex >= ARC_AMOUNT) ResetPattern(6f);
                else _fireballArcTimer = 1f;
            }
            else _fireballArcTimer -= Time.DeltaTime;
        }

        private const float DASH_TIME = 1.6f;
        private float _dashTimer;

        private bool _dashed = false;

        private void DashAttack()
        {
            if (_dashTimer <= 0)
            {
                if (!_dashed)
                {
                    _dashed = true;
                    _dashTimer = DASH_TIME;
                    Velocity = TowardsPlayer(0.3f) * DASH_SPEED;
                    SummonFireballRing(16, 0, 230);
                }
                else ResetPattern(4.5f);
            }
            else _dashTimer -= Time.DeltaTime;
        }

        private const float FIREBALL_WALL_TIME = 2.1f;
        private int[] _fireballWallDirections;

        private float _fireballWallTimer;
        private bool _fireballWall = false;

        private void FireballWalls()
        {
            if(_fireballWallTimer <= 0)
            {
                // Init fireball walls
                if (!_fireballWall)
                {
                    _fireballWallDirections = new int[] { 0, 1, 2, 3 }.TakeRandom(2);
                    SummonFireballWall(_fireballWallDirections[0], 18, 180f);

                    _fireballWallTimer = FIREBALL_WALL_TIME;
                    _fireballWall = true;
                }
                else
                {
                    SummonFireballWall(_fireballWallDirections[1], 18, 180f);
                    ResetPattern(6.5f);
                }
                
            }
            else _fireballWallTimer -= Time.DeltaTime;
        }

        private Vector2 _hellRainTargetPos;
        private int _hellRainState;

        private float _hellRainTimer;
        private int _hellRainCount;
        private bool _hellRainDone = false;

        private void HellRain()
        {
            const float HELL_RAIN_ATTACK_COOLDOWN = 1.2f;
            const int HELL_RAIN_ATTACK_COUNT = 10;

            // Move towards center
            if(_hellRainState == 0)
            {
                _hellRainState = 1;
                _hellRainCount = HELL_RAIN_ATTACK_COUNT;

                _hellRainTargetPos = gameManager.RoomManager.CurrentRoom.Rectangle.Center;
                Velocity = Vector2.Normalize(_hellRainTargetPos - Position) * SPEED * 1.5f;
            }

            // Begin attacks
            else if(_hellRainState == 1)
            {
                // If close to center enough
                if((Position - _hellRainTargetPos).LengthSquared() < 25)
                {
                    _hellRainState = 2;
                    _orientate = false;

                    AnimatedSprite.Play("laugh");
                    SoundPlayer.PlaySound("sound/diaboluxLaughSound", 0.8f);
                    Velocity = Vector2.Zero;
                }
            }

            // Attack update
            else
            {
                _hellRainTimer -= Time.DeltaTime;
                if(_hellRainTimer <= 0)
                {
                    _hellRainTimer = HELL_RAIN_ATTACK_COOLDOWN;

                    // End attack
                    if (--_hellRainCount <= 0)
                    {
                        _orientate = true;
                        SummonFireballRing(24, 0, 200f);

                        _hellRainDone = true;
                        ResetPattern(7.5f);
                    }
                    else SummonFireballWall(new Random().Next(0, 4), 17, 180f);
                }
            }
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

            PlayFireballSound();
        }

        private void SummonFireballWall(int orientation, int amount, float speed)
        {
            const int BASE_AMOUNT = 24;
            Vector2 startPoint, endPoint, velocity;

            switch (orientation)
            {
                case 0:
                    startPoint = FireballPosFromPoint(3, 36);
                    endPoint = FireballPosFromPoint(36, 36);
                    velocity = new Vector2(0, -speed);
                    break;

                case 1:
                    startPoint = FireballPosFromPoint(36, 3);
                    endPoint = FireballPosFromPoint(36, 36);
                    velocity = new Vector2(-speed, 0);
                    break;

                case 2:
                    startPoint = FireballPosFromPoint(3, 3);
                    endPoint = FireballPosFromPoint(36, 3);
                    velocity = new Vector2(0, speed);
                    break;

                case 3:
                    startPoint = FireballPosFromPoint(3, 3);
                    endPoint = FireballPosFromPoint(3, 36);
                    velocity = new Vector2(speed, 0);
                    break;

                default:
                    throw new Exception("Invalid wall orientation");

            }

            // Possible fireball positions
            Vector2[] fireballPossiblePos = new Vector2[BASE_AMOUNT];
            fireballPossiblePos[0] = startPoint;
            fireballPossiblePos[BASE_AMOUNT-1] = endPoint;

            for (int i = 1; i < BASE_AMOUNT-1; i++) fireballPossiblePos[i] = Vector2.Lerp(startPoint, endPoint, (float)i/(BASE_AMOUNT-1));

            // Spawn fireballs
            Vector2[] fireballPositions = fireballPossiblePos.TakeRandom(amount);
            foreach(Vector2 fireballPos in fireballPositions)
            {
                Fireball fireball = new Fireball(game, gameManager, false, fireballPos);
                fireball.Velocity = velocity;
                fireball.Spawn();
            }

            PlayFireballSound();
        }

        private Vector2 FireballPosFromPoint(int x, int y)
            => (new Vector2(x, y) + new Vector2(0.5f)) * Room.TILE_SIZE + gameManager.RoomManager.CurrentRoom.Position;

        private void PlayFireballSound()
        {
            SoundPlayer.PlaySound("sound/fireballSound", 0.5f);
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
