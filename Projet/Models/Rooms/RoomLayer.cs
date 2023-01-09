﻿using HypoluxAdventure.Utils;
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
        public static readonly RoomLayer BossRoom = new RoomLayer(
                new int[0,0],
                Point.Zero,
                new Point[] {new Point(19)},
                new Point[0],
                new Point[0]
            );

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

            new RoomLayer(
                    new int[,]
                    {
                       {8,4,4,4,4,7,1,1,1,1,1,1,1,1,1,1,1,1,8,4,4,4,4,7},
                       {5,0,0,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,3},
                       {5,0,0,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,3},
                       {5,0,0,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,3},
                       {5,0,0,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,3},
                       {9,2,2,2,2,6,1,1,1,1,1,1,1,1,1,1,1,1,9,2,2,2,2,6},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {8,4,4,4,4,7,1,1,1,1,1,1,1,1,1,1,1,1,8,4,4,4,4,7},
                       {5,0,0,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,3},
                       {5,0,0,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,3},
                       {5,0,0,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,3},
                       {5,0,0,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,3},
                       {9,2,2,2,2,6,1,1,1,1,1,1,1,1,1,1,1,1,9,2,2,2,2,6},
                    },
                    new Point(8, 7),
                    new Point[]
                    {
                        new Point(5, 5),
                        new Point(5, 34),
                        new Point(34, 5),
                        new Point(34, 34),
                        new Point(26, 18),
                        new Point(19, 13),
                        new Point(13, 19),
                        new Point(18, 26),
                    },
                    new Point[]
                    {
                        new Point(7, 10),
                        new Point(28, 14),
                        new Point(16, 28),
                        new Point(25, 29),
                    },
                    new Point[]
                    {
                        new Point(19, 19),
                        new Point(19, 20),
                        new Point(20, 19),
                        new Point(20, 20),
                    }
                ),

            new RoomLayer(
                    new int[,]
                    {
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,8,4,4,4,4,4,4,4,7},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,0,0,0,3},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,0,0,0,3},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,0,0,0,3},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,0,0,0,3},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,0,0,0,3},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,5,0,0,0,0,0,0,0,3},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,9,2,2,2,2,2,2,2,6},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,8,4,4,7,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,5,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,5,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,5,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,5,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,5,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,5,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,5,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {1,1,1,1,1,5,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {8,4,4,4,4,12,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,1},
                       {5,0,0,0,0,0,0,0,3,1,1,1,1,1,1,1,1,1,1,8,4,7},
                       {5,0,0,0,0,0,0,0,3,1,1,1,1,1,1,1,1,1,1,5,0,3},
                       {9,2,2,2,2,2,2,2,6,1,1,1,1,1,1,1,1,1,1,9,2,6},

                    },
                    new Point(8, 8),
                    new Point[]
                    {
                        new Point(10, 13),
                        new Point(10, 24),
                        new Point(34, 10),
                        new Point(26, 17),
                        new Point(22, 23),
                        new Point(30, 24),
                        new Point(32, 33),
                        new Point(11, 34),
                    },
                    new Point[]
                    {
                        new Point(13, 27),
                        new Point(24, 16),
                        new Point(24, 22),
                        new Point(28, 32),
                    },
                    new Point[]
                    {
                        new Point(8, 8),
                        new Point(9, 21),
                        new Point(27, 27),
                        new Point(20, 20),
                    }
                ),
            //a:10 b:11 c:12 d:13
            new RoomLayer(
                    new int[,]
                    {
                       {1,1,1,1,1,1,1,8,4,4,4,4,4,4,7,1,1,1,1,1,1,1},
                       {1,1,1,1,1,8,4,12,0,0,0,0,0,0,11,4,7,1,1,1,1,1},
                       {1,1,1,1,8,12,0,0,0,0,0,0,0,0,0,0,11,7,1,1,1,1},
                       {1,1,1,8,12,0,0,0,0,0,0,0,0,0,0,0,0,11,7,1,1,1},
                       {1,1,8,12,0,0,0,0,0,0,0,0,0,0,0,0,0,0,11,7,1,1},
                       {1,8,12,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,11,7,1},
                       {1,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1},
                       {8,12,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,11,7},
                       {5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                       {5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                       {5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                       {5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                       {5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                       {5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                       {9,13,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,6},
                       {1,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1},
                       {1,9,13,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,6,1},
                       {1,1,9,13,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,6,1,1},
                       {1,1,1,9,13,0,0,0,0,0,0,0,0,0,0,0,0,10,6,1,1,1},
                       {1,1,1,1,9,13,0,0,0,0,0,0,0,0,0,0,10,6,1,1,1,1},
                       {1,1,1,1,1,9,2,13,0,0,0,0,0,0,10,2,6,1,1,1,1,1},
                       {1,1,1,1,1,1,1,9,2,2,2,2,2,2,6,1,1,1,1,1,1,1},

                    },
                    new Point(9, 9),
                    new Point[]
                    {
                        new Point(10, 6),
                        new Point(29, 6),
                        new Point(6, 10),
                        new Point(33, 10),
                        new Point(6, 29),
                        new Point(33, 29),
                        new Point(10, 33),
                        new Point(29, 33),
                    },
                    new Point[]
                    {
                        new Point(3, 3),
                        new Point(36, 3),
                        new Point(3, 36),
                        new Point(36, 36),
                    },
                    new Point[]
                    {
                        new Point(8, 8),
                        new Point(31, 8),
                        new Point(8, 31),
                        new Point(31, 31),
                    }
                ),

        };

        public static RoomLayer GetRandomRoomLayer() => _roomLayers[new Random().Next(0, _roomLayers.Length)];

        private const float MIN_MONSTER_SPAWN_RATE = 0.5f;
        private const float MAX_MONSTER_SPAWN_RATE = 0.9f;

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
            float fAmount = _monsterSpawns.Length * MathUtils.Lerp(MIN_MONSTER_SPAWN_RATE, MAX_MONSTER_SPAWN_RATE, difficulty);
            int amount = (int) MathF.Round(fAmount);
            return _monsterSpawns.TakeRandom(amount);
        }

        public Point SpawnChest() => _chestSpawns[new Random().Next(0, _chestSpawns.Length)];
        public Point SpawnExit() => _exitSpawns[new Random().Next(0, _chestSpawns.Length)];

    }
}
