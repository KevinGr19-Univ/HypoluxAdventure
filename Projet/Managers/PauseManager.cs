using HypoluxAdventure.Models;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Core;
using HypoluxAdventure.Models.UI;

namespace HypoluxAdventure.Managers
{
    internal class PauseManager : GameObject
    {
        public const float DEPTH = 0.5f;

        private RectangleF _rect;

        private TextObject _pauseTitle;
        private Button _continue;
        private Button _quit;

        private SpriteFont _buttonFont;
        private SpriteFont _pauseTitleFont;


        public PauseManager(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _rect = new RectangleF(0, 0, Application.SCREEN_WIDTH, Application.SCREEN_HEIGHT);

            _buttonFont = game.Content.Load<SpriteFont>("Font/PauseMenuFont");
            _pauseTitleFont = game.Content.Load<SpriteFont>("Font/PauseTitleFont");

            Vector2 halfScreenDim = Application.ScreenDimensions * 0.5f;

            _pauseTitle = new TextObject(_pauseTitleFont, "PAUSE", new Vector2(halfScreenDim.X, 200));
            _continue = new Button(game, _buttonFont, "CONTINUER", new Vector2(halfScreenDim.X, halfScreenDim.Y - 30), () => { gameManager.SwitchPause(); });
            _quit = new Button(game, _buttonFont, "QUITTER", new Vector2(halfScreenDim.X, halfScreenDim.Y + 70), () => { gameManager.ReturnToMenu(); });

            _pauseTitle.Depth = DEPTH;
            _continue.Depth = DEPTH;
            _quit.Depth = DEPTH;

            _continue.Border = _quit.Border = 3;

            _continue.Padding = _quit.Padding = new Vector2(5);

            ChangeButtonColor(_continue);
            ChangeButtonColor(_quit);

        }

        public override void Draw()
        {
            game.UICanvas.FillRectangle(_rect, Color.Black*0.5f, 0.3f);
            _pauseTitle.Draw(game.UICanvas);
            _continue.Draw();
            _quit.Draw();
        }

        public override void Update()
        {
            _continue.Update();
            _quit.Update();
            
            
        }

        private void ChangeButtonColor(Button button)
        {
            button.ColorNormal = new ButtonColor
            {
                TextColor = Color.White,
                BackgroundColor = Color.DarkGreen,
                BorderColor = Color.DarkOliveGreen
            };

            button.ColorHover = new ButtonColor
            {
                TextColor = Color.White,
                BackgroundColor = Color.GreenYellow,
                BorderColor = Color.Green
            };

            button.ColorDown = new ButtonColor
            {
                TextColor = Color.Black,
                BackgroundColor = Color.LightGray,
                BorderColor = Color.DarkGray
            };
        }
    }
}
