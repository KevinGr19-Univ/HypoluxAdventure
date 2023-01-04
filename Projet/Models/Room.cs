using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
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
        public const int ROOM_SIZE = 32;

        public bool HasOpening(RoomOpening opening) => (Openings | opening) == Openings;

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

        private RoomManager _roomManager;
        private int _roomX, _roomY;

        public RoomOpening Openings { get; private set; }

        public Room(RoomManager roomManager, int roomX, int roomY)
        {
            _roomManager = roomManager;
            _roomX = roomX;
            _roomY = roomY;
            Openings = RoomOpening.North | RoomOpening.West;
        }

        public void RegisterTiles()
        {
            
        }

        public void Update()
        {
            
        }

        public void Draw()
        {

        }
        
    }
}
