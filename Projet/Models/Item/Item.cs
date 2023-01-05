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
    internal abstract class Item : GameObject
    {
        protected Texture2D _texture;
        abstract public float SlotScale { get; }

        protected Sprite _sprite;
        protected Vector2 Position;
        public Vector2 Scale = Vector2.One;

        abstract public Vector2 StartingPoint { get; }
        
        public virtual float Cooldown => 0;
        protected float _currentCooldown { get; private set; }

        public Item(Game1 game, GameManager gameManager) : base(game, gameManager) { }

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
            if (_currentCooldown > 0) _currentCooldown -= Time.DeltaTime;
        }

        public void SelectedUpdate()
        {
            
        }
    }
}
