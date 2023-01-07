using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
using MonoGame.Extended;

namespace HypoluxAdventure.Models
{
    internal class Player : Entity
    {
        public const int SIZE = 32;

        public override int MaxHealth => 20;

        public Player(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            Sprite = new Sprite(game.Content.Load<Texture2D>("img/perso"));
            GraphicsUtils.SetPixelSize(Sprite, SIZE, SIZE, ref Scale);
        }

        private FrameInputs _inputs;
        public bool Controllable => gameManager.State == GameState.Play;

        public override void Update()
        {
            base.Update();
            _inputs = Controllable ? gameManager.FrameInputs : new FrameInputs();

            if (!IsDead)
            {
                CalculateVelocity();
                CalculateShootDirection();
                Move();

                CalculateHitbox();
                ProcessPulse();

                if(Controllable) Sprite.Effect = IsFlipped ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
            else
            {
                ProcessDeathAnimation();
            }

        }

        public override void Draw()
        {
            if (_damageTimer <= 0 || _damageHitVisible) base.Draw();
        }

        public RectangleF Hitbox { get; private set; }

        private const float MAX_SPEED = 300f;
        private const float ACCEL_RATE = 8f;
        private const float DECEL_RATE = 7f;

        private void CalculateVelocity()
        {
            if(_inputs.X == 0 && _inputs.Y == 0)
            {
                Velocity -= Velocity * DECEL_RATE * Time.DeltaTime;
                return;
            }

            Vector2 targetVel = Vector2.Normalize(new Vector2(_inputs.X, _inputs.Y)) * MAX_SPEED;
            Vector2 velDif = targetVel - Velocity;
            Velocity += velDif * ACCEL_RATE * Time.DeltaTime;
        }

        private void CalculateHitbox()
        {
            Hitbox = new RectangleF((Position - new Vector2(SIZE) * 0.5f), new Vector2(SIZE));
        }

        public Vector2 ShootDirection { get; private set; }
        public bool IsFlipped => ShootDirection.X >= 0;
        
        private void CalculateShootDirection()
        {
            Vector2 mousePos = game.Camera.CameraToWorld(Inputs.MousePosition);
            ShootDirection = Vector2.Normalize(mousePos - Position);
        }

        public override int Damage(int damage)
        {
            int finalDamage = base.Damage(damage);
            if (finalDamage > 0)
            {
                gameManager.DamageOverlay.Pulse();
                StartPulsing();
            }

            return finalDamage;
        }

        public override void OnDeath()
        {
            base.OnDeath();
            StopPulsing();
            gameManager.GameOver();

            Sprite.Effect = SpriteEffects.None;
            Random r = new Random();

            _deathRotateSpeed = r.NextSingle(DEATH_ROTATE_MIN_SPEED, DEATH_ROTATE_MAX_SPEED) * (r.NextSingle() < 0.5f ? -1 : 1);
        }
        #region Invulnerability
        private const float DAMAGE_COOLDOWN = 1.5f;
        private const float DAMAGE_HIT_FLIP_TIME = 0.2f;
        private float _damageTimer;
        private float _damageHitFlipTimer;
        private bool _damageHitVisible = false;

        private void StartPulsing()
        {
            _damageTimer = DAMAGE_COOLDOWN;
            _damageHitFlipTimer = DAMAGE_HIT_FLIP_TIME;
            IsInvincible = true;
        }

        private void StopPulsing()
        {
            IsInvincible = false;
            _damageHitVisible = false;
            _damageTimer = 0;
        }

        public void ProcessPulse()
        {
            if(_damageTimer > 0)
            {
                _damageTimer -= Time.DeltaTime;
                if(_damageTimer <= 0)
                {
                    StopPulsing();
                    return;
                }

                _damageHitFlipTimer -= Time.DeltaTime;
                if(_damageHitFlipTimer <= 0)
                {
                    _damageHitFlipTimer += DAMAGE_HIT_FLIP_TIME;
                    _damageHitVisible = !_damageHitVisible;
                }
            }
        }
        #endregion

        #region Death
        private const float DEATH_ROTATE_MIN_SPEED = 360;
        private const float DEATH_ROTATE_MAX_SPEED = 720;
        private const float DEATH_ROTATE_DECEL = 1.5f;
        private float _deathRotateSpeed;

        private void ProcessDeathAnimation()
        {
            Rotation += _deathRotateSpeed * Time.DeltaTime;
            _deathRotateSpeed -= _deathRotateSpeed * DEATH_ROTATE_DECEL * Time.DeltaTime;
        }
        #endregion
    }
}
