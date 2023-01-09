using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.Items;
using HypoluxAdventure.Models.Monsters;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Projectiles
{
    internal class Knife : Projectile
    {
        public const float SPEED = 750f;

        private Vector2 _previousPos;
        private List<Entity> _hitEntities = new List<Entity>();

        public Knife(Game1 game, GameManager gameManager, bool isPlayerProj, Vector2 pos) : base(game, gameManager, isPlayerProj, pos)
        {
            Sprite = new Sprite(game.Content.Load<Texture2D>("img/knife"));
            Sprite.Depth = 0.8f;
            GraphicsUtils.SetPixelSize(Sprite, 35, 35, ref scale);
        }

        public override Vector2 HitboxSize => new Vector2(24, 24);

        public override void Update()
        {
            _previousPos = position;
            base.Update();

            rotation = Velocity != Vector2.Zero ? -MathF.Atan2(Velocity.X, Velocity.Y) + MathHelper.PiOver2 : 0;
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override bool OnEntityCollision(Entity entity)
        {
            if (!_hitEntities.Contains(entity))
            {
                _hitEntities.Add(entity);
                entity.Damage(6);
            }
            return true;
        }

        public override void OnRoomCollision()
        {
            Despawn();
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            SoundPlayer.PlaySound("sound/knifeSound", pitch: -0.3f);

            DropItem item = new KnifeItem(game, gameManager).ToDropItem(false, _previousPos);
            gameManager.ItemManager.Summon(item);
        }
    }
}
