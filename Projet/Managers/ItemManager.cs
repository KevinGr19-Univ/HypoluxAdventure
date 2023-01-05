using HypoluxAdventure.Core;
using HypoluxAdventure.Models;
using HypoluxAdventure.Models.Item;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Managers
{
    internal class ItemManager : GameObject
    {
        private const int SLOT_SPACING = ItemSlot.SLOT_WIDTH + 20;

        private ItemSlot[] _itemSlots;
        private int _currentSlot;
        private Vector2 _position;

        public ItemManager(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _currentSlot = 0;
            _position = new Vector2(Application.ScreenDimensions.X * 0.85f, Application.ScreenDimensions.Y * 0.9125f);

            _itemSlots = new ItemSlot[2];

            AddItemSlot(0, new ItemSlot(game, gameManager));
            AddItemSlot(1, new ItemSlot(game, gameManager));
        }

        public override void Draw()
        {
            foreach (ItemSlot itemSlot in _itemSlots) itemSlot.Draw();
        }

        public override void Update()
        {
            
            
        }
        
        private void AddItemSlot(int slot, ItemSlot newItemSlot)
        {
            _itemSlots[slot] = newItemSlot;
            newItemSlot.Position = _position + Vector2.UnitX * SLOT_SPACING * slot;
        }
    }
}
