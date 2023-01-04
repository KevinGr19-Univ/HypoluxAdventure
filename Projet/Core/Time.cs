using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Core
{
    internal static class Time
    {
        private static GameTime _updateGameTime;
        private static GameTime _drawGameTime;

        public static void Update(GameTime gameTime)
        {
            _updateGameTime = gameTime;
        }

        public static void Draw(GameTime gameTime)
        {
            _drawGameTime = gameTime;
        }

        public static float DrawDeltaTime => (float)_drawGameTime.ElapsedGameTime.TotalSeconds;

        public static float TimeScale = 1;
        public static float RealDeltaTime => (float)_updateGameTime.ElapsedGameTime.TotalSeconds;
        public static float DeltaTime => RealDeltaTime * TimeScale;
    }
}
