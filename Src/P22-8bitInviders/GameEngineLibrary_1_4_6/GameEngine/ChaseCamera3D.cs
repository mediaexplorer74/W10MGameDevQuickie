// Decompiled with JetBrains decompiler
// Type: GameEngine.ChaseCamera3D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class ChaseCamera3D : ICamera
  {
    private Matrix viewMatrix;
    private Matrix projectionMatrix;
    private float FieldOfView;
    public float NearPlaneDistance = 1f;
    public float FarPlaneDistance = 10000f;
    private SceneNode Target;
    private BoundingFrustum cameraFrustrum;
    public Vector3 Position;
    public Vector3 DesiredPosition;
    public Vector3 DesiredPositionOffset;
    public Vector3 Velocity;
    private Vector3 ChasePosition;
    private Vector3 ChaseDirection;
    private Vector3 Up = Vector3.Up;
    public float Stiffness = 400f;
    public float Damping = 400f;
    public float Mass = 50f;

    public Matrix ViewMatrix => this.viewMatrix;

    public Matrix ProjectionMatrix => this.projectionMatrix;

    public Vector3 camPosition => this.Position;

    public Vector3 camUp => this.Up;

    public ChaseCamera3D(Vector3 position, SceneNode target, float fov)
    {
      this.DesiredPosition = position;
      this.DesiredPositionOffset = position;
      this.Position = position;
      this.Target = target;
      this.FieldOfView = fov;
      if (target != null)
      {
        this.ChasePosition = this.Target.Position;
        this.ChaseDirection = this.Target.Direction;
        this.Up = this.Target.Up;
      }
      this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(this.FieldOfView), (float) Engine.gameWidth / (float) Engine.gameHeight, this.NearPlaneDistance, this.FarPlaneDistance);
      this.cameraFrustrum = new BoundingFrustum(this.viewMatrix * this.projectionMatrix);
    }

    public void SetTarget(SceneNode target)
    {
      this.Target = target;
      this.ChasePosition = this.Target.Position;
      this.ChaseDirection = this.Target.Direction;
      this.Up = this.Target.Up;
      this.Reset();
    }

    private void UpdateWorldPositions()
    {
      this.DesiredPosition = this.ChasePosition + Vector3.TransformNormal(this.DesiredPositionOffset, Matrix.Identity with
      {
        Forward = this.ChaseDirection,
        Up = this.Up,
        Right = Vector3.Cross(this.Up, this.ChaseDirection)
      });
    }

    public void Reset()
    {
      this.UpdateWorldPositions();
      this.Velocity = Vector3.Zero;
      this.Position = this.DesiredPosition;
      this.UpdateMatrices();
    }

    private void UpdateMatrices()
    {
      this.viewMatrix = Matrix.CreateLookAt(this.Position, this.Target.Position, this.Up);
      this.cameraFrustrum.Matrix = this.viewMatrix * this.projectionMatrix;
    }

    public void Update(GameTime gameTime)
    {
      this.ChasePosition = this.Target.Position;
      this.ChaseDirection = this.Target.Direction;
      this.Up = this.Target.Up;
      this.UpdateWorldPositions();
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.Velocity += (this.Stiffness * (this.DesiredPosition - this.Position) - this.Damping * this.Velocity) / this.Mass * totalSeconds;
      this.Position += this.Velocity * totalSeconds;
      this.UpdateMatrices();
    }

    public Vector3? CalculateMouse3DPosition(Vector2 touchPosition)
    {
      return this.CalculateMouse3DPosition(touchPosition, new Plane(Vector3.Up, 0.0f));
    }

    public Vector3? CalculateMouse3DPosition(Vector2 touchPosition, Plane GroundPlane)
    {
      float x = touchPosition.X;
      float y = touchPosition.Y;
      Vector3 source1 = new Vector3(x, y, 0.0f);
      Vector3 source2 = new Vector3(x, y, 1f);
      Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
      Vector3 position = Engine.game.GraphicsDevice.Viewport.Unproject(source1, this.ProjectionMatrix, this.ViewMatrix, Matrix.Identity);
      Vector3 direction = Engine.game.GraphicsDevice.Viewport.Unproject(source2, this.ProjectionMatrix, this.ViewMatrix, Matrix.Identity) - position;
      direction.Normalize();
      Ray ray = new Ray(position, direction);
      float? nullable = ray.Intersects(GroundPlane);
      return nullable.HasValue ? new Vector3?(ray.Position + ray.Direction * nullable.Value) : new Vector3?();
    }

    public bool IsInCameraFrustrum(BoundingSphere bsphere)
    {
      return this.cameraFrustrum.Contains(bsphere) != ContainmentType.Disjoint;
    }

    public bool IsInCameraFrustrum(BoundingBox bbox)
    {
      return this.cameraFrustrum.Contains(bbox) != ContainmentType.Disjoint;
    }
  }
}
