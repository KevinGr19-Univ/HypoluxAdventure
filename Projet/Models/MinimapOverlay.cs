using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.UI;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models
{
    internal class MinimapOverlay : GameObject
    {
        private const float WIDTH = 150;
        public const float ROOM_RECT_WIDTH = 130;
        public const float ROOM_TILE_SCALE = 0.8f;

        private const float INVERSE_MAP_WIDTH = 1f / RoomManager.MAP_ROOM_WIDTH;
        private const float TILE_RATIO = ROOM_RECT_WIDTH * INVERSE_MAP_WIDTH;

        private static readonly Color BG_COLOR = new Color(Color.Black, 0.5f);
        private static readonly Color ROOM_COLOR = Color.White;
        private static readonly Color CURRENT_ROOM_COLOR = Color.Red;
        private static readonly Color EXIT_ROOM_COLOR = Color.Green;

        private RectangleF _background;
        private Vector2 _roomRectTopLeft;

        private TextObject _floorCounter;

        private List<Point> _visitedRooms = new List<Point>();

        public MinimapOverlay(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            _background = new RectangleF(25, 25, WIDTH, WIDTH);
            _roomRectTopLeft = _background.TopLeft + new Vector2(WIDTH - ROOM_RECT_WIDTH) * 0.5f;

            SpriteFont font = game.Content.Load<SpriteFont>("Font/FloorTextFont");
            _floorCounter = new TextObject(font, "No text", _background.Center + new Vector2(0, WIDTH * 0.5f + 20));
        }

        public override void Update() { }

        public override void Draw()
        {
            game.UICanvas.FillRectangle(_background, BG_COLOR, 0.4f);

            foreach(Point roomPos in _visitedRooms)
            {
                Vector2 topLeft = _roomRectTopLeft + roomPos.ToVector2() * TILE_RATIO;
                RectangleF rect = new RectangleF(topLeft, new Vector2(TILE_RATIO));
                rect = rect.Scale(new Vector2(ROOM_TILE_SCALE));

                Color color;
                if (gameManager.RoomManager.GetRoom(roomPos).Exit != null) color = EXIT_ROOM_COLOR;
                else color = gameManager.RoomManager.CurrentRoom.PointPos == roomPos ? CURRENT_ROOM_COLOR : ROOM_COLOR;

                game.UICanvas.FillRectangle(rect, color, 0.5f);
            }

            _floorCounter.Draw(game.UICanvas);
        }

        public void Visit(Point point)
        {
            if (!_visitedRooms.Contains(point)) _visitedRooms.Add(point);
        }

        public void Clear()
        {
            _visitedRooms.Clear();
            _floorCounter.Text = $"Étage {gameManager.Floor}";
        }
    }
}
