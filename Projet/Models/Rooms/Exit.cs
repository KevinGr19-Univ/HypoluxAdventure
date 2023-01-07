using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Rooms
{
    internal class Exit : GameObject
    {
        private Room _room;

        private AnimatedSprite _sprite;
        private Vector2 _position;
        private Vector2 _scale;

        private TextObject _counter;

        public Exit(Game1 game, GameManager gameManager, Room room, Point pos) : base(game, gameManager)
        {
            _room = room;
            _position = pos.ToVector2() * Room.TILE_SIZE + room.Position;

            // LOAD SPRITE + SET CORRECT PIXEL SIZE + PLAY LOCK ANIM

            SpriteFont font = game.Content.Load<SpriteFont>("Font/ExitCounterFont");
            _counter = new TextObject(font, "", _position);
        }

        public override void Update()
        {
            
        }

        public override void Draw()
        {
            //_sprite.Draw(game.BackgroundCanvas, _position, 0, _scale);
            _counter.Draw(game.BackgroundCanvas);
        }
    }
}
