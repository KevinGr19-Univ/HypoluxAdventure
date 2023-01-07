using HypoluxAdventure.Core;
using HypoluxAdventure.Models;
using HypoluxAdventure.Models.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Managers
{
    internal class RoomManager : GameObject
    {
        private static Dictionary<int, Rectangle> _tileFrames;

        public static bool GetTileFrame(int id, out Rectangle rectangle)
        {
            rectangle = new Rectangle();
            if (!_tileFrames.TryGetValue(id, out Rectangle rect)) return false;

            rectangle = rect;
            return true;
        }

        public static void CreateTileFrames()
        {
            Rectangle GetRectangle(int tileX)
                => new Rectangle(1 + tileX * (TILESET_TILE_SIZE  + 2), 1, TILESET_TILE_SIZE, TILESET_TILE_SIZE); // Tiles have 1 px of border to prevent texture bleeding

            _tileFrames = new Dictionary<int, Rectangle>
            {
                { 0, GetRectangle(1) }, // Vide
                { 1, GetRectangle(0) }, // Sol
                { 2, GetRectangle(11) }, // 2 à 5 : Mur
                { 3, GetRectangle(12) },
                { 4, GetRectangle(13) },
                { 5, GetRectangle(10) },
                { 6, GetRectangle(8) }, // 6 à 9 : Angle
                { 7, GetRectangle(7) },
                { 8, GetRectangle(6) },
                { 9, GetRectangle(9) },
                { 10, GetRectangle(3) }, // 10 à 13 : Coin
                { 11, GetRectangle(4) },
                { 12, GetRectangle(5) },
                { 13, GetRectangle(2) },
            };
        }

        public const int TILESET_TILE_SIZE = 16;
        public Texture2D TileSet { get; private set; }

        public const int MAP_ROOM_WIDTH = 7;
        public const int MAP_ROOM_HEIGHT = 7;

        public static readonly Point StartingPos = new Point(MAP_ROOM_WIDTH / 2, MAP_ROOM_HEIGHT / 2);

        public const int MAP_WIDTH = MAP_ROOM_WIDTH * Room.ROOM_WIDTH;
        public const int MAP_HEIGHT = MAP_ROOM_HEIGHT * Room.ROOM_WIDTH;

        private Room[,] _rooms;

        public Room CurrentRoom { get; private set; }
        private Room _previousRoom = null;

        private const float CHANGE_ROOM_COOLDOWN = 1.5f;
        private float _changeRoomTimer;

        public bool StayInRoom = false;

        public RoomManager(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            TileSet = game.Content.Load<Texture2D>("img/mapTextureSample");
        }

        public void GenerateRooms()
        {
            _rooms = new Room[MAP_ROOM_HEIGHT, MAP_ROOM_WIDTH];
            AddRoom(0, 0, RoomOpening.South | RoomOpening.East | RoomOpening.North);
            AddRoom(1, 0, RoomOpening.West);
            AddRoom(0, 1, RoomOpening.North);

            CurrentRoom = GetRoom(0, 0);
        }

        public void AddRoom(int x, int y, RoomOpening openings)
        {
            Room newRoom = new Room(game, this, x, y, openings);
            newRoom.GenerateTiles();

            _rooms[y, x] = newRoom;
        }

        public Room GetRoom(int roomX, int roomY)
        {
            if (roomX < 0 || roomY < 0 || roomX >= MAP_ROOM_WIDTH || roomY >= MAP_ROOM_HEIGHT) return null;
            return _rooms[roomY, roomX];
        }

        public void PlayerRoomExitsCollision()
        {
            Player player = gameManager.Player;
            RectangleF roomRect = CurrentRoom.Rectangle;

            void TryExit(RoomOpening opening, float rectSideValue, ref float position, ref float velocity)
            {
                if (_changeRoomTimer <= 0 && !StayInRoom) SwitchRoom(CurrentRoom.GetNextRoom(opening));
                else
                {
                    velocity = 0;
                    position = rectSideValue;
                }
            }

            if (player.Velocity.X < 0 && player.Position.X < roomRect.Left)
                TryExit(RoomOpening.West, roomRect.Left, ref player.Position.X, ref player.Velocity.X);

            else if (player.Velocity.X > 0 && player.Position.X > roomRect.Right)
                TryExit(RoomOpening.East, roomRect.Right, ref player.Position.X, ref player.Velocity.X);

            else if (player.Velocity.Y < 0 && player.Position.Y < roomRect.Top)
                TryExit(RoomOpening.North, roomRect.Top, ref player.Position.Y, ref player.Velocity.Y);

            else if (player.Velocity.Y > 0 && player.Position.Y > roomRect.Bottom)
                TryExit(RoomOpening.South, roomRect.Bottom, ref player.Position.Y, ref player.Velocity.Y);
        }

        public void SwitchRoom(Room nextRoom)
        {
            if(nextRoom == null)
            {
                Logger.Warn("Attempt to switch current room for non-existant room");
                return;
            }

            _previousRoom = CurrentRoom;
            CurrentRoom = nextRoom;

            _changeRoomTimer = CHANGE_ROOM_COOLDOWN;
            Logger.Debug("Switch room");
        }

        public override void Update()
        {
            PlayerRoomExitsCollision();

            CurrentRoom.Update(false);
            if(_changeRoomTimer > 0)
            {
                _changeRoomTimer -= Time.RealDeltaTime;
                _previousRoom.Update(true);
            }
        }

        public override void Draw()
        {
            if (_changeRoomTimer > 0) _previousRoom.Draw();
            CurrentRoom.Draw();
        }
    }
}
