using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
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
    internal class Item : GameObject
    {
        private string _label;
        private Vector2 _startingPoint;
        private Vector2 _position;

        public Vector2 Scale = Vector2.One;
        public virtual float SlotScale => 4;

        private Texture2D _texture;
        private Sprite _sprite;
        
        public virtual float Cooldown => 0;
        private float _currentCooldown;

        public Item(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _texture = game.Content.Load<Texture2D>("img/sword");
            _sprite = new Sprite(_texture);
        }

        public void DrawSlot(Vector2 slotPos)
        {
            Vector2 origin = new Vector2(_texture.Width, _texture.Height) * 0.5f;
            game.UICanvas.Draw(_texture, slotPos, null, Color.White, 0, origin, SlotScale, SpriteEffects.None, ItemSlot.DEPTH + 0.001f);
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
            else
            {
                _currentCooldown = Cooldown;
            }
            
        }

        public void SelectedUpdate()
        {
            
        }
    }
}
