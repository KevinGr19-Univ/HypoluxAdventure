using HypoluxAdventure.Core;
using HypoluxAdventure.Models;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
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
        private const bool DAMP = true;
        public CameraManager(Game1 game, GameManager gameManager) : base(game, gameManager) { }

        public override void Update()
        {
            Vector2 halfCameraDim = Application.ScreenDimensions * 0.5f / game.Camera.Zoom;

            RectangleF roomRect = gameManager.RoomManager.CurrentRoom.Rectangle;
            Vector2 targetPos = Vector2.Clamp(gameManager.Player.Position, roomRect.TopLeft + halfCameraDim, roomRect.BottomRight - halfCameraDim);

            game.Camera.Position = DAMP ? MathUtils.Damp(game.Camera.Position, targetPos, 0.6f, Time.RealDeltaTime) : targetPos;
        }

        public override void Draw() { }
    }
}
