using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;

namespace HypoluxAdventure.Models
{
    internal abstract class Entity : GameObject
    {
        public Entity(Game1 game, GameManager gameManager) : base(game, gameManager) { }

        public Sprite Sprite { get; protected set; }

        public Vector2 Position = Vector2.Zero;
        public Vector2 Velocity;

        public float Rotation;
        public Vector2 Scale = Vector2.One;

        public override void Draw()
        {
            Sprite.Draw(game.Canvas, Position, Rotation, Scale);
        }
    }
}
