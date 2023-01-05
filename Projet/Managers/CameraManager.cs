using HypoluxAdventure.Core;
using HypoluxAdventure.Models;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Managers
{
    internal class CameraManager : GameObject
    {
        private const float BORDER_BUFFER = 10;

        public CameraManager(Game1 game, GameManager gameManager) : base(game, gameManager) { }

        public override void Update()
        {
            Vector2 halfCameraDim = Application.ScreenDimensions * 0.5f / game.Camera.Zoom;
            halfCameraDim += new Vector2(BORDER_BUFFER, BORDER_BUFFER);

            RectangleF roomRect = gameManager.RoomManager.CurrentRoom.Rectangle;
            Vector2 targetPos = Vector2.Clamp(gameManager.Player.Position, roomRect.TopLeft + halfCameraDim, roomRect.BottomRight - halfCameraDim);

            game.Camera.Position = MathUtils.Damp(game.Camera.Position, targetPos, 0.7f, Time.RealDeltaTime);
        }

        public override void Draw() { }
    }
}
