// Decompiled with JetBrains decompiler
// Type: GameEngine.InterpolatorScales
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

#nullable disable
namespace GameEngine
{
  public static class InterpolatorScales
  {
    public static readonly InterpolatorScaleDelegate Linear = new InterpolatorScaleDelegate(InterpolatorScales.LinearInterpolation);
    public static readonly InterpolatorScaleDelegate Quadratic = new InterpolatorScaleDelegate(InterpolatorScales.QuadraticInterpolation);
    public static readonly InterpolatorScaleDelegate Cubic = new InterpolatorScaleDelegate(InterpolatorScales.CubicInterpolation);
    public static readonly InterpolatorScaleDelegate Quartic = new InterpolatorScaleDelegate(InterpolatorScales.QuarticInterpolation);

    private static float LinearInterpolation(float progress) => progress;

    private static float QuadraticInterpolation(float progress) => progress * progress;

    private static float CubicInterpolation(float progress) => progress * progress * progress;

    private static float QuarticInterpolation(float progress)
    {
      return progress * progress * progress * progress;
    }
  }
}
