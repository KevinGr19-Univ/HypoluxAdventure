using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Rooms;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Monsters
{
    internal class Globux : Monster
    {
        public Globux(Game1 game, GameManager gameManager, Room room, Vector2 spawnPos) : base(game, gameManager, room, spawnPos)
        {
            SpriteSheet spriteSheet = game.Content.Load<SpriteSheet>("img/GlobuxAnimation.sf", new JsonContentLoader());
            Sprite = new AnimatedSprite(spriteSheet);
            GraphicsUtils.SetPixelSize(Sprite, 24, 24, ref Scale);

            AnimatedSprite.Play("idle");
        }

        public override Vector2 HitboxSize => new Vector2(24);
        public override int MaxHealth => 10;

        private const float DETECT_RANGE = 200f;
        private const float KEEP_RANGE = 300f;
        private const float SPEED = 100;

        private bool _followingPlayer = false;

        public override void Update()
        {
            if (Inputs.IsKeyPressed(Keys.I)) Damage(3);

            _followingPlayer = _followingPlayer ? IsPlayerInRange(KEEP_RANGE) : CanSeePlayer(DETECT_RANGE);

            if (overlapsPlayer || !_followingPlayer) Velocity = Vector2.Zero;
            else Velocity = TowardsPlayer() * SPEED;

            base.Update();
        }

        public override void OnPlayerCollision()
        {
            gameManager.Player.Damage(2);
        }

        public override void OnDeath()
        {
            base.OnDeath();
            AnimatedSprite.Play("death", () => {
                IsSlained = true;
                Logger.Debug("Completed");
            });
        }
    }
}
