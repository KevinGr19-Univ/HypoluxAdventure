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
using System.Runtime.CompilerServices;
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

            _speed = MathUtils.Lerp(MIN_SPEED, MAX_SPEED, gameManager.Difficulty);
        }

        public override Vector2 HitboxSize => new Vector2(24);
        public override int MaxHealth => 10;

        private const float DETECT_RANGE = 270f;
        private const float KEEP_RANGE = 380f;

        // Increasing speed with difficulty
        private const float MIN_SPEED = 100;
        private const float MAX_SPEED = 140;
        private readonly float _speed;

        private const int DAMAGE = 3;

        private bool _followingPlayer = false;

        public override void Update()
        { 
            _followingPlayer = _followingPlayer ? IsPlayerInRange(KEEP_RANGE) : CanSeePlayer(DETECT_RANGE);

            if (overlapsPlayer || !_followingPlayer) Velocity = Vector2.Zero;
            else Velocity = TowardsPlayer() * _speed;

            base.Update();
        }

        public override void OnPlayerCollision()
        {
            gameManager.Player.Damage(DAMAGE);
        }

        public override void OnDeath()
        {
            base.OnDeath();
            AnimatedSprite.Play("death", () => { IsSlained = true; });
        }
    }
}
