using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private AnimatedSprite _sprite;
        private RectangleF hitbox;
        private Vector2 _scale;

        private TextObject _counter;

        public Exit(Game1 game, GameManager gameManager, Room room, Point pos) : base(game, gameManager)
        {
            _room = room;
            hitbox = new RectangleF(pos.ToVector2() * Room.TILE_SIZE + room.Position, new Vector2(Room.TILE_SIZE));

            // LOAD SPRITE + SET CORRECT PIXEL SIZE + PLAY LOCK ANIM (depth 0.1f)

            SpriteFont font = game.Content.Load<SpriteFont>("Font/ExitCounterFont");
            _counter = new TextObject(font, "No text", hitbox.Center);
            _counter.Depth = 1;
        }

        public void StartAnim()
        {

        }

        public override void Update()
        {
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

            else game.BackgroundCanvas.DrawRectangle(hitbox, Color.Red, 4); // DEBUG
        }
    }
}
