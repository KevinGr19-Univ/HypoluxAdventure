using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Item
{
    internal static class ItemUtils
    {
        private static Texture2D TXT_SWORD;

        public static void LoadTextures(ContentManager content)
        {
            TXT_SWORD = content.Load<Texture2D>("img/sword");
        }

        public static Texture2D GetTexture(ItemType type)
        {
            return type switch
            {
                ItemType.Sword => TXT_SWORD,
                _ => throw new ArgumentException("Texture of type not found"),
            };
        }
    }
}
