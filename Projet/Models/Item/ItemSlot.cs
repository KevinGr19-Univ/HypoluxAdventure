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
        public const int SLOT_WIDTH = 80;
        public const float DEPTH = 0.6f;

        public Item Item;

        private Sprite _frame;
        public Vector2 Position;
        private Vector2 _scale;

        public bool IsSelected = false;

        public string Label => Item == null ? "" : Item.Label;

        public ItemSlot(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _frame = new Sprite(game.Content.Load<Texture2D>("img/itemFrame"));
            GraphicsUtils.SetPixelSize(_frame, SLOT_WIDTH, SLOT_WIDTH, ref _scale);

            _frame.Depth = DEPTH;
        }

        public override void Update()
        {
            if (Item == null) return;

            Item.Update();
            if (IsSelected) Item.SelectedUpdate();
        }

        public override void Draw()
        {
            _frame.Color = IsSelected ? Color.LightCyan : Color.DarkGray;

            _frame.Draw(game.UICanvas, Position, 0, _scale);
            if (Item == null) return;

            Item.DrawSlot(Position);
            if(IsSelected) Item.Draw();

        }
    }
}
