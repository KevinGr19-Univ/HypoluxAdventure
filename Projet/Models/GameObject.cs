using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Managers;

namespace HypoluxAdventure.Models
{
    internal abstract class GameObject
    {
        protected Game1 game { get; private set; }
        protected GameManager gameManager { get; private set; }

        public GameObject(Game1 game, GameManager gameManager)
        {
            this.game = game;
            this.gameManager = gameManager;
        }

        abstract public void Update();
        abstract public void Draw();
    }
}
