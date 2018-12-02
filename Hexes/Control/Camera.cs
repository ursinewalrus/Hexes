using Hexes.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexes
{
    //https://gamedev.stackexchange.com/questions/59301/xna-2d-camera-scrolling-why-use-matrix-transform
    public class Camera
    {
        public float Zoom { get; set; }
        public FloatPoint Center { get; set; }
        public Matrix Transform { get; protected set; }
        public Rectangle Bounds { get; protected set; }
        public Vector2 Position { get; protected set; }

        public Camera(Viewport viewport)
        {
            Bounds = viewport.Bounds;
            Zoom = .5f;
        }

        public void UpdateZoom(float zoomAdjust)
        {
            Zoom += zoomAdjust;
        }

        private void MoveCamera(Vector2 moveVector)
        {
            Position += moveVector;
        }

        private void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(
                                                new Vector3(-Position.X, -Position.Y, 0))
                                                * Matrix.CreateScale(Zoom)
                                                * Matrix.CreateTranslation(new Vector3(Bounds.Width * .25f, Bounds.Height * .25f, 0)
                                            ); 
        }

        private Rectangle UpdateVisibleMatrixArea()
        {
            var inverseMatrix = Matrix.Invert(Transform);
            var topLeft = Vector2.Transform(Vector2.Zero, inverseMatrix);
            var topRight = Vector2.Transform(new Vector2(Bounds.X, 0), inverseMatrix);
            var bottomLeft = Vector2.Transform(new Vector2(0, Bounds.Y), inverseMatrix);
            var bottomRight = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseMatrix);

            var min = new Vector2(
            MathHelper.Min(topLeft.X, MathHelper.Min(topRight.X, MathHelper.Min(bottomLeft.X, bottomRight.X))),
            MathHelper.Min(topLeft.Y, MathHelper.Min(topRight.Y, MathHelper.Min(bottomLeft.Y, bottomRight.Y))));

            var max = new Vector2(
                MathHelper.Max(topLeft.X, MathHelper.Max(topRight.X, MathHelper.Max(bottomLeft.X, bottomRight.X))),
                MathHelper.Max(topLeft.Y, MathHelper.Max(topRight.Y, MathHelper.Max(bottomLeft.Y, bottomRight.Y))));

            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));

        }

        public void UpdateCamera(Viewport viewport,CardinalDirections.Direction scrollDirection)
        {
            Bounds = viewport.Bounds;
            UpdateMatrix();
            Vector2 cameraMovement = Vector2.Zero;
            float scrollSpeed = Math.Max(Zoom * 10, 5);

            switch(scrollDirection)
            {
                case CardinalDirections.Direction.North:
                    cameraMovement.Y -= scrollSpeed;
                    break;

                case CardinalDirections.Direction.South:
                    cameraMovement.Y += scrollSpeed;
                    break;

                case CardinalDirections.Direction.East:
                    cameraMovement.X += scrollSpeed;
                    break;

                case CardinalDirections.Direction.West:
                    cameraMovement.X -= scrollSpeed;
                    break;

                case CardinalDirections.Direction.NorthWest:
                    cameraMovement.Y -= scrollSpeed * .75f;
                    cameraMovement.X -= scrollSpeed * .75f;
                    break;

                case CardinalDirections.Direction.NorthEast:
                    cameraMovement.Y -= scrollSpeed * .75f;
                    cameraMovement.X += scrollSpeed * .75f;
                    break;

                case CardinalDirections.Direction.SouthWest:
                    cameraMovement.Y += scrollSpeed * .75f;
                    cameraMovement.X -= scrollSpeed * .75f;
                    break;

                case CardinalDirections.Direction.SouthEast:
                    cameraMovement.Y += scrollSpeed * .75f;
                    cameraMovement.X += scrollSpeed * .75f;
                    break;
            }
            MoveCamera(cameraMovement);
        }
    }
}
