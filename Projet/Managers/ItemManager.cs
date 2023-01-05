using HypoluxAdventure.Core;
using HypoluxAdventure.Models;
using HypoluxAdventure.Models.Item;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
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
        private const int SLOT_AMOUNT = 3;
        private const int WIDTH = ItemSlot.SLOT_WIDTH + SLOT_SPACING * (SLOT_AMOUNT - 1);

        private ItemSlot[] _itemSlots;
        private int _currentSlot;

        private Vector2 _position;
        private TextObject _itemDescription;

        public ItemManager(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _position = new Vector2(1100, Application.SCREEN_HEIGHT - 60);
            _itemSlots = new ItemSlot[SLOT_AMOUNT];

            for(int i = 0; i < SLOT_AMOUNT; i++)
            {
                _itemSlots[i] = new ItemSlot(game, gameManager);
                _itemSlots[i].Position = _position + new Vector2(SLOT_SPACING * (i - (SLOT_AMOUNT - 1) * 0.5f), 0);
            }

            Vector2 textPos = _position - new Vector2(0, ItemSlot.SLOT_WIDTH + 40) * 0.5f;
            _itemDescription = new TextObject(game.Content.Load<SpriteFont>("Font/ItemFont"), "No text", textPos);

            _currentSlot = 1;
            SelectSlot(0);
        }

        public void SelectSlot(int slot)
        {
            if (_currentSlot == slot) return;
            _itemSlots[_currentSlot].IsSelected = false;

            _currentSlot = slot;
            _itemSlots[_currentSlot].IsSelected = true;

            _itemDescription.Text = _itemSlots[_currentSlot].Label;
        }

        public bool AddItem(Item item)
        {
            int nextSlot = NextFreeSlot();
            if (nextSlot < 0) return false;

            _itemSlots[nextSlot].Item = item;

            if (_currentSlot == nextSlot) _itemDescription.Text = item.Label;
            return true;
        }


        /// <summary>Returns the position of the next free slot in the inventory, -1 if full.</summary>
        public int NextFreeSlot()
        {
            for (int i = 0; i < SLOT_AMOUNT; i++) if (_itemSlots[i].Item == null) return i;
            return -1;
        }

        public void ClearSlot(int slot, bool dropOnGround)
        {
            Item item = _itemSlots[slot].Item;
            if (item == null) return;

            _itemSlots[slot].Item = null;
            if (_currentSlot == slot) _itemDescription.Text = "";

            // TODO: Spawn item object on ground
        }

        public override void Update()
        {
            if (gameManager.FrameInputs.Shoot) AddItem(new Sword(game, gameManager)); // DEBUG

            int selectedSlot = Math.Clamp(_currentSlot - gameManager.FrameInputs.SlotScroll, 0, SLOT_AMOUNT - 1);
            SelectSlot(selectedSlot);

            if(gameManager.FrameInputs.DropItem) ClearSlot(selectedSlot, true);

            for (int i = 0; i < SLOT_AMOUNT; i++)
            {
                ItemSlot itemSlot = _itemSlots[i];

                itemSlot.Update();
                if (itemSlot.Item != null && itemSlot.Item.IsUsed) ClearSlot(i, false);
            }
        }

        public override void Draw()
        {
            foreach (ItemSlot itemSlot in _itemSlots) itemSlot.Draw();
            _itemDescription.Draw(game.UICanvas);
        }
        
    }
}
