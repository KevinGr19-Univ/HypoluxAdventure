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
        public static float Damp(float current, float target, float smmothSpeed, float deltaTime)
            => Lerp(current, target, (float)Math.Pow(deltaTime, smmothSpeed));

        public static float InverseLerp(float a, float b, float value)
        {
            if (a == b) return 1;
            return (value - a) / (b - a);
        }
    }
}
