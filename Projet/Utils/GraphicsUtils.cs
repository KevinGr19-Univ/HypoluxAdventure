using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Utils
{
    internal static class GraphicsUtils
    {
        public static Texture2D GetRectangleColor(GraphicsDevice graphicsDevice, int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(graphicsDevice, width, height);

            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; i++) data[i] = color;

            texture.SetData(data);
            return texture;
        }

        public static void SetPixelSize(Sprite sprite, int width, int height, ref Vector2 scale)
        {
            scale.X = (float)width / sprite.TextureRegion.Width;
            scale.Y = (float)height / sprite.TextureRegion.Height;
        }
    }
}
