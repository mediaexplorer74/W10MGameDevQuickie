// Decompiled with JetBrains decompiler
// Type: GameEngine.WaypointListFollower
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class WaypointListFollower : Queue<Vector2>
  {
    private const float AtDestinationLimit = 5f;
    public Vector2 Position;
    public float Rotation;
    public float moveSpeed = 100f;
    public float rotationInterpolationSpeed = 10f;
    private bool isActive;
    private Vector2 Direction;
    private float previousRotation;
    private float targetRotation;
    private float rotationInterpolation;
    private Vector2 lastDirection;
    private bool recomputeTargetRotation = true;

    public event WaypointListFollower.OnFinishedHandler OnFinished;

    public bool isNavigating => this.isActive;

    private float DistanceToDestination => Vector2.Distance(this.Position, this.Peek());

    private bool AtDestination => (double) this.DistanceToDestination < 5.0;

    public Vector2 this[int index]
    {
      get
      {
        Vector2 vector2_1 = Vector2.Zero;
        foreach (Vector2 vector2_2 in (Queue<Vector2>) this)
        {
          --index;
          if (index < 0)
          {
            vector2_1 = vector2_2;
            break;
          }
        }
        return vector2_1;
      }
    }

    public void Start()
    {
      this.isActive = true;
      this.Position = this[0];
    }

    public void Pause() => this.isActive = false;

    public void Update(GameTime gameTime)
    {
      if (!this.isActive || this.Count == 0)
        return;
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      Vector2 zero = Vector2.Zero;
      if (this.Count <= 0)
        return;
      if (this.AtDestination)
      {
        this.Dequeue();
        this.recomputeTargetRotation = true;
        if (this.Count != 0)
          return;
        this.isActive = false;
        if (this.OnFinished == null)
          return;
        this.OnFinished();
      }
      else
      {
        this.Direction = this.Peek() - this.Position;
        if (this.Direction != Vector2.Zero)
          this.Direction.Normalize();
        this.lastDirection = this.Direction;
        if (this.recomputeTargetRotation)
        {
          this.previousRotation = this.Rotation;
          this.targetRotation = (float) Math.Atan2(-(double) this.Direction.Y, (double) this.Direction.X);
          this.rotationInterpolation = 0.0f;
          if ((double) this.targetRotation - (double) this.previousRotation > 3.1415927410125732)
            this.targetRotation -= 6.28318548f;
          else if ((double) this.targetRotation - (double) this.previousRotation < -3.1415927410125732)
            this.targetRotation += 6.28318548f;
          this.recomputeTargetRotation = false;
        }
        this.rotationInterpolation = MathHelper.Clamp(this.rotationInterpolation + totalSeconds * this.rotationInterpolationSpeed, 0.0f, 1f);
        this.Rotation = this.previousRotation + (this.targetRotation - this.previousRotation) * this.rotationInterpolation;
        this.Position += this.Direction * this.moveSpeed * totalSeconds;
      }
    }

    public delegate void OnFinishedHandler();
  }
}
