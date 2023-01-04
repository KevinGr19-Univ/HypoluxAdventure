using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Core
{
    public static class Application
    {
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;

        public const int TARGET_FRAMERATE = 60;

        public static Vector2 ScreenDimensions => new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT);
    }
}
