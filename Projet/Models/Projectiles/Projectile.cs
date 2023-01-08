using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using Microsoft.Xna.Framework;
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
        protected Vector2 Position;
        protected Vector2 Velocity;

        protected Sprite Sprite;
        protected float Rotation;

        abstract public Vector2 HitboxSize { get; }

        protected Projectile(Game1 game, GameManager gameManager) : base(game, gameManager)
        {

        }

        public override void Update()
        {
            Position += Velocity * Time.DeltaTime;
        }

        public void HandleCollisions()
        {

        }

    }
}
