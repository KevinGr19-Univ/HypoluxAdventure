using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Rooms;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Monsters
{
    internal abstract class Monster : Entity
    {
        protected AnimatedSprite AnimatedSprite => (AnimatedSprite)Sprite;
        protected Room room;

        /// <summary>The spawn position of the monster, relative to the room.</summary>
        protected Vector2 spawnPosition { get; private set; }

        /// <summary>Whether or not the monster should reload if its room is reloaded.</summary>
        public bool IsSlained = false;

        protected Monster(Game1 game, GameManager gameManager, Room room, Vector2 spawnPos) : base(game, gameManager)
        {
            this.room = room;
            spawnPosition = spawnPos;
        }

        protected bool overlapsPlayer;

        public bool IsPlayerInRange(float range) =>
            (Position - gameManager.Player.Position).LengthSquared() < range * range;

        public bool CanSeePlayer(float range)
        {
            const int POINTS_AMOUNT = 12;
            if (!IsPlayerInRange(range)) return false;

            for(int i = 1; i <= POINTS_AMOUNT; i++)
            {
                Vector2 lerpPoint = Vector2.Lerp(Position, gameManager.Player.Position, (float)i / POINTS_AMOUNT);
                Point tilePos = ((lerpPoint - room.Position) / Room.TILE_SIZE).ToPoint();

                if (room.IsWall(tilePos.X, tilePos.Y)) return false;
            }

            return true;
        }

        public Vector2 TowardsPlayer()
        {
            if (gameManager.Player.Position == Position) return Vector2.Zero;
            return Vector2.Normalize(gameManager.Player.Position - Position);
        }

        public int GetOrientation(Vector2 direction)
        {
            if (direction == Vector2.Zero) return -1;
            float angle = MathF.Atan2(direction.X, direction.Y);
            float posAngle = MathF.Abs(angle);

            if (posAngle < 0.25f * MathF.PI) return 2;
            if (posAngle > 0.75f * MathF.PI) return 0;

            return angle < 0 ? 1 : 3;
        }

        #region Damage Red Pulse
        private const float PULSE_TIME = 0.4f;
        private float _pulseTimer;

        public override void OnDamage()
        {
            base.OnDamage();
            _pulseTimer = PULSE_TIME;
        }

        public override void Update()
        {
            base.Update();
            AnimatedSprite.Update(Time.DeltaTime);

            if (!IsDead)
            {
                Move();
                overlapsPlayer = Hitbox.Intersects(gameManager.Player.Hitbox);
                if (overlapsPlayer) OnPlayerCollision();
            }

            if(_pulseTimer > 0) _pulseTimer -= Time.DeltaTime;
        }

        public override void Draw()
        {
            Sprite.Color = _pulseTimer > 0 ? Color.Lerp(Color.White, Color.Red, _pulseTimer / PULSE_TIME) : Color.White;
            base.Draw();
        }
        #endregion

        abstract public void OnPlayerCollision();
        public virtual void Spawn()
        {
            Health = MaxHealth;
            Position = spawnPosition;
        }

        public virtual void Unload()
        {
            if (IsDead) IsSlained = true;
        }
    }
}
