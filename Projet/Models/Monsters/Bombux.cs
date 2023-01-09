using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Projectiles;
using HypoluxAdventure.Models.Rooms;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    internal class Bombux : Monster
    {
        public Bombux(Game1 game, GameManager gameManager, Room room, Vector2 spawnPos) : base(game, gameManager, room, spawnPos)
        {
            SpriteSheet spriteSheet = game.Content.Load<SpriteSheet>("img/bomberAnimation.sf", new JsonContentLoader());
            Sprite = new AnimatedSprite(spriteSheet);
            GraphicsUtils.SetPixelSize(Sprite, 72, 72, ref Scale);

            AnimatedSprite.Play("idleSouth");
            _shootCooldown = MathUtils.Lerp(MAX_SHOOT_COOLDOWN, MIN_SHOOT_COOLDOWN, gameManager.Difficulty);
        }

        public override Vector2 HitboxSize => new Vector2(45, 72);
        public override int MaxHealth => 24;

        private const float RANGE = 600f;

        private const float MAX_SHOOT_COOLDOWN = 3f;
        private const float MIN_SHOOT_COOLDOWN = 2f;
        private const float SHOOT_ANIM_TIME = 0.5f;

        private readonly float _shootCooldown;
        private float _shootTimer, _shootAnimTimer;
        private bool _targetingPlayer = false;

        private int _spriteOrientation = 2;

        public override void Update()
        {
            base.Update();
            if (!IsDead)
            {
                int orientation = GetOrientation(TowardsPlayer());

                // Target player
                bool targetingPlayer = CanSeePlayer(RANGE);
                if (!_targetingPlayer && targetingPlayer)
                {
                    // On target
                    _targetingPlayer = true;
                    _shootTimer = _shootCooldown;
                }
                else if (_targetingPlayer && !targetingPlayer)
                {
                    // On untarget
                    _targetingPlayer = false;
                }

                if(_shootAnimTimer > 0)
                {
                    _shootAnimTimer -= Time.DeltaTime;
                    if (_shootAnimTimer < 0) SetAnimation(orientation, false);
                }
                else
                {
                    if (orientation != _spriteOrientation) SetAnimation(orientation, false);
                    if(_targetingPlayer && _shootTimer > 0)
                    {
                        _shootTimer -= Time.DeltaTime;
                        if(_shootTimer < 0)
                        {
                            Bomb bomb = new Bomb(game, gameManager, false, Position);
                            bomb.Velocity = TowardsPlayer(0.2f) * Bomb.SPEED;
                            bomb.Spawn();

                            _shootAnimTimer = SHOOT_ANIM_TIME;
                            SetAnimation(orientation, true);
                            _shootTimer = _shootCooldown;
                        }
                    }
                }
                
            }
        }

        public override void OnPlayerCollision()
        {
            gameManager.Player.Damage(4);
        }

        private void SetAnimation(int orientation, bool attack)
        {
            switch (orientation)
            {
                case 0:
                    AnimatedSprite.Play(attack ? "atqNorth" : "idleNorth");
                    break;

                case 1:
                    AnimatedSprite.Play(attack ? "atqSide" : "idleSide");
                    Sprite.Effect = SpriteEffects.FlipHorizontally;
                    break;

                case 2:
                    AnimatedSprite.Play(attack ? "atqSouth" : "idleSouth");
                    break;

                case 3:
                    AnimatedSprite.Play(attack ? "atqSide" : "idleSide");
                    Sprite.Effect = SpriteEffects.None;
                    break;
            }

            _spriteOrientation = orientation;
        }

        public override void OnDeath()
        {
            base.OnDeath();
            AnimatedSprite.Play("death", () => { IsSlained = true; });
        }
    }
}
