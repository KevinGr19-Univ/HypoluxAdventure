using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;

namespace HypoluxAdventure.Models.UI
{
    internal class HealthBar : GameObject
    {
        private Sprite _decoration;

        private static Color HEALTH_LOW = Color.Black;
        private static float HEALTH_THRESHOLD_LOW = 0f;

        private static Color HEALTH_MIDDLE = Color.Red;
        private static float HEALTH_THRESHOLD_MIDDLE = 0.25f;

        private static Color HEALTH_HIGH = Color.Crimson;
        private static float HEALTH_THRESHOLD_HIGH = 0.8f;

        private float _healthScale = 1f;
        private Texture2D _barTexture;

        public Vector2 Position { get; private set; }
        private Vector2 _origin;

        public HealthBar(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _decoration = new Sprite(game.Content.Load<Texture2D>("img/healthBarDecoration"));
            _decoration.OriginNormalized = _origin;

            _barTexture = GraphicsUtils.GetRectangleColor(game.GraphicsDevice, _decoration.TextureRegion.Width, _decoration.TextureRegion.Height, Color.White);
            Position = new Vector2(30,Application.SCREEN_HEIGHT-60);
            _origin = new Vector2(0, 0);

            _decoration.Depth = 0.6f;
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

            
            game.UICanvas.Draw(_barTexture, Position, null, color, 0, _origin, new Vector2(_healthScale, 1), SpriteEffects.None, 0.5f);
            _decoration.Draw(game.UICanvas,Position, 0, Vector2.One);
            
        }

        private bool _trigger = false;

        public override void Update()
        {
            float targetScale = _trigger ? 0 : (float)gameManager.Player.Health / gameManager.Player.MaxHealth;
            _healthScale = MathUtils.Damp(_healthScale, targetScale, 0.45f,Time.DeltaTime);
        }
    }
}
