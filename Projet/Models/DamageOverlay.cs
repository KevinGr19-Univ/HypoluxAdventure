using HypoluxAdventure.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models
{
    internal class DamageOverlay : GameObject
    {
        // Texture2D damageScreen
        // float _timer + const float PULSE_TIME => à faire quand MathUtils.LerpCubic existe

        // int nextNumberId
        // Dictionary<int, DamageNumber>

        public DamageOverlay(Game1 game, GameManager gameManager) : base(game, gameManager) { }

        public override void Update()
        {
            
        }

        public override void Draw()
        {

        }

        // Spawn number(int damage)
    }
}
