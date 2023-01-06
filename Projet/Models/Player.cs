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

                if(Controllable) Sprite.Effect = IsFlipped ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
        }

        #region Movement and collisions
        public RectangleF Hitbox { get; private set; }

        private float _maxSpeed = 300f;
        private float _accelRate = 8f;
        private float _decelRate = 7f;

        private void CalculateVelocity()
        {
            if(_inputs.X == 0 && _inputs.Y == 0)
            {
                Velocity -= Velocity * _decelRate * Time.DeltaTime;
                return;
            }

            Vector2 targetVel = Vector2.Normalize(new Vector2(_inputs.X, _inputs.Y)) * _maxSpeed;
            Vector2 velDif = targetVel - Velocity;
            Velocity += velDif * _accelRate * Time.DeltaTime;
        }

        private void CalculateHitbox()
        {
            Hitbox = new RectangleF((Position - new Vector2(SIZE) * 0.5f), new Vector2(SIZE));
        }
        #endregion

        #region Shooting
        public Vector2 ShootDirection { get; private set; }
        public bool IsFlipped => ShootDirection.X >= 0;
        
        private void CalculateShootDirection()
        {
            Vector2 mousePos = game.Camera.CameraToWorld(Inputs.MousePosition);
            ShootDirection = Vector2.Normalize(mousePos - Position);
        }
        #endregion
    }
}
