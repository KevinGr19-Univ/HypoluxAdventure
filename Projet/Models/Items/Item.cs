using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Items
{
    internal abstract class Item : GameObject
    {
        public Texture2D Texture { get; protected set; }
        protected Sprite sprite;

        public string Label = "No name item";

        public virtual float Cooldown => 0;
        protected float currentCooldown { get; private set; }

        abstract public float SlotScale { get; }
        public float CooldownProgress => currentCooldown / Cooldown;
        public bool IsOnCooldown => currentCooldown > 0;

        abstract protected float distFromPlayer { get; }
        abstract protected float defaultOrientation { get; }
        abstract protected int pixelSize { get; }

        protected Vector2 position { get; private set; }
        protected float localRotation;
        protected float rotation { get; private set; }
        private readonly float _defaultAngle;

        public bool UpdateRotation = true;
        public bool IsLocked = false;

        private Vector2 _scale;

        public bool IsUsed = false; // If IsUsed -> ClearSlot(slot, false)

        public Item(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            LoadSprite();
            GraphicsUtils.SetPixelSize(sprite, pixelSize, pixelSize, ref _scale);

            _defaultAngle = MathHelper.ToRadians(defaultOrientation + 90);
        }

        protected void TriggerCooldown()
        {
            currentCooldown = Cooldown;
        }

        public override void Update()
        {
            if (currentCooldown > 0)
            {
                currentCooldown -= Time.DeltaTime;
                if(currentCooldown <= 0)
                {
                    currentCooldown = 0;
                    OnCooldownRefresh();
                }
            }

            RotateAround();
        }

        public virtual void SelectedUpdate()
        {
            if(currentCooldown <= 0)
            {
                if (gameManager.FrameInputs.Shoot) OnShoot();
                else if (gameManager.FrameInputs.Use) OnUse();
            }
        }

        private float _tempRot;

        private void RotateAround()
        {
            if(UpdateRotation) _tempRot = MathF.Atan2(gameManager.Player.ShootDirection.X, gameManager.Player.ShootDirection.Y);
            float angle = _tempRot + MathHelper.ToRadians(localRotation);

            position = gameManager.Player.Position + new Vector2(MathF.Sin(angle), MathF.Cos(angle)) * distFromPlayer;
            rotation = -angle + _defaultAngle;
        }

        public override void Draw()
        {
            sprite.Depth = GameManager.GetYDepth(position.Y);
            sprite.Draw(game.Canvas, position, rotation, _scale);
        }

        public void DrawSlot(Vector2 slotPos)
        {
            Vector2 origin = new Vector2(Texture.Width, Texture.Height) * 0.5f;
            game.UICanvas.Draw(Texture, slotPos, null, Color.White, 0, origin, SlotScale, SpriteEffects.None, ItemSlot.DEPTH + 0.001f);
        }

        abstract protected void LoadSprite();
        abstract public DropItem ToDropItem(bool startHover, Vector2 pos); 

        abstract public void OnShoot();
        abstract public void OnUse();
        public virtual void OnCooldownRefresh() { }
        public virtual void OnDrop() { }
    }
}
