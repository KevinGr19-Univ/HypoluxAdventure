using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
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

namespace HypoluxAdventure.Models.UI
{
    internal class HealthOverlay : GameObject
    {
        private const int HEART_SIZE = 50;

        private AnimatedSprite _healthIcon;
        private Vector2 _healthIconScale;
        private Vector2 _heartPosition;

        private HealthBar _healthBar;

        public HealthOverlay(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _healthBar = new HealthBar(game,gameManager);

            SpriteSheet heartIconSpriteSheet = game.Content.Load<SpriteSheet>("img/Heart/heartAnimation.sf", new JsonContentLoader());
            _healthIcon = new AnimatedSprite(heartIconSpriteSheet);
            _healthIcon.Play("idle");

            _heartPosition = _healthBar.Position + new Vector2(300, 20);

            GraphicsUtils.SetPixelSize(_healthIcon, HEART_SIZE, HEART_SIZE, ref _healthIconScale);
            _healthIcon.Depth = 0.6f;
        }

        public override void Draw()
        {
            _healthBar.Draw();
            _healthIcon.Draw(game.UICanvas, _heartPosition, 0, _healthIconScale);
        }

        public override void Update()
        {
            _healthBar.Update();
            _healthIcon.Update(Time.DeltaTime);
        }

        public void PlayDeathAnimation()
        {
            _healthIcon.Play("death");
        }
    }
}
