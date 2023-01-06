using HypoluxAdventure.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Item
{
    internal class DropItem : GameObject
    {
        // Texture2D _texture
        // Item _item;
        // public Position
        // public Rectangle LocalHitbox
        // Rectangle

        public DropItem(Game1 game, GameManager gameManager, Item item) : base(game, gameManager)
        {
        }

        public override void Draw()
        {
            // Affichage Canvas (depth = 0.2f)
        }

        public override void Update()
        {
            // Collision avec joueur (Kévin)
        }
    }
}
