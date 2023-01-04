using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Screens
{
    internal abstract class AbstractScreen : GameScreen
    {
        protected new Game1 Game => (Game1)base.Game;

        protected AbstractScreen(Game1 game) : base(game) { }
    }
}
