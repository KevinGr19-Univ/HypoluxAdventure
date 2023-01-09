using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Rooms;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Monsters
{
    internal class Worm : Monster
    {
        public Worm(Game1 game, GameManager gameManager, Room room, Vector2 spawnPos) : base(game, gameManager, room, spawnPos)
        {
            SpriteSheet spriteSheet = game.Content.Load<SpriteSheet>("img/wormAnimation.sf", new JsonContentLoader());
            Sprite = new AnimatedSprite(spriteSheet);
            GraphicsUtils.SetPixelSize(Sprite, 36, 36, ref Scale);

            AnimatedSprite.Play("idleSouth");
            _dashTime = MathUtils.Lerp(MAX_DASH_COOLDOWN, MIN_DASH_COOLDOWN, gameManager.Difficulty);
        }

        public override Vector2 HitboxSize => new Vector2(36);
        public override int MaxHealth => 16;

        private const float DETECT_RANGE = 350f;
        private const float KEEP_RANGE = 550f;

        private const float MAX_DASH_COOLDOWN = 2;
        private const float MIN_DASH_COOLDOWN = 1.2f;

        private const float DASH_SPEED = 430f;
        private const float DECEL_RATE = 2;

        private bool _followingPlayer = false;
        private readonly float _dashTime;
        private float _dashTimer;

        private int _spriteOrientation = 2;

        public override void Update()
        {
            base.Update();
            if (!IsDead)
            {
                Velocity -= Velocity * DECEL_RATE * Time.DeltaTime;

                int orientation = GetOrientation(TowardsPlayer());
                if (orientation != -1 && _spriteOrientation != orientation) SetAnimation(orientation);

                if (!_followingPlayer && CanSeePlayer(DETECT_RANGE))
                {
                    _followingPlayer = true;
                    _dashTimer = _dashTime * 0.7f;
                }

                if (_followingPlayer)
                {
                    if (!IsPlayerInRange(KEEP_RANGE)) _followingPlayer = false;
                    else if(_dashTimer > 0) _dashTimer -= Time.DeltaTime;

                    else
                    {
                        _dashTimer = _dashTime;
                        Velocity = TowardsPlayer(0.2f) * DASH_SPEED;
                    }
                }
            }
            
        }

        private void SetAnimation(int orientation)
        {
            switch (orientation)
            {
                case 0:
                    AnimatedSprite.Play("idleNorth");
                    break;

                case 1:
                    AnimatedSprite.Play("idleSide");
                    Sprite.Effect = SpriteEffects.None;
                    break;

                case 2:
                    AnimatedSprite.Play("idleSouth");
                    break;

                case 3:
                    AnimatedSprite.Play("idleSide");
                    Sprite.Effect = SpriteEffects.FlipHorizontally;
                    break;
            }

            _spriteOrientation = orientation;
        }

        public override void Draw()
        {
            base.Draw();
            //game.Canvas.DrawRectangle(Hitbox, Color.Red, 4);
        }

        public override void OnPlayerCollision()
        {
            gameManager.Player.Damage(4);
        }

        public override void OnDeath()
        {
            base.OnDeath();
            AnimatedSprite.Play("death", () => { IsSlained = true; });
        }
    }
}
