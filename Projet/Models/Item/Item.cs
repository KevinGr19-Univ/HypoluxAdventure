using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Item
{
    internal class Item : GameObject
    {
        private Sprite _sprite;
        private Vector2 _startingPoint;
        private Vector2 _position;
        private float _scale;

        public virtual float Cooldown => 0;
        private float _currentCooldown;
        

        public Item(Game1 game, GameManager gameManager) : base(game, gameManager)
        {

        }

        public override void Draw()
        {
            
        }

        public override void Update()
        {
            if (_currentCooldown > 0)
            {
                _currentCooldown -= Core.Time.DeltaTime;
            }
            
        }

        public void SelectedUpdate()
        {

        }
    }
}
