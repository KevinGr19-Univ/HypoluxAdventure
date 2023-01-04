using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.UI
{
    internal class GameOverlay : GameObject
    {
        private HealthBar _healthBar;

        public GameOverlay(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _healthBar = new HealthBar(game,gameManager);
        }

        public override void Draw()
        {
            _healthBar.Draw();
        }

        public override void Update()
        {
            _healthBar.Update();
        }
    }
}
