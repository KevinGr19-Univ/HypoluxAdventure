using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Monsters;
using HypoluxAdventure.Models.Rooms;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Projectiles
{
    internal abstract class Projectile : GameObject
    {
        protected Vector2 position;
        public Vector2 Velocity;

        protected Sprite Sprite;
        protected float rotation;
        protected Vector2 scale = Vector2.One;

        abstract public Vector2 HitboxSize { get; }
        public RectangleF Hitbox => new RectangleF(position - HitboxSize * 0.5f, HitboxSize);

        protected bool DoUpdate = true;

        public bool IsPlayerProjectile { get; private set; }

        protected Projectile(Game1 game, GameManager gameManager, bool isPlayerProj, Vector2 pos) : base(game, gameManager)
        {
            position = pos;
            IsPlayerProjectile = isPlayerProj;
        }

        public override void Update()
        {
            if (!DoUpdate) return;
            position += Velocity * Time.DeltaTime;

            // Enemy collision
            if (IsPlayerProjectile)
            {
                foreach(Monster monster in gameManager.RoomManager.CurrentRoom.GetAliveMonsters())
                {
                    if (!Hitbox.Intersects(monster.Hitbox)) continue;
                    if (!OnEntityCollision(monster)) break;
                }
            }
            else if (Hitbox.Intersects(gameManager.Player.Hitbox)) OnEntityCollision(gameManager.Player);

            // Room collision
            Room currentRoom = gameManager.RoomManager.CurrentRoom;
            foreach(Vector2 point in Hitbox.GetCorners())
            {
                if (!currentRoom.Rectangle.Contains(point))
                {
                    OnRoomCollision();
                    break;
                }

                if (currentRoom.Chest != null && currentRoom.Chest.Hitbox.Contains(point))
                {
                    OnRoomCollision();
                    break;
                }

                Point tilePos = ((point - currentRoom.Position) / Room.TILE_SIZE).ToPoint();
                if (currentRoom.IsWall(tilePos.X, tilePos.Y))
                {
                    OnRoomCollision();
                    break;
                }
            }
        }

        public void Spawn()
        {
            if (IsPlayerProjectile) gameManager.Player.ProjectileHolder.SpawnProjectile(this);
            else gameManager.RoomManager.CurrentRoom.ProjectileHolder.SpawnProjectile(this);
            OnSpawn();
        }

        public void Despawn()
        {
            if (IsPlayerProjectile) gameManager.Player.ProjectileHolder.DespawnProjectile(this);
            else gameManager.RoomManager.CurrentRoom.ProjectileHolder.DespawnProjectile(this);
        }

        public override void Draw()
        {
            Sprite.Draw(game.Canvas, position, rotation, scale);
        }

        abstract public void OnRoomCollision();

        /// <summary>Method called when the projectile hits something</summary>
        /// <param name="entity">The hit entity.</param>
        /// <returns>Whether or not the projectile should continue to hit other enemies that frame.</returns>
        abstract public bool OnEntityCollision(Entity entity);

        public virtual void OnSpawn() { }
        public virtual void OnDespawn() { }
    }
}
