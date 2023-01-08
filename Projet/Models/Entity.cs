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
using HypoluxAdventure.Models.Rooms;

namespace HypoluxAdventure.Models
{
    internal abstract class Entity : GameObject
    {
        public Entity(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            Health = MaxHealth;
            isPlayer = this is Player; // Literally
        }

        public readonly bool isPlayer;

        public Sprite Sprite { get; protected set; }

        public Vector2 Position = Vector2.Zero;
        public Vector2 Velocity;

        public float Rotation;
        public Vector2 Scale = Vector2.One;

        abstract public Vector2 HitboxSize { get; }

        private RectangleF _previousHitbox;
        public RectangleF Hitbox => new RectangleF(Position - HitboxSize * 0.5f, HitboxSize);

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
        public bool NoClip = false;

        public virtual int Damage(int damage)
        {
            if (IsInvincible) return 0;

            int finalDamage = Math.Min(Health, damage);
            if (finalDamage <= 0) return 0;

            Health -= finalDamage;
            gameManager.DamageOverlay.SpawnNumber(Position, finalDamage, false);
            isDamageFrame = true;
            return finalDamage;
        }

        public virtual int Heal(int heal)
        {
            int finalHeal = Math.Min(MaxHealth - Health, heal);
            if (finalHeal <= 0) return 0;

            Health += finalHeal;
            gameManager.DamageOverlay.SpawnNumber(Position, finalHeal, true);
            return finalHeal;
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

        /// <summary>Default method to move entity with its velocity and handle collisions.</summary>
        public virtual void Move()
        {
            if (Velocity.LengthSquared() < 0.1f) Velocity = Vector2.Zero;

            _previousHitbox = Hitbox;
            Position += Velocity * Time.DeltaTime;
            if(!NoClip) HandleCollisions();

        }

        #region Collisions
        private void HandleCollisions()
        {
            const float ERROR_BUFFER = 0.001f; // Prevents rounding errors

            Chest collidedChest = null;

            Vector2[] collidePoints = new Vector2[3];
            Action<RectangleF> collideAction;

            // X Axis
            if(Velocity.X != 0)
            {
                if (Velocity.X < 0)
                {
                    collidePoints[0] = new Vector2(Hitbox.Left, _previousHitbox.Top);
                    collidePoints[2] = new Vector2(Hitbox.Left, _previousHitbox.Bottom);
                    collidePoints[1] = Vector2.Lerp(collidePoints[0], collidePoints[2], 0.5f);

                    collideAction = (rect) =>
                    {
                        Velocity.X = 0;
                        Position.X = rect.Right + HitboxSize.X * 0.5f;
                    };
                }
                else
                {
                    collidePoints[0] = new Vector2(Hitbox.Right, _previousHitbox.Top);
                    collidePoints[2] = new Vector2(Hitbox.Right, _previousHitbox.Bottom);
                    collidePoints[1] = Vector2.Lerp(collidePoints[0], collidePoints[2], 0.5f);

                    collideAction = (rect) =>
                    {
                        Velocity.X = 0;
                        Position.X = rect.Left - HitboxSize.X * 0.5f - ERROR_BUFFER;
                    };
                }

                collidedChest = HandleSideCollision(collidePoints, collideAction);
            }

            // Y Axis
            if (Velocity.Y != 0)
            {
                if (Velocity.Y < 0)
                {
                    collidePoints[0] = new Vector2(_previousHitbox.Left, Hitbox.Top);
                    collidePoints[2] = new Vector2(_previousHitbox.Right, Hitbox.Top);
                    collidePoints[1] = Vector2.Lerp(collidePoints[0], collidePoints[2], 0.5f);

                    collideAction = (rect) =>
                    {
                        Velocity.Y = 0;
                        Position.Y = rect.Bottom + HitboxSize.Y * 0.5f;
                    };
                }
                else
                {
                    collidePoints[0] = new Vector2(_previousHitbox.Left, Hitbox.Bottom);
                    collidePoints[2] = new Vector2(_previousHitbox.Right, Hitbox.Bottom);
                    collidePoints[1] = Vector2.Lerp(collidePoints[0], collidePoints[2], 0.5f);

                    collideAction = (rect) =>
                    {
                        Velocity.Y = 0;
                        Position.Y = rect.Top - HitboxSize.Y * 0.5f - ERROR_BUFFER;
                    };
                }

                Chest tempChest = HandleSideCollision(collidePoints, collideAction);
                if (collidedChest == null) collidedChest = tempChest;
            }

            if (collidedChest != null && isPlayer) collidedChest.Open();
        }

        private Chest HandleSideCollision(Vector2[] collidePoints, Action<RectangleF> collideAction)
        {
            Room room = gameManager.RoomManager.CurrentRoom; // Only in currentRoom

            // Chest collisions
            if(room.Chest != null)
            {
                foreach(Vector2 pos in collidePoints)
                {
                    if (!room.Chest.Hitbox.Contains(pos)) continue;
            
                    collideAction.Invoke(room.Chest.Hitbox);
                    return room.Chest;
                }
            }

            // Room collisions
            foreach(Vector2 pos in collidePoints)
            {
                RectangleF? tileCollider = room.GetTileCollider(pos);

                if(tileCollider != null)
                {
                    collideAction.Invoke(tileCollider ?? new RectangleF());
                    break;
                }
            }

            return null;
        }
        #endregion

        public virtual void OnDeath() { }
        public virtual void OnDamage() { }

        public override void Draw()
        {
            Sprite.Depth = GameManager.GetYDepth(Position.Y);
            Sprite.Draw(game.Canvas, Position, MathHelper.ToRadians(Rotation), Scale);
        }
    }
}
