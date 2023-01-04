using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HypoluxAdventure.Models.UI
{
    internal class HealthBar : GameObject
    {

        private static Color HEALTH_LOW = Color.Red;
        private static float HEALTH_THRESHOLD_LOW = 0.2f;

        private static Color HEALTH_MIDDLE = Color.Yellow;
        private static float HEALTH_THRESHOLD_MIDDLE = 0.5f;

        private static Color HEALTH_HIGH = Color.Green;
        private static float HEALTH_THRESHOLD_HIGH = 0.8f;


        private float _healthScale = 1f;
        private Texture2D _barTexture;

        private Vector2 _position;
        private Vector2 _origin;

        public HealthBar(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _barTexture = GraphicsUtils.GetRectangleColor(game.GraphicsDevice, 200, 25, Color.White);
            _position = new Vector2(50,Application.SCREEN_HEIGHT-50);
            _origin = new Vector2(0, 0.5f);

        }

        public override void Draw()
        {
            Color color;
            if (_healthScale < HEALTH_THRESHOLD_LOW) color = HEALTH_LOW;
            else if (_healthScale > HEALTH_THRESHOLD_HIGH) color = HEALTH_HIGH;
            else
            {
                if (_healthScale < HEALTH_THRESHOLD_MIDDLE)
                    color = Color.Lerp(HEALTH_LOW, HEALTH_MIDDLE, MathUtils.InverseLerp(HEALTH_THRESHOLD_LOW, HEALTH_THRESHOLD_MIDDLE, _healthScale));
                else
                    color = Color.Lerp(HEALTH_MIDDLE, HEALTH_HIGH, MathUtils.InverseLerp(HEALTH_THRESHOLD_MIDDLE, HEALTH_THRESHOLD_HIGH, _healthScale));
            }

            game.UICanvas.Draw(_barTexture, _position, null, color, 0, _origin, new Vector2(_healthScale,1),SpriteEffects.None,0.5f);
        }

        public override void Update()
        {
            float targetScale = (float)gameManager.Player.Health / gameManager.Player.MaxHealth;
            _healthScale = MathUtils.Damp(_healthScale, targetScale, 1f, Time.DeltaTime);
        }
    }
}
