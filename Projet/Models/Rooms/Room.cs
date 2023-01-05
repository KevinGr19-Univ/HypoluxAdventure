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

namespace HypoluxAdventure.Models.Rooms
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

        private Game1 _game;
        private RoomManager _roomManager;
        private int _roomX, _roomY;

        private int[,] _tiles;

        public RoomOpening Openings { get; private set; }
        public bool HasOpening(RoomOpening opening) => (Openings | opening) == Openings;

        public Room(Game1 game, RoomManager roomManager, int roomX, int roomY, RoomOpening openings)
        {
            _game = game;

            _roomManager = roomManager;
            Openings = openings;

            _roomX = roomX;
            _roomY = roomY;
            Rectangle = new RectangleF(_roomX * ROOM_WIDTH, _roomY * ROOM_WIDTH, ROOM_WIDTH, ROOM_WIDTH);
        }

        public void GenerateTiles()
        {
            const int S_TOP = 16;
            const int S_END = 37;

            _tiles = new int[ROOM_SIZE, ROOM_SIZE];

            // Room center
            for (int j = 3; j < ROOM_SIZE - 3; j++) for (int i = 3; i < ROOM_SIZE - 3; i++)
                    _tiles[j, i] = 1;

            // Top-down borders
            for(int i = 3; i < ROOM_SIZE - 3; i++)
            {
                _tiles[2, i] = 2;
                _tiles[ROOM_SIZE - 3, i] = 4;
            }

            // Left-right borders
            for(int j = 3; j < ROOM_SIZE - 3; j++)
            {
                _tiles[j, 2] = 3;
                _tiles[j, ROOM_SIZE - 3] = 5;
            }

            // Angles
            _tiles[2, 2] = 10;
            _tiles[ROOM_SIZE - 3, 2] = 11;
            _tiles[ROOM_SIZE - 3, ROOM_SIZE - 3] = 12;
            _tiles[2, ROOM_SIZE - 3] = 13;

            

            if (HasOpening(RoomOpening.North))
                _tiles.Fusion(
                        new int[,]
                        {
                            { 3, 1, 1, 1, 1, 1, 1, 5 },
                            { 3, 1, 1, 1, 1, 1, 1, 5 },
                            { 6, 1, 1, 1, 1, 1, 1, 9 },
                        },
                        S_TOP,
                        0
                    );

            if (HasOpening(RoomOpening.West))
                _tiles.Fusion(
                        new int[,]
                        {
                            { 2, 2, 6 },
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 4, 4, 7 },
                        },
                        0,
                        S_TOP
                    );

            if (HasOpening(RoomOpening.South))
                _tiles.Fusion(
                        new int[,]
                        {
                            { 7, 1, 1, 1, 1, 1, 1, 8 },
                            { 3, 1, 1, 1, 1, 1, 1, 5 },
                            { 3, 1, 1, 1, 1, 1, 1, 5 },
                        },
                        S_TOP,
                        S_END
                    );

            if (HasOpening(RoomOpening.East))
                _tiles.Fusion(
                        new int[,]
                        {
                            { 9, 2, 2 },
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 1, 1, 1 },
                            { 8, 4, 4 },
                        },
                        S_END,
                        S_TOP
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

            Vector2 topLeft = Position;

            for (int j = 0; j < _tiles.GetLength(1); j++) for (int i = 0; i < _tiles.GetLength(0); i++)
                {
                    if (!RoomManager.GetTileFrame(_tiles[j, i], out Rectangle sourceRect)) continue;
                    Rectangle destRect = new Rectangle(i * TILE_SIZE + _roomX * ROOM_WIDTH, j * TILE_SIZE + _roomY * ROOM_WIDTH, TILE_SIZE, TILE_SIZE);

                    _game.BackgroundCanvas.Draw(_roomManager.TileSet, destRect, sourceRect, Color.White);
                }
        }

    }
}
