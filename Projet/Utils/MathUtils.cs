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
        public static float Damp(float current, float target, float smoothRate, float deltaTime)
            => Lerp(current, target, (float)Math.Pow(deltaTime, smoothRate));

        public static Vector2 Damp(Vector2 current, Vector2 target, float smoothRate, float deltaTime)
        {
            float lerpCoef = MathF.Pow(deltaTime, smoothRate);
            float x = Lerp(current.X, target.X, lerpCoef);
            float y = Lerp(current.Y, target.Y, lerpCoef);
            return new Vector2(x, y);
        }

        public static float InverseLerp(float a, float b, float value)
        {
            if (a == b) return 1;
            return (value - a) / (b - a);
        }

        public static float LerpOutCubic(float a, float b, float time, float currentTime)
        {
            if (time == 0) return b;

            float lerpCoef = (float) Math.Clamp(currentTime / time, 0, 1);
            return Lerp(a, b, 1 - MathF.Pow(lerpCoef, 3));
        }
    }
}
