using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
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
    internal class Diabolux : Monster
    {
        public Diabolux(Game1 game, GameManager gameManager, Room room, Vector2 spawnPos) : base(game, gameManager, room, spawnPos)
        {
            SpriteSheet spriteSheet = game.Content.Load<SpriteSheet>("img/diaboluxAnimation.sf", new JsonContentLoader());
            Sprite = new AnimatedSprite(spriteSheet);
            GraphicsUtils.SetPixelSize(Sprite, 200, 200, ref Scale);

            SoundPlayer.PlaySound("sound/diaboluxLaughSound");
            AnimatedSprite.Play("laugh");
        }

        private float _laughTime = 4;

        public override Vector2 HitboxSize => new Vector2(200);
        public override int MaxHealth => 10;

        private bool _flipped = false;

        public override void Update()
        {
            base.Update();

            if(_laughTime > 0)
            {
                _laughTime -= Time.DeltaTime;
                if (_laughTime <= 0) AnimatedSprite.Play("idleSouth");

                return;
            }

            PatternUpdate();

            Sprite.Effect = _flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        private void PatternUpdate()
        {

        }

        public override void OnPlayerCollision()
        {
            gameManager.Player.Damage(10);
        }

        public override void OnDeath()
        {
            base.OnDeath();
            SoundPlayer.PlaySound("sound/diaboluxDefeatedSound");
            AnimatedSprite.Play("death", () =>
            {
                gameManager.StartNextFloorTransition();
            });
        }
    }
}
