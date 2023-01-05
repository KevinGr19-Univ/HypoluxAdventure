using HypoluxAdventure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.UI
{
    internal class TextObject 
    {
        private SpriteFont _font;
        public string Text;
        public Vector2 Position;
        public Vector2 Scale = Vector2.One;
        public Color Color;
        public float Depth = 0.5f;

        public TextObject(SpriteFont font, string text, Vector2 position, Color? color = null)
        {
            _font = font;
            Text = text;
            Position = position;
            Color = color ?? Color.White;
        }

        public Vector2 Measure()
        {
            return _font.MeasureString(Text);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 measure = Measure();
            spriteBatch.DrawString(_font, Text, Position, Color.White, 0, measure * 0.5f, Scale, SpriteEffects.None, Depth);
        }



    }
}
