using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Rooms
{
    internal class RoomLayer
    {
        private static readonly RoomLayer[] _roomLayers = new RoomLayer[]
        {
            new RoomLayer(
                    new int[,]
                    {
                        { 8, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 7 },
                        { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
                        { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
                        { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
                        { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
                        { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
                        { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
                        { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
                        { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
                        { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
                        { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
                        { 9, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 6 },
                    },
                    new Point(14, 14),
                    new Point[]
                    {
                        new Point(8, 8),
                        new Point(8, 31),
                        new Point(31, 8),
                        new Point(31, 31),
                        new Point(19, 12),
                        new Point(12, 20),
                        new Point(20, 27),
                        new Point(27, 19),
                    },
                    new Point[]
                    {
                        new Point(3, 3),
                        new Point(3, 36),
                        new Point(36, 3),
                        new Point(36, 36),
                    },
                    new Point[]
                    {
                        new Point(20, 13),
                        new Point(13, 19),
                        new Point(19, 26),
                        new Point(26, 20),
                    }
                ),
        };

        public static RoomLayer GetRandomRoomLayer() => _roomLayers[new Random().Next(0, _roomLayers.Length)];

        private const float MIN_MONSTER_SPAWN_RATE = 0.5f;

        private int[,] _layer;
        private Point _topLeftLayerPos;
        private Point[] _monsterSpawns;
        private Point[] _chestSpawns;
        private Point[] _exitSpawns;

        public RoomLayer(int[,] layer, Point topLeft, Point[] monsterSpawns, Point[] chestSpawns, Point[] exitSpawns)
        {
            _layer = layer;
            _topLeftLayerPos = topLeft;
            _monsterSpawns = monsterSpawns;
            _chestSpawns = chestSpawns;
            _exitSpawns = exitSpawns;
        }

        public void FusionLayer(int[,] baseMap)
        {
            baseMap.Fusion(_layer, _topLeftLayerPos.X, _topLeftLayerPos.Y);
        }

        public Point[] SpawnMonsters(float difficulty)
        {
            int amount = 1;//(int)(_monsterSpawns.Length * MathUtils.Lerp(MIN_MONSTER_SPAWN_RATE, 1, difficulty));
            return _monsterSpawns.TakeRandom(amount);
        }

        public Point SpawnChest() => _chestSpawns[new Random().Next(0, _chestSpawns.Length)];
        public Point SpawnExit() => _exitSpawns[new Random().Next(0, _chestSpawns.Length)];

    }
}
