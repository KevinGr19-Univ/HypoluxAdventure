using HypoluxAdventure.Models;
using HypoluxAdventure.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Managers
{
    internal class ItemManager : GameObject
    {
        // Lists
        private List<DropItem> _items;
        private List<DropItem> _itemsToRemove;


        public ItemManager(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _items = new List<DropItem>();
            _itemsToRemove = new List<DropItem>();

        }

        public override void Update()
        {
            foreach (DropItem item in _items) item.Update();

            // Effacer les objets
            foreach (DropItem item in _itemsToRemove) _items.Remove(item);
            _itemsToRemove.Clear();
        }

        public override void Draw()
        {
            foreach (DropItem item in _items)
            {
                item.Draw();
            }
        }

        // Summon(DropItem)
        public void Summon(DropItem droppedItem)
        {
            _items.Add(droppedItem);
        }

        // Dispawn(DropItem)
        public void Despawn(DropItem droppedItemToDespawn)
        {
            _itemsToRemove.Add(droppedItemToDespawn);
        }

        // Clear
        public void Clear()
        {
            _items.Clear();
            _itemsToRemove.Clear();
        }
    }
}
