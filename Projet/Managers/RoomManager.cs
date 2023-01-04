using HypoluxAdventure.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Managers
{
    internal class RoomManager : GameObject
    {
        private Texture2D _tileset;

        private Room[,] _rooms;
        private int[,] _tiles;

        public RoomManager(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _tileset = game.Content.Load<Texture2D>("img/tileset");
            GenerateRooms();
        }

        public void GenerateRooms()
        {
            _rooms = new Room[4, 4];
            _rooms[0, 0] = new Room(this, 0, 0);
            _rooms[0, 0].RegisterTiles();

            int tileMapLength = _rooms.Length * Room.ROOM_SIZE;
            _tiles = new int[tileMapLength, tileMapLength];
        }

        public void SetTile(int x, int y, int value) => _tiles[x, y] = value;
        public void SetRoomTiles(int[,] tiles, int roomX, int roomY)
        {
            int x = roomX * Room.ROOM_SIZE;
            int y = roomY * Room.ROOM_SIZE;

            if (x + tiles.GetLength(0) >= tiles.GetLength(0) || y + tiles.GetLength(1) >= tiles.GetLength(1))
                throw new ArgumentException("Tile out of range");

            for (int i = 0; i < tiles.GetLength(0); i++) for (int j = 0; j < tiles.GetLength(1); j++)
                    _tiles[i + x, j + y] = tiles[i, j];
        }

        public override void Update()
        {
            
        }

        public override void Draw()
        {

        }
    }
}
