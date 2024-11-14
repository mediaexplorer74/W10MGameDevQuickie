// Decompiled with JetBrains decompiler
// Type: GameEngine.Camera3D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameEngine
{
  public class Camera3D : ICamera
  {
    private Matrix projectionMatrix;
    private Matrix viewMatrix;
    private Matrix posrotMatrix;
    private Vector3 viewPosition;
    private Quaternion myRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1f);
    private float yaw;
    private float pitch;
    private float FOV;
    private Vector3 zoomvector;
    private float zoomamount;
    private float nearClip;
    private float farClip;
    private CameraType camtype;
    private GameTime gameTime;
    private BoundingFrustum cameraFrustrum;
    public SceneNode Target;
    public float maxRadius;
    public float minRadius;
    public float maxY;
    public float minY;
    public Vector3 clickpoint;
    public Vector3 thirdPersonReference;
    public float maxzoom;
    public float minzoom;
    public float maxPitch;
    public float minPitch;
    public Vector3 _upVector = Vector3.Up;

    public float zoomAmount => this.zoomamount;

    public Matrix ViewMatrix => this.viewMatrix;

    public Matrix ProjectionMatrix => this.projectionMatrix;

    public Vector3 camPosition => this.viewPosition;

    public Vector3 camUp => Vector3.Up;

    public Vector3 Position
    {
      get => this.viewPosition;
      set
      {
        this.viewPosition = value;
        if (this.camtype == CameraType.FirstPerson)
          this.FPCamUpdate();
        else if (this.camtype == CameraType.LookAt)
          this.LACamUpdate();
        else
          this.FollowUpdate();
      }
    }

    public Vector3 upVector
    {
      get => this._upVector;
      set
      {
        this._upVector = value;
        if (this.camtype == CameraType.LookAt)
          this.LACamUpdate();
        else
          this.FollowUpdate();
      }
    }

    public Vector3 ViewTarget
    {
      get => this.Target.Position;
      set
      {
        this.Target.Position = value;
        if (this.camtype == CameraType.FirstPerson)
          this.FPCamUpdate();
        else if (this.camtype == CameraType.LookAt)
          this.LACamUpdate();
        else
          this.FollowUpdate();
      }
    }

    public float Yaw => this.yaw;

    public Camera3D(CameraType cam, Vector3 position, float fov, SceneNode lookAtTarget)
    {
      this.camtype = cam;
      int gameWidth = Engine.gameWidth;
      int gameHeight = Engine.gameHeight;
      this.viewPosition = position;
      this.BuildCamera(fov, gameWidth, gameHeight, 0.1f, 10000f, lookAtTarget);
    }

    public Camera3D(
      CameraType cam,
      Vector3 position,
      float fov,
      float nearPlane,
      float farPlane,
      SceneNode lookAtTarget)
    {
      this.camtype = cam;
      int gameWidth = Engine.gameWidth;
      int gameHeight = Engine.gameHeight;
      this.BuildCamera(fov, gameWidth, gameHeight, nearPlane, farPlane, lookAtTarget);
    }

    public Camera3D(
      CameraType cam,
      Vector3 position,
      float fov,
      int viewXpos,
      int viewYpos,
      int viewWidth,
      int viewHeight,
      SceneNode lookAtTarget)
    {
      this.camtype = cam;
      this.BuildCamera(fov, viewWidth, viewHeight, 0.1f, 1000f, lookAtTarget);
    }

    public Camera3D(
      CameraType cam,
      float fov,
      int viewXpos,
      int viewYpos,
      int viewWidth,
      int viewHeight,
      float nearPlane,
      float farPlane,
      SceneNode lookAtTarget)
    {
      this.camtype = cam;
      this.BuildCamera(fov, viewWidth, viewHeight, nearPlane, farPlane, lookAtTarget);
    }

    ~Camera3D() => Tools.Trace("CAMERA KILLED");

    public void SetUpVector(float angle)
    {
      Vector2 vector = Tools.AngleToVector(MathHelper.ToRadians(90f + angle));
      this.upVector = new Vector3(vector.Y, vector.X, 0.0f);
    }

    public void Update(GameTime gametime)
    {
      this.gameTime = gametime;
      if (this.camtype == CameraType.FirstPerson)
        this.FPCamUpdate();
      else if (this.camtype == CameraType.LookAt)
        this.LACamUpdate();
      else
        this.FollowUpdate();
    }

    public void FollowUpdate()
    {
      this.zoomvector = Vector3.Normalize(this.Target.Position - this.viewPosition);
      Matrix rotationY = Matrix.CreateRotationY(this.yaw);
      this.viewPosition = Vector3.Transform(this.thirdPersonReference, Matrix.CreateRotationX(this.pitch) * rotationY) + this.Target.Position;
      this.viewPosition += this.zoomvector * this.zoomamount;
      this.viewMatrix = Matrix.CreateLookAt(this.viewPosition, this.Target.Position, this._upVector);
      this.cameraFrustrum.Matrix = this.viewMatrix * this.projectionMatrix;
    }

    private void FPCamUpdate()
    {
      this.viewMatrix = Matrix.CreateFromQuaternion(this.myRotation) * Matrix.CreateTranslation(this.viewPosition);
      this.cameraFrustrum.Matrix = this.viewMatrix * this.projectionMatrix;
    }

    private void LACamUpdate()
    {
      this.viewMatrix = Matrix.CreateLookAt(this.viewPosition, this.Target.Position, this._upVector);
      this.cameraFrustrum.Matrix = this.viewMatrix * this.projectionMatrix;
    }

    public void Rotate(Vector3 axis, float angle)
    {
      if (this.camtype == CameraType.FirstPerson)
      {
        this.yaw += angle * axis.Y;
        this.pitch += angle * axis.X;
        Matrix fromAxisAngle = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 1f, 0.0f), this.yaw);
        this.myRotation = Quaternion.Normalize(Quaternion.CreateFromRotationMatrix(Matrix.CreateFromAxisAngle(new Vector3(1f, 0.0f, 0.0f), this.pitch) * fromAxisAngle));
      }
      else if (this.camtype == CameraType.LookAt)
      {
        this.yaw += angle * axis.Y;
        this.pitch += angle * axis.X;
        Matrix rotationY = Matrix.CreateRotationY(angle * axis.Y);
        Matrix rotationX = Matrix.CreateRotationX(angle * axis.X);
        this.posrotMatrix = Matrix.CreateTranslation(this.viewPosition - this.Target.Position) * rotationY * rotationX;
        this.viewPosition = this.posrotMatrix.Translation;
      }
      else
      {
        this.yaw += angle * axis.Y;
        this.pitch += angle * axis.X;
        if ((double) this.pitch > (double) this.maxPitch)
        {
          this.pitch = this.maxPitch;
        }
        else
        {
          if ((double) this.pitch >= (double) this.minPitch)
            return;
          this.pitch = this.minPitch;
        }
      }
    }

    public void Translate(Vector3 speed)
    {
      speed *= (float) (this.gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
      if (this.camtype == CameraType.FirstPerson)
        this.viewPosition += Vector3.Transform(speed, Matrix.CreateFromQuaternion(this.myRotation));
      else if (this.camtype == CameraType.LookAt)
      {
        Vector3 vector1 = Vector3.Normalize(new Vector3(this.Target.Position.X - this.viewPosition.X, this.Target.Position.Y, this.Target.Position.Z - this.viewPosition.Z));
        Vector3 vector3 = Vector3.Cross(vector1, Vector3.Up);
        this.viewPosition.X += vector3.X * speed.X;
        this.viewPosition.Z += vector3.Z * speed.Z;
        this.Target.Position.X += vector3.X * speed.X;
        this.Target.Position.Z += vector3.Z * speed.Z;
        this.viewPosition.Y += speed.Y;
        this.Target.Position.Y += speed.Y;
        this.viewPosition.X += vector1.X * speed.X;
        this.viewPosition.Z += vector1.Z * speed.Z;
        this.Target.Position.X += vector1.X * speed.X;
        this.Target.Position.Z += vector1.Z * speed.Z;
      }
      else
      {
        this.thirdPersonReference.Y += speed.Y;
        double num = (double) Vector3.Distance(this.viewPosition, this.Target.Position);
        this.thirdPersonReference.Y = Math.Max(this.thirdPersonReference.Y, this.Target.Position.Y + 20f);
        this.thirdPersonReference.Y = Math.Min(this.thirdPersonReference.Y, this.maxY);
      }
    }

    public void Zoom(float amount)
    {
      if (this.camtype == CameraType.LookAt)
      {
        if ((double) amount == 0.0)
          return;
        this.viewPosition += Vector3.Normalize(this.Target.Position - this.viewPosition) * amount;
      }
      else
      {
        if (this.camtype != CameraType.ThirdPerson)
          return;
        this.zoomamount += amount;
        if ((double) this.zoomamount > (double) this.maxzoom)
          this.zoomamount = this.maxzoom;
        if ((double) this.zoomamount >= (double) this.minzoom)
          return;
        this.zoomamount = this.minzoom;
      }
    }

    public void SetZoom(float amount)
    {
      if (this.camtype == CameraType.LookAt)
      {
        if ((double) amount == 0.0)
          return;
        this.viewPosition = Vector3.Normalize(this.Target.Position - this.viewPosition) * amount;
      }
      else
      {
        if (this.camtype != CameraType.ThirdPerson)
          return;
        this.zoomamount = amount;
      }
    }

    public void ResetZoom() => this.zoomamount = 0.0f;

    public void ResetYawPitch()
    {
      this.yaw = 0.0f;
      this.pitch = 0.0f;
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

    public Ray CalculateCursorRay(Vector2 Position)
    {
      Vector3 source1 = new Vector3(Position, 0.0f);
      Vector3 source2 = new Vector3(Position, 1f);
      Vector3 position = Engine.gdm.GraphicsDevice.Viewport.Unproject(source1, this.projectionMatrix, this.viewMatrix, Matrix.Identity);
      Vector3 direction = Engine.gdm.GraphicsDevice.Viewport.Unproject(source2, this.projectionMatrix, this.viewMatrix, Matrix.Identity) - position;
      direction.Normalize();
      return new Ray(position, direction);
    }

    public void SetTarget(SceneNode target)
    {
      this.Target = target;
      if (this.camtype == CameraType.FirstPerson)
        this.FPCamUpdate();
      else if (this.camtype == CameraType.LookAt)
        this.LACamUpdate();
      else
        this.FollowUpdate();
    }

    public bool IsInCameraFrustrum(BoundingSphere bsphere)
    {
      return this.cameraFrustrum.Contains(bsphere) != ContainmentType.Disjoint;
    }

    public bool IsInCameraFrustrum(BoundingBox bbox)
    {
      return this.cameraFrustrum.Contains(bbox) != ContainmentType.Disjoint;
    }

    private void BuildCamera(
      float fov,
      int viewWidth,
      int viewHeight,
      float nearPlane,
      float farPlane,
      SceneNode avatar)
    {
      this.gameTime = new GameTime();
      this.Target = avatar;
      this.thirdPersonReference = this.viewPosition;
      this.maxRadius = 300f;
      this.minRadius = 25f;
      this.maxY = 200f;
      this.minY = 35f;
      this.FOV = fov;
      this.nearClip = nearPlane;
      this.farClip = farPlane;
      this.zoomamount = 0.0f;
      this.maxzoom = 70f;
      this.minzoom = -70f;
      this.minPitch = -0.2f;
      this.maxPitch = 0.6f;
      this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), (float) viewWidth / (float) viewHeight, 1f, farPlane);
      this.cameraFrustrum = new BoundingFrustum(this.viewMatrix * this.projectionMatrix);
    }
  }
}
