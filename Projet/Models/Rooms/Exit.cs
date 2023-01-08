using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Rooms
{
    internal class Exit : GameObject
    {
        private Room _room;
        private bool _opened = false;
        private bool _trigger = false;

        private SpriteSheet _spriteSheet;
        private AnimatedSprite _sprite;
        private RectangleF hitbox;
        private Vector2 _scale;
        private Vector2 _position;
        private float _rotation;

        private TextObject _counter;

        public Exit(Game1 game, GameManager gameManager, Room room, Point pos) : base(game, gameManager)
        {
            _room = room;
            hitbox = new RectangleF(pos.ToVector2() * Room.TILE_SIZE + room.Position, new Vector2(Room.TILE_SIZE));

            // LOAD SPRITE + SET CORRECT PIXEL SIZE + PLAY LOCK ANIM (depth 0.1f)


            SpriteFont font = game.Content.Load<SpriteFont>("Font/ExitCounterFont");
            _counter = new TextObject(font, "No text", hitbox.Center);
            _counter.Depth = 1;

            _position = pos.ToVector2() * Room.TILE_SIZE + new Vector2(16,16) + room.Position;
            _scale = new Vector2(2,2);
            _rotation = 0;
            _spriteSheet = game.Content.Load<SpriteSheet>("img/holeAnimation.sf", new JsonContentLoader());
            _sprite = new AnimatedSprite(_spriteSheet);
            _sprite.Play("closed");
        }

        public void StartAnim()
        {
            _sprite.Play("opening");

        }

        public override void Update()
        {
            _sprite.Update(Time.DeltaTime);
            // UPDATE SPRITE
            if (!_opened)
            {
                int aliveMonsters = _room.GetAliveMonsters().Count();
                

                if (aliveMonsters == 0)
                {
                    _opened = true;
                    StartAnim();

                }
                else _counter.Text = $"{aliveMonsters} / {_room.MonsterCount}";
            }
            else if(!_trigger)
            {
                if (gameManager.Player.Hitbox.Intersects(hitbox))
                {
                    _trigger = true;
                    gameManager.StartNextFloorTransition();
                }
            }
        }

        public override void Draw()
        {
            //_sprite.Draw(game.Canvas, _position, 0, _scale);
            if (!_opened) _counter.Draw(game.Canvas);

            else _sprite.Draw(game.BackgroundCanvas, _position, _rotation, _scale); // DEBUG game.BackgroundCanvas.DrawRectangle(hitbox,Color.Red);
        }
    }
}
