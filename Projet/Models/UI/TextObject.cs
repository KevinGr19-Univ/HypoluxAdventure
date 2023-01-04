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
        private SpriteBatch _spriteBatch;
        public TextObject(SpriteFont font, string text)
        {
            _font = font;
            _text = text;
        }

        public void Draw()
        {
            Vector2 center = _font.MeasureString(_text);

        }


    }
}
