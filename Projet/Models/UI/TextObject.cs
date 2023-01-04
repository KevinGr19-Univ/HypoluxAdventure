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
        private SpriteFont font;
        private string text;

        public TextObject(SpriteFont font, string text)
        {
            this.font = font;
            this.text = text;
        }

        public SpriteFont Font { get => font; set => font = value; }
        public string Text { get => text; set => text = value; }
    }
}
