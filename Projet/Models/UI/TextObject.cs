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
        public Vector2 Position;

        public TextObject(SpriteFont font, string text, Vector2 position)
        {
            _font = font;
            _text = text;
            Position = position;
        }

        public void Draw()
        {
            Vector2 measure = _font.MeasureString(_text);
            _spriteBatch.DrawString(_font, _text, Position - measure*0.5f, Color.White);

        }


    }
}
