// Decompiled with JetBrains decompiler
// Type: GameEngine.Spring
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class Spring
  {
    public Spring.SpringMode springMode = Spring.SpringMode.Paused;
    public Vector2 Velocity = Vector2.Zero;
    public Vector2 Stiffness = new Vector2(100f);
    public Vector2 Damping = new Vector2(10f);
    public float Mass = 1f;
    public float maxVelocity = 1000f;
    public float Tolerance = 1f / 1000f;
    public Vector2 Value;
    public Vector2 Start;
    public Vector2 Target;

    public event Spring.OnFinishedHandler OnFinished;

    public Spring()
    {
    }

    public Spring(Vector2 start, Vector2 target, bool startnow)
    {
      this.springMode = !startnow ? Spring.SpringMode.Paused : Spring.SpringMode.Playing;
      this.Start = start;
      this.Target = target;
      this.Value = this.Start;
    }

    public void Restart(Vector2 start, Vector2 target)
    {
      this.Start = start;
      this.Target = target;
      this.Value = this.Start;
      this.springMode = Spring.SpringMode.Playing;
    }

    public void Stop() => this.springMode = Spring.SpringMode.Paused;

    public Spring(float start, float target, bool startnow)
    {
      this.springMode = !startnow ? Spring.SpringMode.Paused : Spring.SpringMode.Playing;
      this.Start = new Vector2(start);
      this.Target = new Vector2(target);
      this.Value = this.Start;
    }

    public void Restart(float start, float target)
    {
      this.Restart(new Vector2(start), new Vector2(target));
    }

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
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      if (this.springMode == Spring.SpringMode.Paused)
        return;
      Vector2 vector2_1 = currentvalue;
      Vector2 vector2_2 = vector2_1 - this.Target;
      if (Tools.WithinToleranceV2(vector2_1.X, this.Target.X, this.Tolerance) && Tools.WithinToleranceV2(vector2_1.Y, this.Target.Y, this.Tolerance))
        vector2_2 = Vector2.Zero;
      this.Velocity += (-this.Stiffness * vector2_2 - this.Damping * this.Velocity) / this.Mass * totalSeconds;
      this.Velocity.X = MathHelper.Clamp(this.Velocity.X, -this.maxVelocity, this.maxVelocity);
      this.Velocity.Y = MathHelper.Clamp(this.Velocity.Y, -this.maxVelocity, this.maxVelocity);
      this.Value = vector2_1 + this.Velocity * totalSeconds;
      currentvalue = this.Value;
      if (!(vector2_2 == Vector2.Zero))
        return;
      currentvalue = this.Target;
      this.springMode = Spring.SpringMode.Paused;
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
