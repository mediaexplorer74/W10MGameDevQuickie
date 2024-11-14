// Decompiled with JetBrains decompiler
// Type: GameEngine.Camera2D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameEngine
{
  public class Camera2D
  {
    public bool cameraChanged = true;
    public bool isMovingUsingScreenAxis;
    private Vector2 position;
    private float rotation;
    private float zoom;
    private Matrix _localTransform;
    public float ZoomOutClip = 0.2f;
    public float ZoomInClip = 5f;

    public Matrix LocalTransform
    {
      get
      {
        if (this.cameraChanged)
        {
          this._localTransform = Matrix.Identity * Matrix.CreateTranslation(-this.position.X, -this.position.Y, 0.0f) * Matrix.CreateScale(this.zoom, this.zoom, 1f) * Matrix.CreateRotationZ(this.rotation) * Matrix.CreateTranslation(new Vector3((float) (Engine.gameWidth / 2), (float) (Engine.gameHeight / 2), 0.0f));
          this.cameraChanged = false;
        }
        return this._localTransform;
      }
    }

    public Vector2 ScreenToWorld(Vector2 point)
    {
      return Vector2.Transform(point, Matrix.Invert(this.LocalTransform));
    }

    public Vector2 Position
    {
      get => this.position;
      set
      {
        if (!(this.position != value))
          return;
        this.cameraChanged = true;
        this.position = value;
      }
    }

    public float Rotation
    {
      get => this.rotation;
      set
      {
        if ((double) this.rotation == (double) value)
          return;
        this.cameraChanged = true;
        this.rotation = value;
      }
    }

    public float Zoom
    {
      get => this.zoom;
      set
      {
        if ((double) this.zoom == (double) value)
          return;
        this.cameraChanged = true;
        this.zoom = value;
      }
    }

    public Camera2D()
    {
      this.zoom = 1f;
      this.rotation = 0.0f;
      this.position = Vector2.Zero;
    }

    public void ResetChanged() => this.cameraChanged = false;

    public void MoveRight(ref float distance)
    {
      if ((double) distance == 0.0)
        return;
      this.cameraChanged = true;
      if (this.isMovingUsingScreenAxis)
      {
        this.position.X += (float) Math.Cos(-(double) this.rotation) * distance;
        this.position.Y += (float) Math.Sin(-(double) this.rotation) * distance;
      }
      else
        this.position.X += distance;
    }

    public void MoveLeft(ref float distance)
    {
      if ((double) distance == 0.0)
        return;
      this.cameraChanged = true;
      if (this.isMovingUsingScreenAxis)
      {
        this.position.X -= (float) Math.Cos(-(double) this.rotation) * distance;
        this.position.Y -= (float) Math.Sin(-(double) this.rotation) * distance;
      }
      else
        this.position.X -= distance;
    }

    public void MoveUp(ref float distance)
    {
      if ((double) distance == 0.0)
        return;
      this.cameraChanged = true;
      if (this.isMovingUsingScreenAxis)
      {
        this.position.X -= (float) Math.Sin((double) this.rotation) * distance;
        this.position.Y -= (float) Math.Cos((double) this.rotation) * distance;
      }
      else
        this.position.Y -= distance;
    }

    public void MoveDown(ref float distance)
    {
      if ((double) distance == 0.0)
        return;
      this.cameraChanged = true;
      if (this.isMovingUsingScreenAxis)
      {
        this.position.X += (float) Math.Sin((double) this.rotation) * distance;
        this.position.Y += (float) Math.Cos((double) this.rotation) * distance;
      }
      else
        this.position.Y += distance;
    }
  }
}
