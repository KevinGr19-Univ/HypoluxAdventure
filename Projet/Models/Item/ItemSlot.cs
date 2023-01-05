using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
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
        public const int SLOT_WIDTH = 50;

        private Item _item;
        private Sprite _frame;
        public Vector2 _position;
        private Vector2 _scale;


        public ItemSlot(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _frame= new Sprite(game.Content.Load<Texture2D>("img/itemFrame"));
            GraphicsUtils.SetPixelSize(_frame, SLOT_WIDTH, SLOT_WIDTH, ref _scale);
        }

        public override void Draw()
        {
            Logger.Debug(_position);
            _frame.Draw(game.UICanvas, _position, 0, Vector2.One);
        }

        public override void Update()
        {
            
        }
    }
}
