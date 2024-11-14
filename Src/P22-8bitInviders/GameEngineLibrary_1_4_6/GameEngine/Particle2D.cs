﻿// Decompiled with JetBrains decompiler
// Type: GameEngine.Particle2D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameEngine
{
  public class Particle2D
  {
    private float Decay;
    public Color color;
    public Vector2 Origin;
    public Vector2 Position;
    public Vector2 Velocity;
    public float Scale;
    public float Rotation;
    public float Lifetime;
    public float Alpha;
    public float TimeSinceStart;
    public bool doFade;
    public float RotationSpeed;
    public float scaleRate;
    public bool OrientToDirection;
    public float Gravity;
    public float decayFactor;
    public bool Follow;
    public float startRotation;

    public bool Active => (double) this.TimeSinceStart < (double) this.Lifetime;

    public void Initialize(
      Vector2 position,
      Vector2 velocity,
      float lifetime,
      float scale,
      float rotationSpeed)
    {
      this.Origin = position;
      this.Position = position;
      this.Velocity = velocity;
      this.Lifetime = lifetime;
      this.Scale = scale;
      this.RotationSpeed = rotationSpeed;
      this.TimeSinceStart = 0.0f;
      this.Decay = 1f;
      this.Alpha = 1f;
    }

    public void Update(float dt, Vector2 positionDelta)
    {
      if (this.Follow)
        this.Position += this.Velocity * dt + positionDelta;
      else
        this.Position += this.Velocity * dt;
      this.Velocity.X /= this.Decay;
      this.Velocity.Y /= this.Decay;
      this.Velocity.Y += this.Gravity * dt;
      if (this.OrientToDirection)
        this.Rotation = this.startRotation + (float) Math.Atan2((double) this.Velocity.Y, (double) this.Velocity.X);
      else
        this.Rotation += this.RotationSpeed * dt;
      this.Scale += this.scaleRate * dt;
      this.Scale = Math.Max(0.0f, this.Scale);
      this.Decay += this.decayFactor * dt;
      this.Alpha = !this.doFade ? 1f : (float) (1.0 - (double) this.TimeSinceStart / (double) this.Lifetime);
      this.TimeSinceStart += dt;
    }
  }
}