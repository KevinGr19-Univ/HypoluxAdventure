using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HypoluxAdventure.Core;
using HypoluxAdventure.Models.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;
using HypoluxAdventure.Utils;

namespace HypoluxAdventure.Models.UI
{
    struct ButtonColor
    {
        public Color TextColor;
        public Color BorderColor;
        public Color BackgroundColor;

    }

    internal class Button
    {
        private Game1 game;
        private TextObject _text;

        public ButtonColor ColorNormal;
        public ButtonColor ColorHover;
        public ButtonColor ColorDown;

        private RectangleF _rect;
        private RectangleF _rectBackground;
        private RectangleF _rectText;

        public Vector2 Position;
        public Vector2 Scale = Vector2.One;
        public Vector2 Padding;

        private bool _hover;
        private bool _down;
        public float Border;
        public float Depth = 0.5f;

        private Action _action;

        public Button(Game1 game, SpriteFont textFont, string text, Vector2 position, Action action)
        {            
            _text = new TextObject(textFont, text, position);
            Position = position;
            this.game = game;
            this._action = action;
        }

        public void Update()
        {
            Vector2 textDimension = _text.Measure();
            _rectText = new RectangleF(Position - textDimension * 0.5f, textDimension);
            
            _rectBackground = _rectText;
            _rectBackground.Inflate(Padding.X, Padding.Y);

            _rect = _rectBackground;
            _rect.Inflate(Border, Border);

            _hover = _rect.Scale(Scale).Contains(Inputs.MousePosition);
            _down = _hover && Inputs.IsClickDown(Inputs.MouseButton.Left);

            if (_hover && Inputs.IsClickReleased(Inputs.MouseButton.Left)) _action.Invoke();
        }

        public void Draw()
        {
            ButtonColor colorState;
            if (_down) colorState = ColorDown;
            else if (_hover) colorState = ColorHover;
            else colorState = ColorNormal;

            game.UICanvas.FillRectangle(_rect.Scale(Scale), colorState.BorderColor, layerDepth:Depth - 0.002f);
            game.UICanvas.FillRectangle(_rectBackground.Scale(Scale), colorState.BackgroundColor, layerDepth:Depth - 0.001f);

            _text.Position = Position;
            _text.Scale = Scale;
            _text.Depth = Depth;
            _text.Color = colorState.TextColor;

            _text.Draw(game.UICanvas);
        }
    }
}
