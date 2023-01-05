using HypoluxAdventure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Item
{
    internal class DroppedItem : GameObject
    {
        public const int PIXEL_SIZE = 24;

        private ItemType _type;
        private Texture2D _texture;

        public RectangleF Rectangle { get; private set; }

        public DroppedItem(Game1 game, GameManager gameManager, ItemType type, Vector2 position) : base(game, gameManager)
        {
            // LOAD TEXTURE CORRESPONDANTE
            // CREATE THE RECTANGLE
        }

        public override void Update()
        {
            // TESTER SI PLAYER EN CONTACT
        }

        public override void Draw()
        {
            // DRAW WITH Y DEPTH
        }
        
    }
}
