// Decompiled with JetBrains decompiler
// Type: GameEngine.VectorAnim
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class VectorAnim : Anim
  {
    private Vector3 startval;

    public VectorAnim(string name, Vector3 startvalue, Vector3 targetval)
      : base(name, startvalue, targetval)
    {
      this.startval = startvalue;
    }

    public VectorAnim(
      string name,
      Vector3 startvalue,
      Vector3 targetval,
      float speed,
      MotionType motiontype,
      EndBehavior endbehavior)
      : base(name, startvalue, targetval)
    {
      this.Speed = speed;
      this.endBehavior = endbehavior;
      this.Motion = motiontype;
      this.startval = startvalue;
    }

    public override void Play()
    {
      this.current = this.startval;
      this.StartValue = this.startval;
      this.vector = this.TargetValue - this.StartValue;
      this.lapSpeed = this.Speed / this.vector.Length();
      this.Weight = 0.0f;
      base.Play();
    }
  }
}
