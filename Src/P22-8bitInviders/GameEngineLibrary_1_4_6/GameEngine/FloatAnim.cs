// Decompiled with JetBrains decompiler
// Type: GameEngine.FloatAnim
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

#nullable disable
namespace GameEngine
{
  public class FloatAnim : Anim
  {
    private float startval;

    public float CurrentValue => this.CurrentFloatValue;

    public FloatAnim(string name, float startvalue, float targetvalue)
      : base(name, startvalue, targetvalue)
    {
      this.startval = startvalue;
    }

    public FloatAnim(
      string name,
      float startvalue,
      float targetvalue,
      float speed,
      MotionType motiontype,
      EndBehavior endbehavior)
      : base(name, startvalue, targetvalue)
    {
      this.Speed = speed;
      this.endBehavior = endbehavior;
      this.Motion = motiontype;
      this.startval = startvalue;
    }

    public override void Play()
    {
      this.current.X = this.startval;
      this.StartValue.X = this.startval;
      this.vector = this.TargetValue - this.StartValue;
      this.lapSpeed = this.Speed / this.vector.Length();
      this.Weight = 0.0f;
      base.Play();
    }
  }
}
