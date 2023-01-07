using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models
{
    internal struct InputLayout
    {
        public string Name { get; init; }

        public Keys NegX { get; init; }
        public Keys NegY { get; init; }
        public Keys PosX { get; init; }
        public Keys PosY { get; init; }
    }
}
