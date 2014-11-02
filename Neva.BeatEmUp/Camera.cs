using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neva.BeatEmUp.GameObjects.Components;

namespace Neva.BeatEmUp
{
    public sealed class Camera 
    {
        #region Properties
        public Vector2 Position
        {
            get;
            set;
        }
        public Viewport Viewport
        {
            get;
            private set;
        }
        public Matrix Transformation
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                                           Matrix.CreateRotationZ(0.0f) *
                                           Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                           Matrix.CreateTranslation(new Vector3(0, 0, 0));
            }
        }
        public float Zoom
        {
            get;
            set;
        }
        #endregion

        public Camera(Vector2 position, Viewport viewport)
        {
            Position = position;
            Viewport = viewport;
            Zoom = 1.0f;
        }
    }
}