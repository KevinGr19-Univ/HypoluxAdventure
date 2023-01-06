using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.UI;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models
{
    internal class DamageNumber : GameObject
    {
        public const float MIN_SCALE = 0.6f;
        public const float MAX_SCALE = 0.8f;
        public const int MIN_SCALE_DAMAGE = 2;
        public const int MAX_SCALE_DAMAGE = 10;

        public const float velY = 30;

        public const float LIFETIME = 1.2f;
        private float _lifetimer;
        
        private TextObject _text;
        public readonly int Id;
        private readonly int _damage;
        private float scale;

        private Vector2 _position;

        // float scale = MathUtils.Lerp(MIN_SCALE, MAX_SCALE, MathUtils.InverseLerp(damage, MIN_SCALE_DAMAGE, MAX_SCALE_DAMAGE))

        

        public DamageNumber(Game1 game, GameManager gameManager, int id, int damage, Vector2 pos) : base(game, gameManager)
        {
            _text = new TextObject(gameManager.DamageOverlay.Font, damage.ToString(), _position);
            _text.Depth = 0.6f;
            _lifetimer = LIFETIME;
            Id = id;
            _damage = damage;
            _position = pos;
        }

        public override void Update()
        {
            if (_lifetimer > 0)
            {
                float lerpCoef = Math.Clamp(MathUtils.InverseLerp(MIN_SCALE_DAMAGE, MAX_SCALE_DAMAGE, _damage), 0, 1);
                scale = MathUtils.Lerp(MIN_SCALE, MAX_SCALE, lerpCoef);
                _text.Color = Color.Lerp(Color.Crimson, Color.Red, lerpCoef);

                _position.Y -= velY * Time.DeltaTime;
                _text.Position = _position;
                _text.Scale = new Vector2(scale);
            } 
            else gameManager.DamageOverlay.DespawnNumber(Id);

            _lifetimer -= Time.DeltaTime;
            // déplacement avec pos et velY
            // quand timer < 0 -> DamageOverlay.Dispawn
        }

        public override void Draw()
        {
            _text.Draw(game.Canvas);
        }
        
    }
}
