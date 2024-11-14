// Decompiled with JetBrains decompiler
// Type: GameEngine.PositionAnim
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class PositionAnim : Anim
  {
    private SceneNode entity;

    public PositionAnim(string name, SceneNode node, Vector3 target)
      : base(name, node.Position, target)
    {
      this.entity = node;
    }

    public PositionAnim(
      string name,
      SceneNode node,
      Vector3 target,
      float speed,
      MotionType motiontype,
      EndBehavior endbehavior)
      : base(name, node.Position, target)
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
        this.entity.Position = this.CurrentVector3Value;
      }
    }

    public override void Play()
    {
      this.current = this.entity.Position;
      this.StartValue = this.entity.Position;
      this.vector = this.TargetValue - this.StartValue;
      this.lapSpeed = this.Speed / this.vector.Length();
      this.Weight = 0.0f;
      base.Play();
    }
  }
}
