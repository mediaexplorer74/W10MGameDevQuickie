// Decompiled with JetBrains decompiler
// Type: GameEngine.ScaleAnim
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class ScaleAnim : Anim
  {
    private SceneNode entity;

    public ScaleAnim(string name, SceneNode node, float target)
      : base(name, node.Scale, target)
    {
      this.entity = node;
    }

    public ScaleAnim(
      string name,
      SceneNode node,
      float target,
      float speed,
      MotionType motiontype,
      EndBehavior endbehavior)
      : base(name, node.Scale, target)
    {
      this.Speed = speed;
      this.endBehavior = endbehavior;
      this.Motion = motiontype;
      this.entity = node;
    }

    public override void UpdateAnimation(GameTime gameTime)
    {
      if (this.entity == null)
      {
        Engine.aniManager.RemoveAnim(this.Name);
      }
      else
      {
        base.UpdateAnimation(gameTime);
        if (this.Status != AnimStatus.Running)
          return;
        this.entity.Scale = this.CurrentFloatValue;
      }
    }

    public override void Play()
    {
      this.current.X = this.entity.Scale;
      this.StartValue.X = this.entity.Scale;
      this.vector = this.TargetValue - this.StartValue;
      this.lapSpeed = this.Speed / this.vector.Length();
      this.Weight = 0.0f;
      base.Play();
    }
  }
}
