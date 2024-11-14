// Decompiled with JetBrains decompiler
// Type: GameEngine.Camera3DOrthographic
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class Camera3DOrthographic : ICamera
  {
    private Matrix projectionMatrix;
    private Matrix viewMatrix;
    private Vector3 viewPosition;
    private GameTime gameTime;
    private BoundingFrustum cameraFrustrum;

    public Matrix ProjectionMatrix => this.projectionMatrix;

    public Matrix ViewMatrix => this.viewMatrix;

    public Vector3 camPosition => this.viewPosition;

    public Vector3 camUp => Vector3.Up;

    public Camera3DOrthographic()
    {
      this.gameTime = new GameTime();
      this.projectionMatrix = Matrix.CreateOrthographicOffCenter(0.0f, (float) Engine.gameWidth, 0.0f, (float) Engine.gameHeight, 0.0f, 1000f);
      this.cameraFrustrum = new BoundingFrustum(this.viewMatrix * this.projectionMatrix);
    }

    ~Camera3DOrthographic() => Tools.Trace("CAMERAORTHO KILLED");

    public void Update(GameTime gametime)
    {
      this.gameTime = gametime;
      this.viewMatrix = Matrix.CreateLookAt(new Vector3(this.viewPosition.X, this.viewPosition.Y, this.viewPosition.Z + 50f), new Vector3(this.viewPosition.X, this.viewPosition.Y, this.viewPosition.Z - 50f), Vector3.Up);
      this.cameraFrustrum.Matrix = this.viewMatrix * this.projectionMatrix;
    }

    public void Translate(Vector2 speed)
    {
      speed *= (float) this.gameTime.ElapsedGameTime.TotalSeconds;
      this.viewPosition.X += speed.X;
      this.viewPosition.Y += speed.Y;
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
