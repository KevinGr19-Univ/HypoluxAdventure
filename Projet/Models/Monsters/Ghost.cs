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
    internal class Ghost : Monster
    {
        public Ghost(Game1 game, GameManager gameManager, Room room, Vector2 spawnPos) : base(game, gameManager, room, spawnPos)
        {
            SpriteSheet spriteSheet = game.Content.Load<SpriteSheet>("img/ghostAnimation.sf", new JsonContentLoader());
            Sprite = new AnimatedSprite(spriteSheet);
            GraphicsUtils.SetPixelSize(Sprite, 52, 52, ref Scale);

            NoClip = true;
            AnimatedSprite.Play("south");
            _speed = MathUtils.Lerp(MIN_SPEED, MAX_SPEED, gameManager.Difficulty);
        }

        public override int MaxHealth => 20;
        public override Vector2 HitboxSize => new Vector2(40, 48);

        private const float MIN_SPEED = 80;
        private const float MAX_SPEED = 115;
        private readonly float _speed;

        private int _spriteOrientation = 2;

        public override void Update()
        {
            if (!IsDead)
            {
                int orientation = GetOrientation(Velocity);
                if (orientation != -1 && _spriteOrientation != orientation) SetAnimation(orientation);
            }

            Velocity = !overlapsPlayer ? TowardsPlayer() * _speed : Vector2.Zero;
            base.Update();
        }

        private void SetAnimation(int orientation)
        {
            switch (orientation)
            {
                case 0:
                    AnimatedSprite.Play("north");
                    break;

                case 1:
                    AnimatedSprite.Play("side");
                    Sprite.Effect = SpriteEffects.FlipHorizontally;
                    break;

                case 2:
                    AnimatedSprite.Play("south");
                    break;

                case 3:
                    AnimatedSprite.Play("side");
                    Sprite.Effect = SpriteEffects.None;
                    break;
            }

            _spriteOrientation = orientation;
        }

        public override void OnPlayerCollision()
        {
            gameManager.Player.Damage(4);
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void OnDeath()
        {
            base.OnDeath();
            AnimatedSprite.Play("death", () => { IsSlained = true; });
        }
    }
}
