using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Items
{
    internal class ItemSlot : GameObject
    {
        public const int SLOT_WIDTH = 80;
        public const float DEPTH = 0.6f;

        public static readonly Color COOLDOWN_COLOR = new Color(Color.Black, 0.5f);

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
            float cooldownScale = 0;

            if (Item != null)
            {
                Item.DrawSlot(Position);
                cooldownScale = Item.CooldownProgress;

                if (IsSelected) Item.Draw();
            }

            RectangleF frameRect = _frame.GetBoundingRectangle(Position, 0, _scale);

            game.UICanvas.FillRectangle(frameRect.TopLeft, frameRect.Size * new Vector2(1, cooldownScale),
                COOLDOWN_COLOR, DEPTH + 0.2f);

        }
    }
}
