using HypoluxAdventure.Core;
using HypoluxAdventure.Models;
using HypoluxAdventure.Models.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Managers
{
    internal class ItemManager : GameObject
    {
        private ItemSlot[] _itemSlotTab;
        private int _currentSlot;

        public ItemManager(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _currentSlot = 0;
        }

        public override void Draw()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}
