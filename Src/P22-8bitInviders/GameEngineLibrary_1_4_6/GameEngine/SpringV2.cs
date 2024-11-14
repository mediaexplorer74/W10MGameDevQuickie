// Decompiled with JetBrains decompiler
// Type: GameEngine.SpringV2
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameEngine
{
  public class SpringV2
  {
    public SpringV2.SpringMode springMode = SpringV2.SpringMode.Paused;
    public float BoingPower = 2.2f;
    public float BoingCoeff = 2.5f;
    public float BoingCoeff2 = 1.2f;
    public Vector2 Start;
    public Vector2 Target;
    protected Vector2 vector;
    private float _speed = 50f;
    protected float lapSpeed;
    protected float Weight;
    public Vector2 Value;

    public event SpringV2.OnFinishedHandler OnFinished;

    public float Speed
    {
      get => this._speed;
      set
      {
        this._speed = value / 1000f;
        this.lapSpeed = this._speed / this.vector.Length();
      }
    }

    public SpringV2() => this.Speed = 50f;

    public SpringV2(Vector2 start, Vector2 target, bool startnow)
    {
      this.Speed = 50f;
      this.springMode = !startnow ? SpringV2.SpringMode.Paused : SpringV2.SpringMode.Playing;
      this.Start = start;
      this.Target = target;
      this.Value = this.Start;
      this.vector = this.Target - this.Start;
      this.lapSpeed = this._speed / this.vector.Length();
    }

    public SpringV2(float start, float target, bool startnow)
    {
      this.Speed = 50f;
      this.springMode = !startnow ? SpringV2.SpringMode.Paused : SpringV2.SpringMode.Playing;
      this.Start = new Vector2(start);
      this.Target = new Vector2(target);
      this.Value = this.Start;
      this.vector = this.Target - this.Start;
      this.lapSpeed = this._speed / this.vector.Length();
    }

    public void Restart(Vector2 start, Vector2 target)
    {
      this.Start = start;
      this.Target = target;
      this.Value = this.Start;
      this.vector = this.Target - this.Start;
      this.lapSpeed = this._speed / this.vector.Length();
      this.springMode = SpringV2.SpringMode.Playing;
      this.Weight = 0.0f;
    }

    public void Restart(float start, float target)
    {
      this.Restart(new Vector2(start), new Vector2(target));
    }

    public void Stop() => this.springMode = SpringV2.SpringMode.Paused;

    public void Update(Vector2 currentvalue, GameTime gameTime)
    {
      this.Update(ref currentvalue, gameTime);
    }

    public void Update(float currentvalue, GameTime gameTime)
    {
      this.Update(new Vector2(currentvalue), gameTime);
    }

    public void Update(ref Vector2 currentvalue, GameTime gameTime)
    {
      if (this.springMode == SpringV2.SpringMode.Paused)
        return;
      this.Weight += this.lapSpeed * (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      this.Value = this.Start + (this.Target - this.Start) * (((float) (Math.Sin((double) this.Weight * Math.PI * ((double) this.BoingCoeff * (double) this.Weight * (double) this.Weight * (double) this.Weight)) * Math.Pow(1.0 - (double) this.Weight, (double) this.BoingPower)) + this.Weight) * (float) (1.0 + (double) this.BoingCoeff2 * (1.0 - (double) this.Weight)));
      if ((double) this.Weight <= 0.99995)
        return;
      this.Value = this.Target;
      this.springMode = SpringV2.SpringMode.Paused;
      if (this.OnFinished == null)
        return;
      this.OnFinished();
    }

    public delegate void OnFinishedHandler();

    public enum SpringMode
    {
      Playing,
      Paused,
    }
  }
}
