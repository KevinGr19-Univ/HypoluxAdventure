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
        North,
        West,
        South,
        East
    }

    internal class Room
    {
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

        public Room(RoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        public void Update()
        {
            
        }

        public void Draw()
        {

        }
        
    }
}
