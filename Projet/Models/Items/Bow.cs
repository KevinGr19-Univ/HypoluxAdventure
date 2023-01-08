using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Items
{
    internal class Bow : Item
    {
        public Bow(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            Label = "ARC";
            AnimatedSprite.Play("idle");
        }

        public override float Cooldown => 1.25f;

        public override float SlotScale => 4;
        protected override float distFromPlayer => 50;
        protected override float defaultOrientation => 0;
        protected override int pixelSize => 40;

        private AnimatedSprite AnimatedSprite => (AnimatedSprite)sprite;

        private bool _startCharge = false;
        private bool _charged = false;

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
            AnimatedSprite.Update(Time.DeltaTime);

            if (_startCharge && !_charged && !Inputs.IsClickDown(Inputs.MouseButton.Left) && !Inputs.IsClickDown(Inputs.MouseButton.Right))
            {
                AnimatedSprite.Play("idle");
                ResetBow();
            }
        }

        public override void OnShoot()
        {
            TriggerBow();
        }

        public override void OnUse()
        {
            TriggerBow();
        }

        private void TriggerBow()
        {
            if (!_charged)
            {
                _startCharge = true;
                AnimatedSprite.Play("charging", () => { _charged = true; });
            }
            else
            {
                Arrow arrow = new Arrow(game, gameManager, true, position);
                arrow.Velocity = gameManager.Player.ShootDirection * Arrow.SPEED;
                arrow.Spawn();

                AnimatedSprite.Play("idle_empty");
                ResetBow();
                TriggerCooldown();
            }
        }

        private void ResetBow()
        {
            _startCharge = false;
            _charged = false;
        }

        public override void OnCooldownRefresh()
        {
            base.OnCooldownRefresh();
            AnimatedSprite.Play("idle");
        }

        public override void OnDrop()
        {
            base.OnDrop();

            AnimatedSprite.Play(IsOnCooldown ? "idle_empty" : "idle");
            ResetBow();
        }

        public override DropItem ToDropItem(bool startHover, Vector2 pos)
        {
            DropItem dropItem = new DropItem(game, gameManager, this, startHover);
            dropItem.CalculateHitbox(pos, new Vector2(24, 24));
            dropItem.SetTextureSize(24, 24);
            return dropItem;
        }

        protected override void LoadSprite()
        {
            SpriteSheet spriteSheet = game.Content.Load<SpriteSheet>("img/bowAnimation.sf", new JsonContentLoader());
            Texture = game.Content.Load<Texture2D>("img/bow_texture");
            sprite = new AnimatedSprite(spriteSheet);
        }
    }
}
