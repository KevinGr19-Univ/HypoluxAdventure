using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Utils
{
    internal static class MathUtils
    {
        public static float Lerp(float a, float b, float t) => a * (1 - t) + b * t;
        public static float Damp(float current, float target, float smoothTime, float deltaTime)
            => Lerp(current, target, (float)Math.Pow(smoothTime, deltaTime));
    }
}
