using HypoluxAdventure.Core;
using HypoluxAdventure.Models;
using HypoluxAdventure.Models.Rooms;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                { 0, GetRectangle(1) }, // Void
                { 1, GetRectangle(0) }, // Ground
                { 2, GetRectangle(11) }, // 2 to 5 : Wall
                { 3, GetRectangle(12) },
                { 4, GetRectangle(13) },
                { 5, GetRectangle(10) },
                { 6, GetRectangle(8) }, // 6 to 9 : Angle
                { 7, GetRectangle(7) },
                { 8, GetRectangle(6) },
                { 9, GetRectangle(9) },
                { 10, GetRectangle(3) }, // 10 to 13 : Corner
                { 11, GetRectangle(4) },
                { 12, GetRectangle(5) },
                { 13, GetRectangle(2) },
            };
        }

        public const int TILESET_TILE_SIZE = 16;
        public Texture2D TileSet { get; private set; }

        public const int MAP_ROOM_WIDTH = 7;

        public static readonly Point StartingPos = new Point(MAP_ROOM_WIDTH / 2);

        public const int MAP_WIDTH = MAP_ROOM_WIDTH * Room.ROOM_WIDTH;
        public const int MAP_HEIGHT = MAP_ROOM_WIDTH * Room.ROOM_WIDTH;

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
            const int MIN_SPAWN_DIST_STOP = 3;
            const int MAX_SPAWN_DIST_STOP = 7;

            const int MIN_SPAWN_DIST_CHEST = 3;
            const float CHEST_CHANCE = 0.5f;
            const float CHEST_REDUCE_COEF = 0.5f;

            const float CHANGE_DIRECTION_CHANCE = 0.5f;
            const int MIN_SPAWN_DIST_BRANCH = 3;
            const float BRANCHING_CHANCE = 0.6f;
            const float BRANCHING_REDUCE_COEF = 0.5f;
            const float BRANCHING_CHANCE_FULL = 1f;
            int branchCount = 0;

            Random r = new Random(); // Seedable

            _rooms = new Room[MAP_ROOM_WIDTH, MAP_ROOM_WIDTH];

            // Place starting room
            Room startingRoom = new Room(game, gameManager, this, StartingPos.X, StartingPos.Y);
            startingRoom.directions = new List<Point>(Room.Directions);
            startingRoom.SpawnDistance = 0;
            AddRoom(startingRoom);

            CurrentRoom = startingRoom;

            // Place neighbor rooms
            List<Room> roomsToList = new List<Room>() { startingRoom };
            List<Room> chestRooms = new List<Room>();
            Queue<Room> rooms = new Queue<Room>();

            void AddRoomWithDirection(Room parentRoom, Point dir)
            {
                Point pos = parentRoom.PointPos + dir;

                Room newRoom = new Room(game, gameManager, this, pos.X, pos.Y);
                newRoom.RawSpawnDistance = parentRoom.RawSpawnDistance + 1;

                newRoom.spawnDir = new Point(-dir.X, -dir.Y); // If spawned from left, seal right for positions, and vice-versa
                newRoom.AddDirection(newRoom.spawnDir);

                AddRoom(newRoom);
                roomsToList.Add(newRoom);
                rooms.Enqueue(newRoom);

                parentRoom.AddDirection(dir);
            }

            void AddRoomOrBranch(Room parentRoom, Point dir)
            {
                Room room = GetRoom(parentRoom.PointPos +  dir);

                if (room == null) AddRoomWithDirection(parentRoom, dir);
                else
                {
                    room.AddDirection(new Point(-dir.X, -dir.Y));
                    parentRoom.AddDirection(dir);
                }
            }

            foreach (Point dir in Room.Directions) AddRoomWithDirection(startingRoom, dir);
            
            // Queue loop
            while(rooms.TryDequeue(out Room room))
            {
                // Generate chest
                if(room.RawSpawnDistance >= MIN_SPAWN_DIST_CHEST)
                {
                    float chestChance = CHEST_CHANCE * MathF.Pow(CHEST_REDUCE_COEF, chestRooms.Count);
                    if (r.NextSingle() < chestChance) chestRooms.Add(room);
                }

                // Stop generating further
                float stopChance = MathUtils.InverseLerp(MIN_SPAWN_DIST_STOP, MAX_SPAWN_DIST_STOP, room.RawSpawnDistance);
                stopChance = (float)Math.Clamp(stopChance, 0, 1);

                if (r.NextSingle() < stopChance) continue;

                // Available positions
                List<Point> availablePos = new List<Point>();
                foreach(Point dir in Room.Directions)
                {
                    if (dir == room.spawnDir) continue;

                    Point nextPos = room.PointPos + dir;
                    if (IsOutOfBounds(nextPos.X, nextPos.Y)) continue;

                    availablePos.Add(nextPos);
                }

                // Free positions to add new room
                List<Point> freePos = new List<Point>();
                foreach(Point pos in availablePos) if (GetRoom(pos) == null) freePos.Add(pos);

                if(freePos.Count > 0)
                {
                    Point frontPos = room.PointPos - room.spawnDir;
                    Point nextPos;

                    if(freePos.Contains(frontPos) && r.NextSingle() > CHANGE_DIRECTION_CHANCE) nextPos = frontPos;
                    else nextPos = freePos[r.Next(0, freePos.Count)];
                    
                    Point dir = nextPos - room.PointPos;

                    AddRoomWithDirection(room, dir);
                    availablePos.Remove(nextPos);
                }

                // Branching
                if (availablePos.Count == 0 || room.RawSpawnDistance < MIN_SPAWN_DIST_BRANCH) continue;
                float branchChance = BRANCHING_CHANCE * (float)Math.Pow(BRANCHING_REDUCE_COEF, branchCount);

                if (r.NextSingle() > branchChance) continue;
                branchCount++;

                // Try add rew room to random neighbor tile
                Point branchPos = availablePos[r.Next(0, availablePos.Count)];
                availablePos.Remove(branchPos);
                AddRoomOrBranch(room, branchPos - room.PointPos);

                // Try add the remaining neighbor room tile
                if (availablePos.Count > 0 && r.NextSingle() < BRANCHING_CHANCE_FULL)
                {
                    branchPos = availablePos[0];
                    AddRoomOrBranch(room, branchPos - room.PointPos);
                }
            }

            // Calculate true spawn distance
            void CalculateSpawnDistance(Room room, int previousDistance)
            {
                int trueDistance = previousDistance + 1;

                // Found shorter path
                if (room.SpawnDistance <= trueDistance) return;
                
                room.SpawnDistance = trueDistance;
                foreach(Point dir in room.directions)
                {
                    if (dir == room.spawnDir) continue;
                    CalculateSpawnDistance(GetRoom(room.PointPos + dir), trueDistance);
                }
            }

            foreach (Point dir in startingRoom.directions) CalculateSpawnDistance(GetRoom(StartingPos + dir), 0);

            // Init rooms
            foreach (Room room in roomsToList)
            {
                room.GenerateTiles();

                if (room == startingRoom) continue;
                room.GenerateMonsters();
            }
            foreach (Room room in chestRooms) room.SummonChest();
            startingRoom.SummonChest(); // DEBUG

            // Exit room
            int maxSpawnDist = 0;
            List<Room> potentialExitRooms = new List<Room>();

            foreach(Room room in roomsToList)
            {
                if (room.SpawnDistance == maxSpawnDist) potentialExitRooms.Add(room);
                else if(room.SpawnDistance > maxSpawnDist)
                {
                    maxSpawnDist = room.SpawnDistance;
                    potentialExitRooms.Clear();
                    potentialExitRooms.Add(room);
                }
            }

            Room exitRoom = potentialExitRooms[r.Next(0, potentialExitRooms.Count)];
            exitRoom.SpawnExit();

            // DEBUG
            for (int row = 0; row < MAP_ROOM_WIDTH; row++)
            {
                for (int col = 0; col < MAP_ROOM_WIDTH; col++)
                {
                    Room room = GetRoom(col, row);
                    Console.Write((room == null ? " " : room.SpawnDistance) + "|");
                }
                Console.WriteLine("\n");
            }

            CurrentRoom.Load();
        }

        public void GenerateBossRoom()
        {
            _rooms = new Room[MAP_ROOM_WIDTH, MAP_ROOM_WIDTH];

            Room startingRoom = new Room(game, gameManager, this, StartingPos.X, StartingPos.Y);
            CurrentRoom = startingRoom;

            AddRoom(startingRoom);

            startingRoom.RoomLayer = RoomLayer.BossRoom;
            startingRoom.GenerateTiles();
            startingRoom.GenerateMonsters();

            gameManager.Player.Position = new Vector2(19, 35) * Room.TILE_SIZE + CurrentRoom.Position;

            CurrentRoom.Load();
        }

        public void SpawnPlayer()
        {
            float distFromSide = 5 * Room.TILE_SIZE;
            float halfDim = Room.ROOM_WIDTH * 0.5f;

            Vector2 spawnPosition = new Random().Next(0, 4) switch
            {
                0 => new Vector2(halfDim, distFromSide),
                1 => new Vector2(distFromSide, halfDim),
                2 => new Vector2(halfDim, Room.ROOM_WIDTH - distFromSide),
                _ => new Vector2(Room.ROOM_WIDTH - distFromSide, halfDim)
            } + CurrentRoom.Position;

            gameManager.Player.Position = spawnPosition;
        }

        public void AddRoom(Room room)
        {
            _rooms[room.Y, room.X] = room;
        }

        public Room GetRoom(int roomX, int roomY)
        {
            if(IsOutOfBounds(roomX, roomY)) return null;
            return _rooms[roomY, roomX];
        }

        public bool IsOutOfBounds(int roomX, int roomY)
            => roomX < 0 || roomY < 0 || roomX >= MAP_ROOM_WIDTH || roomY >= MAP_ROOM_WIDTH;

        public Room GetRoom(Point point) => GetRoom(point.X, point.Y);

        public void PlayerRoomExitsCollision()
        {
            Player player = gameManager.Player;
            RectangleF roomRect = CurrentRoom.Rectangle;

            void TryExit(int x, int y, float rectSideValue, ref float position, ref float velocity)
            {
                if (_changeRoomTimer <= 0 && !StayInRoom) SwitchRoom(CurrentRoom.GetNextRoom(x, y));
                else
                {
                    velocity = 0;
                    position = rectSideValue;
                }
            }

            if (player.Velocity.X < 0 && player.Position.X < roomRect.Left)
                TryExit(-1, 0, roomRect.Left, ref player.Position.X, ref player.Velocity.X);

            else if (player.Velocity.X > 0 && player.Position.X > roomRect.Right)
                TryExit(1, 0, roomRect.Right, ref player.Position.X, ref player.Velocity.X);

            else if (player.Velocity.Y < 0 && player.Position.Y < roomRect.Top)
                TryExit(0, -1, roomRect.Top, ref player.Position.Y, ref player.Velocity.Y);

            else if (player.Velocity.Y > 0 && player.Position.Y > roomRect.Bottom)
                TryExit(0, 1, roomRect.Bottom, ref player.Position.Y, ref player.Velocity.Y);
        }

        public void SwitchRoom(Room nextRoom)
        {
            if(nextRoom == null)
            {
                Logger.Warn("Attempt to switch current room for non-existant room");
                return;
            }

            _previousRoom?.ProjectileHolder.Clear();
            _previousRoom = CurrentRoom;

            CurrentRoom = nextRoom;
            CurrentRoom.Load();

            gameManager.MinimapOverlay.Visit(CurrentRoom.PointPos);
            _changeRoomTimer = CHANGE_ROOM_COOLDOWN;
        }

        public override void Update()
        {
            PlayerRoomExitsCollision();

            CurrentRoom.Update(false);
            if(_changeRoomTimer > 0)
            {
                _changeRoomTimer -= Time.RealDeltaTime;

                if (_changeRoomTimer <= 0) _previousRoom.Unload();
                else _previousRoom.Update(true);
            }
        }

        public override void Draw()
        {
            if (_changeRoomTimer > 0) _previousRoom.Draw();
            CurrentRoom.Draw();
        }
    }
}
