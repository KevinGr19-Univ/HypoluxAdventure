using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models
{
    internal struct FrameInputs
    {
        public int X { get; init; }
        public int Y { get; init; }
        public bool Shoot { get; init; }
        public bool Use { get; init; }
        public bool DropItem { get; init; }

        public int SlotScroll { get; init; }
        public bool Slot1 { get; init; }
        public bool Slot2 { get; init; }
        public bool Slot3 { get; init; }

        public bool Pause { get; init; }
    }
}
