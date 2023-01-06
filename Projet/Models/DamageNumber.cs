using HypoluxAdventure.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models
{
    internal class DamageNumber : GameObject
    {
        // int id

        // const int MIN_SCALE
        // const int MAX_SCALE
        // const int MIN_SCALE_DAMAGE
        // const int MAX_SCALE_DAMAGE

        // float scale = MathUtils.Lerp(MIN_SCALE, MAX_SCALE, MathUtils.InverseLerp(damage, MIN_SCALE_DAMAGE, MAX_SCALE_DAMAGE))
        // const float velY
        // vec2 position

        // textObject

        // const float lifetime
        // float lifetimer

        public DamageNumber(Game1 game, GameManager gameManager, int id, int damage) : base(game, gameManager) { }

        public override void Update()
        {
            // déplacement avec pos et velY
            // quand timer < 0 -> DamageOverlay.Dispawn
        }

        public override void Draw()
        {
            
        }
        
    }
}
