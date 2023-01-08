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
    internal class Room
    {
        public static readonly Point[] Directions = new Point[]
        {
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 1),
            new Point(1, 0)
        };

        public const int ROOM_SIZE = 40;
        public const int TILE_SIZE = 32;

        public const int ROOM_WIDTH = ROOM_SIZE * TILE_SIZE;

        private Game1 _game;
        private GameManager _gameManager;
        private RoomManager _roomManager;

        public int X { get; private set; }
        public int Y { get; private set; }
        public Point PointPos => new Point(X, Y);

        public List<Point> directions = new List<Point>();
        public Point spawnDir;

        public int RawSpawnDistance;
        public int SpawnDistance = int.MaxValue;

        private RoomLayer _roomLayer;
        private int[,] _tiles;

        public Room(Game1 game, GameManager gameManager, RoomManager roomManager, int roomX, int roomY)
        {
            _game = game;
            _gameManager = gameManager;
            _roomManager = roomManager;

            X = roomX;
            Y = roomY;
            Rectangle = new RectangleF(X * ROOM_WIDTH, Y * ROOM_WIDTH, ROOM_WIDTH, ROOM_WIDTH);

            _roomLayer = RoomLayer.GetRandomRoomLayer();
        }

        public void AddDirection(Point dir)
        {
            if (!directions.Contains(dir)) directions.Add(dir);
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

            if (directions.Contains(Directions[0]))
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

            if (directions.Contains(Directions[1]))
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

            if (directions.Contains(Directions[2]))
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

            if (directions.Contains(Directions[3]))
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

            _roomLayer.FusionLayer(_tiles);
        }

        public Room GetNextRoom(int x, int y) => _roomManager.GetRoom(X + x, Y + y);

        public RectangleF Rectangle { get; private set; }
        public Vector2 Position => Rectangle.TopLeft;

        // Monsters
        private List<Monster> _monsters = new List<Monster>();
        public int MonsterCount => _monsters.Count;

        public void GenerateMonsters()
        {
            Point[] spawnPoints = _roomLayer.SpawnMonsters(_gameManager.Difficulty);
            foreach(Point spawnPoint in spawnPoints)
            {
                Vector2 spawnPos = spawnPoint.ToVector2() * TILE_SIZE + Position;
                _monsters.Add(RandomMonster(spawnPos));
            }
        }

        private Monster RandomMonster(Vector2 spawnPos)
        {
            return null;
        }

        public IEnumerable<Monster> GetAliveMonsters() => _monsters.Where(monster => !monster.IsDead && !monster.IsSlained);

        // Projectiles
        // TODO: Room projectiles

        // Chest
        public Chest Chest { get; private set; } = null;

        public void SummonChest()
        {
            Logger.Warn(PointPos);

            Point chestPos = _roomLayer.SpawnChest();
            Chest = new Chest(_game, _gameManager, this, chestPos);
        }

        // Exit
        public Exit Exit { get; private set; }

        public void SpawnExit()
        {
            Point pos = _roomLayer.SpawnExit();
            Exit = new Exit(_game, _gameManager, this, pos);
        }

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
            Chest?.Update();
            Exit?.Update();

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
                    Rectangle destRect = new Rectangle(i * TILE_SIZE + X * ROOM_WIDTH, j * TILE_SIZE + Y * ROOM_WIDTH, TILE_SIZE, TILE_SIZE);

                    _game.BackgroundCanvas.Draw(_roomManager.TileSet, destRect, sourceRect, Color.White);
                }

            Chest?.Draw();
            Exit?.Draw();
        }

    }
}
