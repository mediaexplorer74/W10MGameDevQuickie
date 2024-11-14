// Decompiled with JetBrains decompiler
// Type: GameEngine.Interpolator
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameEngine
{
  public sealed class Interpolator
  {
    private bool valid;
    private float progress;
    private float start;
    private float end;
    private float range;
    private float speed;
    private float value;
    private InterpolatorScaleDelegate scale;
    private Action<Interpolator> step;
    private Action<Interpolator> completed;
    public bool returnToPool;

    public bool IsActive => this.valid;

    public float Progress => this.progress;

    public float Start => this.start;

    public float End => this.end;

    public float Value => this.value;

    public object Tag { get; set; }

    internal Interpolator()
    {
    }

    public Interpolator(
      float startValue,
      float endValue,
      Action<Interpolator> step,
      Action<Interpolator> completed)
      : this(startValue, endValue, 1f, InterpolatorScales.Linear, step, completed)
    {
      this.returnToPool = false;
    }

    public Interpolator(
      float startValue,
      float endValue,
      float interpolationLength,
      Action<Interpolator> step,
      Action<Interpolator> completed)
      : this(startValue, endValue, interpolationLength, InterpolatorScales.Linear, step, completed)
    {
      this.returnToPool = false;
    }

    public Interpolator(
      float startValue,
      float endValue,
      float interpolationLength,
      InterpolatorScaleDelegate scale,
      Action<Interpolator> step,
      Action<Interpolator> completed)
    {
      this.returnToPool = false;
      this.Reset(startValue, endValue, interpolationLength, scale, step, completed);
    }

    public void Stop() => this.valid = false;

    public void ForceFinish()
    {
      if (!this.valid)
        return;
      this.valid = false;
      this.progress = 1f;
      this.value = this.start + this.range * this.scale(this.progress);
      if (this.step != null)
        this.step(this);
      if (this.completed == null)
        return;
      this.completed(this);
    }

    internal void Reset(
      float s,
      float e,
      float l,
      InterpolatorScaleDelegate scaleFunc,
      Action<Interpolator> stepFunc,
      Action<Interpolator> completedFunc)
    {
      if ((double) l <= 0.0)
        throw new ArgumentException("length must be greater than zero");
      if (scaleFunc == null)
        throw new ArgumentNullException(nameof (scaleFunc));
      this.valid = true;
      this.progress = 0.0f;
      this.start = s;
      this.end = e;
      this.range = e - s;
      this.speed = 1f / l;
      this.scale = scaleFunc;
      this.step = stepFunc;
      this.completed = completedFunc;
    }

    public void Update(GameTime gameTime)
    {
      if (!this.valid)
        return;
      this.progress = Math.Min(this.progress + this.speed * (float) gameTime.ElapsedGameTime.TotalSeconds, 1f);
      this.value = this.start + this.range * this.scale(this.progress);
      if (this.step != null)
        this.step(this);
      if ((double) this.progress != 1.0)
        return;
      this.valid = false;
      this.returnToPool = true;
      if (this.completed != null)
        this.completed(this);
      this.Tag = (object) null;
      this.scale = (InterpolatorScaleDelegate) null;
      this.step = (Action<Interpolator>) null;
      this.completed = (Action<Interpolator>) null;
    }
  }
}
