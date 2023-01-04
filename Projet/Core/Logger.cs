using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Core
{
    internal static class Logger
    {
        public static bool DisplayDebug = true;

        public static void Log(object obj) => Log(obj.ToString());
        public static void Log(string msg) => Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {msg}");

        public static void Debug(object obj) => Debug(obj.ToString());
        public static void Debug(string msg)
        {
            if (DisplayDebug) Log($"[DEBUG] {msg}");
        }

        public static void Warn(object obj) => Warn(obj.ToString());
        public static void Warn(string msg)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Log($"[WARNING] {msg}");
            Console.ForegroundColor = color;
        }
    }
}
