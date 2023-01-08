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
        public const float MIN_SCALE = 0.8f;
        public const float MAX_SCALE = 1;
        public const int MIN_SCALE_DAMAGE = 1;
        public const int MAX_SCALE_DAMAGE = 10;

        public const float velY = 30;

        public const float LIFETIME = 1.2f;
        private float _lifetimer;
        
        private TextObject _text;
        public readonly int Id;
        private readonly int _damage;

        private Vector2 _position;

        public DamageNumber(Game1 game, GameManager gameManager, int id, int damage, Vector2 pos, bool heal) : base(game, gameManager)
        {
            _text = new TextObject(gameManager.DamageOverlay.Font, damage.ToString(), _position);
            _text.Depth = 0.6f;
            _lifetimer = LIFETIME;
            Id = id;
            _damage = damage;
            _position = pos;

            float lerpCoef = Math.Clamp(MathUtils.InverseLerp(MIN_SCALE_DAMAGE, MAX_SCALE_DAMAGE, _damage), 0, 1);
            _text.Scale = new Vector2(MathUtils.Lerp(MIN_SCALE, MAX_SCALE, lerpCoef));

            if (heal) _text.Color = Color.Lerp(Color.Green, Color.Green, lerpCoef);
            else _text.Color = Color.Lerp(Color.Crimson, Color.Red, lerpCoef);
        }

        public override void Update()
        {
            if (_lifetimer > 0)
            {
                _position.Y -= velY * Time.DeltaTime;
                _text.Position = _position;
            } 
            else gameManager.DamageOverlay.DespawnNumber(Id);

            _lifetimer -= Time.DeltaTime;
        }

        public override void Draw()
        {
            _text.Draw(game.Canvas);
        }
        
    }
}
