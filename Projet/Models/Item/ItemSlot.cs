using HypoluxAdventure.Managers;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Item
{
    internal class ItemSlot : GameObject
    {
        private Item _item;
        private Sprite _frame;

        public ItemSlot(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _frame= new Sprite(game.Content.Load<Texture2D>("img/itemFrame"));
        }

        public override void Draw()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}
