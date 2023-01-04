using HypoluxAdventure.Models;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Managers
{
    internal class RoomManager : GameObject
    {
        private Texture2D _tileset;
        private Room[,] _rooms;
        private int[,] _tiles;

        public RoomManager(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _tileset = game.Content.Load<Texture2D>("img/tileset");
            _rooms = new Room[4, 4];
        }

        public override void Update()
        {
            
        }

        public override void Draw()
        {

        }
    }
}
