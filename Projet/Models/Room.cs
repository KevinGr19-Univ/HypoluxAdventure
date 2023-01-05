using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Monsters;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models
{
    public enum RoomOpening
    {
        North = 1,
        West = 2,
        South = 4,
        East = 8
    }

    internal class Room
    {
        #region Room Openings Utils
        public static RoomOpening FlipOpening(RoomOpening opening)
        {
            return opening switch
            {
                RoomOpening.North => RoomOpening.South,
                RoomOpening.West => RoomOpening.East,
                RoomOpening.South => RoomOpening.North,
                _ => RoomOpening.West
            };
        }
        #endregion

        public const int ROOM_SIZE = 40;
        public const int TILE_SIZE = 32;

        public const int ROOM_WIDTH = ROOM_SIZE * TILE_SIZE;

        private RoomManager _roomManager;
        private int _roomX, _roomY;

        private int[,] _tiles;

        public RoomOpening Openings { get; private set; }
        public bool HasOpening(RoomOpening opening) => (Openings | opening) == Openings;

        public Room(RoomManager roomManager, int roomX, int roomY, RoomOpening openings)
        {
            _roomManager = roomManager;
            Openings = openings;

            _roomX = roomX;
            _roomY = roomY;
            Rectangle = new RectangleF(_roomX * ROOM_WIDTH, _roomY * ROOM_WIDTH, ROOM_WIDTH, ROOM_WIDTH);
        }

        public void GenerateTiles()
        {
            _tiles = new int[ROOM_SIZE, ROOM_SIZE];
            for (int i = 3; i < ROOM_SIZE - 3; i++) for (int j = 3; j < ROOM_SIZE - 3; j++)
                    _tiles[i, j] = 1;

            if (HasOpening(RoomOpening.North))
                _tiles.Fusion(
                        new int[,]
                        {
                            { 1, 1, 1, 1},
                            { 1, 1, 1, 1},
                            { 1, 1, 1, 1},
                        },
                        18,
                        0
                    );

            if (HasOpening(RoomOpening.West))
                _tiles.Fusion(
                        new int[,]
                        {
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 1, 1, 1 }
                        },
                        0,
                        18
                    );

            if (HasOpening(RoomOpening.South))
                _tiles.Fusion(
                        new int[,]
                        {
                            { 1, 1, 1, 1},
                            { 1, 1, 1, 1},
                            { 1, 1, 1, 1},
                        },
                        18,
                        36
                    );

            if (HasOpening(RoomOpening.East))
                _tiles.Fusion(
                        new int[,]
                        {
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                        },
                        36,
                        16
                    );
        }

        public Room GetNextRoom(RoomOpening opening)
        {
            return opening switch
            {
                RoomOpening.North => _roomManager.GetRoom(_roomX, _roomY - 1),
                RoomOpening.South => _roomManager.GetRoom(_roomX, _roomY + 1),
                RoomOpening.West => _roomManager.GetRoom(_roomX - 1, _roomY),
                RoomOpening.East => _roomManager.GetRoom(_roomX + 1, _roomY),
                _ => throw new ArgumentException("Tried to get neighbor room with custom RoomOpening")
            };
        }

        public RectangleF Rectangle { get; private set; }
        public Vector2 Position => Rectangle.TopLeft;

        private List<Monster> _monsters = new List<Monster>(0);
        // TODO: Room projectiles
        // TODO: Chest? or List of chests

        /// <summary>Method called when the room is entered by the player.</summary>
        public void Load()
        {
            _monsters.ForEach((monster) =>
            {
                if (monster.IsSlained) return;
                monster.Spawn();
            });
        }

        /// <summary>Method called when the room is left by the player.</summary>
        public void Unload()
        {

        }

        public void Update(bool transitionOut)
        {
            if (!transitionOut)
            {
                _monsters.ForEach((monster) =>
                {
                    if (monster.IsSlained) return;
                    monster.Update();
                });
            }
        }

        public void Draw()
        {
            _monsters.ForEach((monster) =>
            {
                if (monster.IsSlained) return;
                monster.Draw();
            });
        }
        
    }
}
