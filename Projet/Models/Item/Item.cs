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
        public string Label = "No name item";

        protected float _currentCooldown { get; private set; }
        public float CooldownProgress => _currentCooldown / Cooldown;              

        public float AdditionalRotation;
        private float _rotation;
        public Vector2 Scale = Vector2.One;

        public virtual float Cooldown => 0;
        abstract public Vector2 DefaultDirection { get; }
        abstract public float SlotScale { get; }
        abstract public float DistFromPlayer { get; }

        public bool IsUsed = false; // If IsUsed -> ClearSlot(slot, false)

        public Item(Game1 game, GameManager gameManager) : base(game, gameManager) { }

        protected void TriggerCooldown()
        {
            _currentCooldown = Cooldown;
        }

        public override void Update()
        {
            if (_currentCooldown > 0)
            {
                _currentCooldown -= Time.DeltaTime;
                if(_currentCooldown <= 0)
                {
                    _currentCooldown = 0;
                    OnCooldownRefresh();
                }
            }
        }

        public virtual void SelectedUpdate()
        {
            if(_currentCooldown <= 0)
            {
                if (gameManager.FrameInputs.Shoot) OnShoot();
                else if (gameManager.FrameInputs.Use) OnUse();
            }
        }

        public override void Draw()
        {

        }

        public void DrawSlot(Vector2 slotPos)
        {
            Vector2 origin = new Vector2(_texture.Width, _texture.Height) * 0.5f;
            game.UICanvas.Draw(_texture, slotPos, null, Color.White, 0, origin, SlotScale, SpriteEffects.None, ItemSlot.DEPTH + 0.001f);
        }

        abstract public void OnShoot();
        abstract public void OnUse();
        public virtual void OnCooldownRefresh() { }

        abstract public DropItem ToDropItem(); 
    }
}
