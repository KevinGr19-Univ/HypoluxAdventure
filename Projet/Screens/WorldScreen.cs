using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Managers;

namespace HypoluxAdventure.Screens
{
    internal class WorldScreen : AbstractScreen
    {
        public WorldScreen(Game1 game) : base(game) { }

        private GameManager _gameManager;

        public override void LoadContent()
        {
            base.LoadContent();
            _gameManager = new GameManager(Game);
            _gameManager.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            _gameManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _gameManager.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            _gameManager.Draw();
        }
    }
}
