using System;
using Microsoft.Xna.Framework;

namespace Slicer.Core
{
    public class Camera
    {
        public Camera(int viewportWidth, int viewportHeight, Vector2 cameraPosition)
        {
            ViewportWidth = viewportWidth;
            ViewportHeight = viewportHeight;
            Position = cameraPosition;
        
            Zoom = 1.0f;
        }

        // Centered Position of the Camera.
        public Vector2 Position { get; set; }
        public float Zoom { get; private set; }
        public float Rotation { get; private set; }

        // height and width of the viewport window which should adjust when the player resizes the game window.
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        // Center of the Viewport does not account for scale
        public Vector2 ViewportCenter
        {
            get
            {
                return new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f);
            }
        }

        // create a matrix for the camera to offset everything we draw, the map and our objects. since the
        // camera coordinates are where the camera is, we offset everything by the negative of that to simulate
        // a camera moving. we also cast to integers to avoid filtering artifacts
        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)Position.X, -(int)Position.Y, 0) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                   Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        public void AdjustZoom(float amount)
        {
            Zoom += amount;
            if (Zoom < 0.1f)
            {
                Zoom = 0.1f;
            }
        }

        public void MoveCamera(Vector2 cameraMovement, bool clampToMap = false)
        {
            Position += cameraMovement;

            if (clampToMap)
            {
                // clamp the camera so it never leaves the visible area of the map.
                var cameraMax = new Vector2(Global.Room.Size.X * 16 - ViewportWidth,
                                             Global.Room.Size.X * 16 - ViewportHeight);

                Position = Vector2.Clamp(Position, Vector2.Zero, cameraMax);
            }
        }

        public Rectangle ViewportWorldBoundry()
        {
            Vector2 viewPortCorner = ScreenToWorld(new Vector2(0, 0));
            Vector2 viewPortBottomCorner = ScreenToWorld(new Vector2(ViewportWidth, ViewportHeight));

            return new Rectangle((int)viewPortCorner.X, (int)viewPortCorner.Y, (int)(viewPortBottomCorner.X - viewPortCorner.X),
                                                   (int)(viewPortBottomCorner.Y - viewPortCorner.Y));
        }

        public void CenterOn(Vector2 position)
        {
            Position = position;
        }

        public void CenterOn(Point location)
        {
            Position = CenteredPosition(location);
        }

        private Vector2 CenteredPosition(Point location, bool clampToMap = false)
        {
            var cameraPosition = new Vector2(location.X * 16, location.Y * 16);
            var cameraCenteredOnTilePosition = new Vector2(cameraPosition.X + 8,
                                                            cameraPosition.Y + 8);
            if (clampToMap)
            {
                // clamp the camera so it never leaves the visible area of the map.
                var cameraMax = new Vector2(Global.Room.Size.X * 16 - ViewportWidth,
                                             Global.Room.Size.Y * 16 - ViewportHeight);

                return Vector2.Clamp(cameraCenteredOnTilePosition, Vector2.Zero, cameraMax);
            }

            return cameraCenteredOnTilePosition;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(TranslationMatrix));
        }

        public void HandleInput()
        {
            // Move the camera's position based on WASD or Right thumbstick input
            Vector2 cameraMovement = Vector2.Zero;
            
            if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.A, false))
            {
                cameraMovement.X = -1;
            }
            else if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.D, false))
            {
                cameraMovement.X = 1;
            }
            if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.W, false))
            {
                cameraMovement.Y = -1;
            }
            else if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.S, false))
            {
                cameraMovement.Y = 1;
            }
            
            // to match the thumbstick behavior, we need to normalize non-zero vectors in case the user
            // is pressing a diagonal direction.
            if (cameraMovement != Vector2.Zero)
            {
                cameraMovement.Normalize();
            }

            // scale our movement to move 25 pixels per second
            cameraMovement *= 25f;

            // move the camera
            MoveCamera(cameraMovement);
        }
    }
}

