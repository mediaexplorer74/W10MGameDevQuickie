using FbonizziMonoGame.Drawing.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FbonizziMonoGame.Drawing
{
    /// <summary>
    /// A simple camera to show a portion of a world that is larger than the screen
    /// </summary>
    public class Camera2D
    {
        private readonly IScreenTransformationMatrixProvider _scalingMatrixProvider;

        private float _maximumZoom = float.MaxValue;
        private float _minimumZoom;
        private float _zoom;

        /// <summary>
        /// Camera constructor with scale matrix provider
        /// </summary>
        /// <param name="screenTransformationMatrixProvider"></param>
        public Camera2D(IScreenTransformationMatrixProvider screenTransformationMatrixProvider)
        {
            Rotation = 0;
            Zoom = 1;
            Position = Vector2.Zero;
            _scalingMatrixProvider = screenTransformationMatrixProvider;
            Origin = new Vector2(screenTransformationMatrixProvider.VirtualWidth / 2f, screenTransformationMatrixProvider.VirtualHeight / 2f);
        }

        /// <summary>
        /// Camera constructor without scale matrix provider
        /// </summary>
        public Camera2D(int width, int height)
            : this(new IdentityMatrixProvider(width, height))
        {

        }

        /// <summary>
        /// Camera origin
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Zoom of the camera
        /// </summary>
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                if (value < MinimumZoom || value > MaximumZoom)
                    throw new ArgumentException($"Zoom must be between {MinimumZoom} and {MaximumZoom}");

                _zoom = value;
            }
        }

        /// <summary>
        /// Sets the minimum camera zoom
        /// </summary>
        public float MinimumZoom
        {
            get { return _minimumZoom; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("MinimumZoom must be greater than zero");

                if (Zoom < value)
                    Zoom = MinimumZoom;

                _minimumZoom = value;
            }
        }

        /// <summary>
        /// Sets the maximum camera zoom
        /// </summary>
        public float MaximumZoom
        {
            get { return _maximumZoom; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("MaximumZoom must be greater than zero");

                if (Zoom > value)
                    Zoom = value;

                _maximumZoom = value;
            }
        }

        /// <summary>
        /// The camera rectangle representation
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                var frustum = GetBoundingFrustum();
                var corners = frustum.GetCorners();
                var topLeft = corners[0];
                var bottomRight = corners[2];
                var width = bottomRight.X - topLeft.X;
                var height = bottomRight.Y - topLeft.Y;
                return new Rectangle(
                    (int)topLeft.X, (int)topLeft.Y,
                    (int)width, (int)height);
            }
        }

        /// <summary>
        /// The camera position
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The camera rotation
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Moves the camera position
        /// </summary>
        /// <param name="direction"></param>
        public void Move(Vector2 direction)
        {
            Position += Vector2.Transform(direction, Matrix.CreateRotationZ(-Rotation));
        }

        /// <summary>
        /// Rotates the camera by an amount in radians degrees
        /// </summary>
        /// <param name="deltaRadians"></param>
        public void Rotate(float deltaRadians)
        {
            Rotation += deltaRadians;
        }

        /// <summary>
        /// Zooms in the camera safely
        /// </summary>
        /// <param name="deltaZoom"></param>
        public void ZoomIn(float deltaZoom)
        {
            ClampZoom(Zoom + deltaZoom);
        }

        /// <summary>
        /// Zooms out the camera safely
        /// </summary>
        /// <param name="deltaZoom"></param>
        public void ZoomOut(float deltaZoom)
        {
            ClampZoom(Zoom - deltaZoom);
        }

        private void ClampZoom(float value)
        {
            if (value < MinimumZoom)
                Zoom = MinimumZoom;
            else
                Zoom = value > MaximumZoom ? MaximumZoom : value;
        }

        /// <summary>
        /// Sets the camera position
        /// </summary>
        /// <param name="position"></param>
        public void LookAt(Vector2 position)
        {
            Position = position - new Vector2(_scalingMatrixProvider.VirtualWidth / 2f, _scalingMatrixProvider.VirtualHeight / 2f);
        }

        /// <summary>
        /// Transforms the given world coordinates into screen coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector2 WorldToScreen(float x, float y)
        {
            return WorldToScreen(new Vector2(x, y));
        }

        /// <summary>
        /// Transforms the given world coordinates into screen coordinates
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition + new Vector2(_scalingMatrixProvider.RealScreenWidth, _scalingMatrixProvider.RealScreenHeight), GetViewMatrix());
        }

        /// <summary>
        /// Transforms the given screen coordinates into world coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector2 ScreenToWorld(float x, float y)
        {
            return ScreenToWorld(new Vector2(x, y));
        }

        /// <summary>
        /// Transforms the given screen coordinates into world coordinates
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {

            return Vector2.Transform(screenPosition - new Vector2(_scalingMatrixProvider.RealScreenWidth, _scalingMatrixProvider.RealScreenHeight),
                Matrix.Invert(GetViewMatrix()));
        }

        /// <summary>
        /// Returns the current camera view matrix with the zoom factor and parallax factor
        /// </summary>
        /// <param name="parallaxFactor"></param>
        /// <returns></returns>
        public Matrix GetViewMatrix(Vector2 parallaxFactor)
        {
            return GetWorldMatrix(parallaxFactor) * _scalingMatrixProvider.ScaleMatrix;
        }

        /// <summary>
        /// Returns the world matrix
        /// </summary>
        /// <param name="parallaxFactor"></param>
        /// <returns></returns>
        public Matrix GetWorldMatrix(Vector2 parallaxFactor)
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position * parallaxFactor, 0.0f))
                * Matrix.CreateTranslation(new Vector3(-Origin, 0.0f))
                * Matrix.CreateRotationZ(Rotation)
                * Matrix.CreateScale(Zoom, Zoom, 1)
                * Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        /// <summary>
        /// Returns the world matrix
        /// </summary>
        /// <returns></returns>
        public Matrix GetVirtualViewMatrix()
        {
            return GetWorldMatrix(Vector2.One);
        }

        /// <summary>
        /// It returns the current ViewMatrix view matrix with the zoom factor
        /// </summary>
        /// <returns></returns>
        public Matrix GetViewMatrix()
        {
            return GetViewMatrix(Vector2.One);
        }

        /// <summary>
        /// It return the inverse of the view matrix
        /// </summary>
        /// <returns></returns>
        public Matrix GetInverseViewMatrix()
        {
            return Matrix.Invert(GetViewMatrix());
        }

        /// <summary>
        /// It returns the projection matrix
        /// </summary>
        /// <param name="viewMatrix"></param>
        /// <returns></returns>
        public Matrix GetProjectionMatrix(Matrix viewMatrix)
        {
            var projection = Matrix.CreateOrthographicOffCenter(0, _scalingMatrixProvider.VirtualWidth,
                _scalingMatrixProvider.VirtualHeight, 0, -1, 0);
            Matrix.Multiply(ref viewMatrix, ref projection, out projection);
            return projection;
        }

        /// <summary>
        /// It returns the current view bounding frustum
        /// </summary>
        /// <returns></returns>
        public BoundingFrustum GetBoundingFrustum()
        {
            var viewMatrix = GetVirtualViewMatrix();
            var projectionMatrix = GetProjectionMatrix(viewMatrix);
            return new BoundingFrustum(projectionMatrix);
        }

        /// <summary>
        /// Checks if the point is contained in the camera view
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public ContainmentType Contains(Point point)
        {
            return Contains(point.ToVector2());
        }

        /// <summary>
        /// Checks if the given coordinates are contained in the camera view
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public ContainmentType Contains(Vector2 vector2)
        {
            return GetBoundingFrustum().Contains(new Vector3(vector2.X, vector2.Y, 0));
        }

        /// <summary>
        /// Checks if the given coordinates is contained in the camera view
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public ContainmentType Contains(Rectangle rectangle)
        {
            var max = new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, 0.5f);
            var min = new Vector3(rectangle.X, rectangle.Y, 0.5f);
            var boundingBox = new BoundingBox(min, max);
            return GetBoundingFrustum().Contains(boundingBox);
        }
    }
}