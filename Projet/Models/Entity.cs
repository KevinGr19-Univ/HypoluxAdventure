using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using HypoluxAdventure.Utils;

namespace HypoluxAdventure.Models
{
    internal abstract class Entity : GameObject
    {
        public Entity(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            Health = MaxHealth;
        }

        public Sprite Sprite { get; protected set; }

        public Vector2 Position = Vector2.Zero;
        public Vector2 Velocity;

        public float Rotation;
        public Vector2 Scale = Vector2.One;

        abstract public int MaxHealth { get; }
        public bool IsDead => Health <= 0;
        protected bool isDeathFrame { get; private set; }

        private int _health;
        public int Health
        {
            get => _health;
            protected set
            {
                bool wasDead = IsDead;

                _health = value;
                if (IsDead && !wasDead) isDeathFrame = true;
            }
        }

        protected bool isDamageFrame = false;
        public bool IsInvincible = false;

        public virtual int Damage(int damage)
        {
            if (IsInvincible) return 0;

            int finalDamage = Math.Min(Health, damage);
            Health -= finalDamage;

            isDamageFrame = true;
            return finalDamage;
        }

        public override void Update()
        {
            if (isDeathFrame)
            {
                OnDeath();
                isDeathFrame = false;
            }

            if (isDamageFrame)
            {
                OnDamage();
                isDamageFrame = false;
            }
        }

        /// <summary>Default method to move entity with its velocity.</summary>
        public void Move()
        {
            Position += Velocity * Time.DeltaTime;
        }

        public virtual void OnDeath() { }
        public virtual void OnDamage() { }

        public override void Draw()
        {
            float yDepth = MathUtils.InverseLerp(0, RoomManager.MAP_HEIGHT, Position.Y);
            Sprite.Depth = yDepth;

            Sprite.Draw(game.Canvas, Position, Rotation, Scale);
        }
    }
}
