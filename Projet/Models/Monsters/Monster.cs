using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Monsters
{
    internal abstract class Monster : Entity
    {
        protected Room room;

        /// <summary>The spawn position of the monster, relative to the room.</summary>
        protected Vector2 spawnPosition { get; private set; }

        /// <summary>Whether or not the monster should reload if its room is reloaded.</summary>
        public bool IsSlained = false;

        protected Monster(Game1 game, GameManager gameManager, Room room, Vector2 spawnPos) : base(game, gameManager)
        {
            this.room = room;
            spawnPosition = spawnPos;
        }

        abstract public void Spawn();
    }
}
