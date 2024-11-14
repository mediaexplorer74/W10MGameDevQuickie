// Decompiled with JetBrains decompiler
// Type: GameEngine.InterpolatorCollection
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameEngine
{
  public sealed class InterpolatorCollection
  {
    private readonly AdvancedObjectPool<Interpolator> interpolators;

    public InterpolatorCollection()
    {
      this.interpolators = new AdvancedObjectPool<Interpolator>(10);
      for (int index = 0; index < 10; ++index)
        this.interpolators.AddToPool(new Interpolator());
    }

    public Interpolator NewInterpolator(
      float start,
      float end,
      Action<Interpolator> step,
      Action<Interpolator> completed)
    {
      return this.NewInterpolator(start, end, 1f, InterpolatorScales.Linear, step, completed);
    }

    public Interpolator NewInterpolator(
      float start,
      float end,
      float length,
      Action<Interpolator> step,
      Action<Interpolator> completed)
    {
      return this.NewInterpolator(start, end, length, InterpolatorScales.Linear, step, completed);
    }

    public Interpolator NewInterpolator(
      float start,
      float end,
      float length,
      InterpolatorScaleDelegate scale,
      Action<Interpolator> step,
      Action<Interpolator> completed)
    {
      Interpolator interpolator = (Interpolator) null;
      if (this.interpolators.AvailableCount > 0)
      {
        interpolator = this.interpolators.Get().Item;
        interpolator.Reset(start, end, length, scale, step, completed);
      }
      return interpolator;
    }

    public void Update(GameTime gameTime)
    {
      lock (this.interpolators)
      {
        foreach (AdvancedObjectPool<Interpolator>.Node activeNode in this.interpolators.ActiveNodes)
        {
          activeNode.Item.Update(gameTime);
          if (activeNode.Item.returnToPool)
            this.interpolators.Return(activeNode);
        }
      }
    }
  }
}
