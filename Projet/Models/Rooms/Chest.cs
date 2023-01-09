using HypoluxAdventure.Managers;
using HypoluxAdventure.Core;
using HypoluxAdventure.Models.Items;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Content;
using HypoluxAdventure.Utils;

namespace HypoluxAdventure.Models.Rooms
{
    internal class Chest : GameObject
    {
        private AnimatedSprite _sprite;
        private Vector2 _scale;

        public RectangleF Hitbox { get; private set; }

        private bool _opened = false;

        public Chest(Game1 game, GameManager gameManager, Room room, Point pos) : base(game, gameManager)
        {
            SpriteSheet spriteSheet = game.Content.Load<SpriteSheet>("img/chestAnimation.sf", new JsonContentLoader());
            _sprite = new AnimatedSprite(spriteSheet);
            GraphicsUtils.SetPixelSize(_sprite, Room.TILE_SIZE, Room.TILE_SIZE, ref _scale);

            Hitbox = new RectangleF(pos.ToVector2() * Room.TILE_SIZE + room.Position, new Vector2(Room.TILE_SIZE));
            _sprite.Play("closed");
        }

        public override void Draw()
        {
            float yDepth = GameManager.GetYDepth(Hitbox.Center.Y);

            _sprite.Depth = yDepth;
            _sprite.Draw(game.Canvas, Hitbox.Center, 0, _scale);
        }

        public override void Update()
        {
            _sprite.Update(Time.DeltaTime);
            if (Inputs.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.U)) gameManager.Player.Damage(3);
        }

        public void Open()
        {
            if (_opened) return;
            _opened = true;

            // PLAY ANIMATION
            _sprite.Play("opened");
            DropItem dropItem = LootItem().ToDropItem(false, gameManager.Player.Position);
            gameManager.ItemManager.Summon(dropItem);
        }

        private Item LootItem()
        {
            const float MIN_HEALTH_CHANCE = 0.2f;
            const float MAX_HEALTH_CHANCE = 1f;

            Random r = new Random();
            float healChance = MathUtils.Lerp(MAX_HEALTH_CHANCE, MIN_HEALTH_CHANCE, (float)gameManager.Player.Health / gameManager.Player.MaxHealth);
            float itemChance = r.NextSingle() * 100;

            if(r.NextSingle() < healChance)
            {
                if (itemChance < 30) return new SuperPotion(game, gameManager);
                else return new Potion(game, gameManager);
            }
            else
            {
                if (itemChance < 10) return new Sword(game, gameManager);
                else if (itemChance < 40) return new Bow(game, gameManager);
                else if (itemChance < 70) return new KnifeItem(game, gameManager);
                else return new Shotgun(game, gameManager);
            }
        }
    }
}
