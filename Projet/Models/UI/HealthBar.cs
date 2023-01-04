using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;

namespace HypoluxAdventure.Models.UI
{
    internal class HealthBar : GameObject
    {
        private Sprite _decoration;
        private Vector2 _scale;

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
            _barTexture = GraphicsUtils.GetRectangleColor(game.GraphicsDevice, 260, 30, Color.White);
            _position = new Vector2(30,Application.SCREEN_HEIGHT-60);
            _origin = new Vector2(0, 1f);

            _decoration = new Sprite(game.Content.Load<Texture2D>("img/healthBarDecoration"));
            _decoration.OriginNormalized = Vector2.Zero;

        }

        public override void Draw()
        {

            Color color;
            if (_healthScale < HEALTH_THRESHOLD_LOW)
            {
                color = HEALTH_LOW;
            }
            else if (_healthScale > HEALTH_THRESHOLD_HIGH)
            {
                color = HEALTH_HIGH;
            }
            else
            {
                if (_healthScale < HEALTH_THRESHOLD_MIDDLE)
                    color = Color.Lerp(HEALTH_LOW, HEALTH_MIDDLE, MathUtils.InverseLerp(HEALTH_THRESHOLD_LOW, HEALTH_THRESHOLD_MIDDLE, _healthScale));
                else
                    color = Color.Lerp(HEALTH_MIDDLE, HEALTH_HIGH, MathUtils.InverseLerp(HEALTH_THRESHOLD_MIDDLE, HEALTH_THRESHOLD_HIGH, _healthScale));
            }

            
            game.UICanvas.Draw(_barTexture, _position, null, color, 0, _origin, new Vector2(_healthScale,1),SpriteEffects.None,0.5f);
            _decoration.Draw(game.UICanvas,_position,0, Vector2.One);
        }

        public override void Update()
        {
            float targetScale = (float)gameManager.Player.Health / gameManager.Player.MaxHealth;
            _healthScale = MathUtils.Damp(_healthScale, targetScale, 1f, Time.DeltaTime);
        }
    }
}
