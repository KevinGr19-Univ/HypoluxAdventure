using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.UI;
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
        public const int MIN_SCALE = 2;
        public const int MAX_SCALE = 3;
        public const int MIN_SCALE_DAMAGE = 2;
        public const int MAX_SCALE_DAMAGE = 3;

        public const float velY = 2;

        public const float LIFETIME = 2;
        private float _lifetimer;
        
        private TextObject _text;
        public readonly int Id;

        public Vector2 Position;

        // float scale = MathUtils.Lerp(MIN_SCALE, MAX_SCALE, MathUtils.InverseLerp(damage, MIN_SCALE_DAMAGE, MAX_SCALE_DAMAGE))

        

        public DamageNumber(Game1 game, GameManager gameManager, int id, int damage) : base(game, gameManager)
        {
            _text = new TextObject(gameManager.DamageOverlay.Font, damage.ToString(), Position);
        }

        public override void Update()
        {
            
                _text.Position.Y += velY * Time.DeltaTime;

            

            
            // déplacement avec pos et velY
            // quand timer < 0 -> DamageOverlay.Dispawn
        }

        public override void Draw()
        {
            
        }
        
    }
}
