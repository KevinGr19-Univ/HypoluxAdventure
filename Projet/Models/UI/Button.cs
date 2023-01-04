using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HypoluxAdventure.Core;
using HypoluxAdventure.Models.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.UI
{
    struct ButtonColor
    {
        public Color TextColor;
        public Color BorderColor;
        public Color BackgroundColor;

        public Color TextColorHover;
        public Color BorderColorHover;
        public Color BackgroundColorHover;

        public Color TextColorDown;
        public Color BorderColorDown;
        public Color BackgroundColorDown;

    }

    internal class Button
    {
        private TextObject _text;
        private ButtonColor _color;
        private Rectangle _rect;
        public Vector2 Position;
        private bool _hover;
        
        public Button(SpriteFont textFont, string text, ButtonColor color, Vector2 position, Vector2 padding, float border)
        {
            TextObject buttonText = new TextObject(textFont, text, position);
            _rect = new Rectangle(0, 0, (int)textFont.MeasureString(text).X, (int)textFont.MeasureString(text).Y);
            Vector2 halfDim = new Vector2(textFont.MeasureString(text).X / 2, textFont.MeasureString(text).Y / 2);
            _rect.Offset(-halfDim);
        }

        public void Update()
        {
            _hover = _rect.Contains(Inputs.MousePosition);
            if (_hover)
            {
                
            }
            
        }

        public void Draw()
        {
            Color textColor, buttonColor, backgroundColor;

        }
    }
}
