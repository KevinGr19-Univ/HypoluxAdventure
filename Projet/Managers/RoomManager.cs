using HypoluxAdventure.Core;
using HypoluxAdventure.Models;
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
        private Texture2D _tileset;

        public const int MAP_ROOM_SIZE = 6;
        private Room[,] _rooms;

        public Room CurrentRoom { get; private set; }
        private Room _previousRoom = null;

        private const float CHANGE_ROOM_COOLDOWN = 1;
        private float _changeRoomTimer;

        public bool StayInRoom = false;

        public RoomManager(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _tileset = game.Content.Load<Texture2D>("img/tileset");
            GenerateRooms();
        }

        public void GenerateRooms()
        {
            _rooms = new Room[MAP_ROOM_SIZE, MAP_ROOM_SIZE];
            _rooms[0, 0] = new Room(this, 0, 0, RoomOpening.South | RoomOpening.East);
            _rooms[1, 0] = new Room(this, 1, 0, RoomOpening.North);
            _rooms[0, 1] = new Room(this, 0, 1, RoomOpening.West);

            CurrentRoom = _rooms[0, 0];
        }

        public Room GetRoom(int roomX, int roomY)
        {
            if (roomX < 0 || roomY < 0 || roomX >= MAP_ROOM_SIZE || roomY >= MAP_ROOM_SIZE) return null;
            return _rooms[roomX, roomY];
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
        }
    }
}
