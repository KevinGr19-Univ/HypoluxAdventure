using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Core;

namespace HypoluxAdventure.Models
{
    public class Camera
    {
        public Vector2 Position = Vector2.Zero;
        public float Rotation;

        private float _zoom = 1;
        public float Zoom { get => _zoom; set { _zoom = value < 0.05f ? 0.05f : value; }}

        public Matrix ViewMatrix { get; private set; }
        private Matrix _inverseMatrix;

        public void CalculateMatrix()
        {
            ViewMatrix = Matrix.CreateTranslation(-Position.X, -Position.Y, 0)
                * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation))
                * Matrix.CreateScale(Zoom)
                * Matrix.CreateTranslation(new Vector3(Application.ScreenDimensions, 0) * 0.5f);

            _inverseMatrix = Matrix.Invert(ViewMatrix);
        }

        public Vector2 WorldToCamera(Vector2 pos) => Vector2.Transform(pos, ViewMatrix);
        public Vector2 CameraToWorld(Vector2 pos) => Vector2.Transform(pos, _inverseMatrix);

    }
}
