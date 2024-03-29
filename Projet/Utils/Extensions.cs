﻿using HypoluxAdventure.Core;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Utils
{
    public static class Extensions
    {
        public static void Fusion(this int[,] baseMap, int[,] map, int x, int y)
        {
            if (map.Rank != baseMap.Rank)
                throw new ArgumentException("Arrays do not have the same rank");

            if (y + map.GetLength(0) - 1 > baseMap.GetLength(0) || x + map.GetLength(1) - 1 > baseMap.GetLength(1))
                throw new ArgumentOutOfRangeException("Array out of range");

            for (int j = 0; j < map.GetLength(0); j++) for (int i = 0; i < map.GetLength(1); i++)
                    baseMap[j + y, i + x] = map[j, i];
                    
        }

        public static RectangleF Scale(this RectangleF rectangle, Vector2 scale)
            => new RectangleF(rectangle.Center - rectangle.Size * scale * 0.5f, rectangle.Size * scale);

        public static T[] TakeRandom<T>(this T[] array, int amount)
        {
            if (amount < 0 || amount > array.Length)
                throw new ArgumentException("Invalid amount to take random elements");

            T[] values = new T[amount];
            List<int> indexes = Enumerable.Range(0, array.Length).ToList();

            for(int i = 0; i < amount; i++)
            {
                int randomIndex = indexes[new Random().Next(0, indexes.Count)];
                values[i] = array[randomIndex];
                indexes.Remove(randomIndex);
            }

            return values;
        }
    }
}
