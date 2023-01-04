using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
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

        // TODO: List of enemies
        // TODO: Chest? or List of chests

        public void Update(bool transitionOut)
        {
            
        }

        public void Draw()
        {

        }
        
    }
}
