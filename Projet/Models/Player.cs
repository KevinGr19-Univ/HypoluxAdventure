﻿using Microsoft.Xna.Framework;
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
using HypoluxAdventure.Models.Projectiles;

namespace HypoluxAdventure.Models
{
    internal class Player : Entity
    {
        public const int SIZE = 32;

        public override Vector2 HitboxSize => new Vector2(SIZE);
        public override int MaxHealth => 25;

        public Player(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            Sprite = new Sprite(game.Content.Load<Texture2D>("img/perso"));
            GraphicsUtils.SetPixelSize(Sprite, SIZE, SIZE, ref Scale);

            ProjectileHolder = new ProjectileHolder(game, gameManager);
        }

        private FrameInputs _inputs;

        public override void Update()
        {
            base.Update();
            _inputs = gameManager.FrameInputs;

            if (!IsDead)
            {
                CalculateVelocity();
                CalculateShootDirection();
                Move();

                ProcessPulse();
                if(gameManager.CanMove) Sprite.Effect = IsFlipped ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                ProjectileHolder.Update();
            }
            else
            {
                ProcessDeathAnimation();
            }
        }

        public override void Draw()
        {
            ProjectileHolder.Draw();
            if (_damageTimer <= 0 || _damageHitVisible) base.Draw();
        }

        private const float MAX_SPEED = 310f;
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
                gameManager.DamageOverlay.Pulse(true);
                StartPulsing();
            }

            return finalDamage;
        }

        public override int Heal(int heal)
        {
            int finalHeal = base.Heal(heal);
            if (finalHeal > 0) gameManager.DamageOverlay.Pulse(false);

            return finalHeal;
        }

        public override void OnDeath()
        {
            base.OnDeath();
            ProjectileHolder.Clear();

            StopPulsing();

            SoundPlayer.PlaySound("sound/deathSound");
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

        #region Projectiles
        public ProjectileHolder ProjectileHolder { get; private set; }

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
