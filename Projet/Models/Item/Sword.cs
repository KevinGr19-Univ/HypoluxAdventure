using HypoluxAdventure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Item
{
    internal class Sword : Item
    {
        public Sword(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _texture = game.Content.Load<Texture2D>("img/sword");
            Label = "ÉPÉE";
        }

        public override float SlotScale => 4;
        public override float DefaultAngle => 45;
        public override float Cooldown => 0.5f;
        public override float DistFromPlayer => 40;

        private float _timer = 3;

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
        }
    }
}
