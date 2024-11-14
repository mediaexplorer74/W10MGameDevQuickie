// Decompiled with JetBrains decompiler
// Type: GameManager.InvaderMeteorite
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public class InvaderMeteorite : SceneNode
  {
    public Sprite meteor;
    private int Health;
    public bool returnToPool;
    public float speed;

    public InvaderMeteorite(GameScreen screen)
      : base(screen)
    {
      this.meteor = new Sprite("meteroite", Vector2.Zero, "sheets/invaders", screen);
      this.Add((SceneNode) this.meteor);
      this.SetSourceRect();
    }

    public void InitFromPool()
    {
      this.speed = Tools.RandomFloat(150f, 250f);
      this.returnToPool = false;
      this.Health = 5;
      this.meteor.Rotation = 0.0f;
      this.meteor.Position = new Vector3(Tools.RandomFloat(48f, 432f), -50f, 0.0f);
    }

    public bool HitCheck(Vector2 where)
    {
      bool flag = this.meteor.Bounds.Contains(where);
      if (this.returnToPool || !flag)
        return false;
      --this.Health;
      if (this.Health <= 0)
      {
        ((InvadersScreen) this.Screen).uiNode.AddScore(200);
        this.returnToPool = true;
      }
      ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(2, this.meteor.Position.ToVector2());
      return true;
    }
  }
}
