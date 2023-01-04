using HypoluxAdventure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private string _text;
        public Vector2 Position;
        public Color Color;

        public TextObject(SpriteFont font, string text, Vector2 position, Color? color = null)
        {
            _font = font;
            _text = text;
            Position = position;
            Color = color ?? Color.White;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 measure = _font.MeasureString(_text);
            spriteBatch.DrawString(_font, _text, Position - measure*0.5f, Color.White);

        }


    }
}
