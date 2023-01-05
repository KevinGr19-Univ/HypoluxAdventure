using HypoluxAdventure.Models;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Core;

namespace HypoluxAdventure.Managers
{
    internal class PauseManager : GameObject
    {
        private RectangleF _rect;

        public PauseManager(Game1 game, GameManager gameManager) : base(game, gameManager) { }

        public override void Draw()
        {
            game.UICanvas.FillRectangle(_rect, Color.Black*0.3f);
        }

        public override void Update()
        {
            _rect = new RectangleF(0, 0, Application.SCREEN_WIDTH, Application.SCREEN_HEIGHT);
            
        }
    }
}
