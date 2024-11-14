// Decompiled with JetBrains decompiler
// Type: GameManager.InvadersMothership
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public class InvadersMothership : SceneNode
  {
    public Sprite mothership;
    public bool returnToPool;
    private int dir;

    public InvadersMothership(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.mothership = new Sprite("mothership", Vector2.Zero, "sheets/invaders", this.Screen);
      this.Add((SceneNode) this.mothership);
      this.SetSourceRect();
    }

    public bool HitCheck(Vector2 where)
    {
      bool flag = this.mothership.Bounds.Contains(where);
      if (this.returnToPool || !flag)
        return false;
      ((InvadersScreen) this.Screen).uiNode.AddScore(1000);
      this.returnToPool = true;
      ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(2, this.mothership.Position.ToVector2());
      return true;
    }

    public void InitFromPool()
    {
      this.returnToPool = false;
      this.mothership.Position = new Vector3(Tools.RandomBool() ? -80f : 560f, 145f, 0.0f);
      if ((double) this.mothership.Position.X == -80.0)
        this.dir = 1;
      else
        this.dir = -1;
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      this.mothership.Rotation = Tools.SineAnimation(gameTime, 10f, -10f, 10f);
      this.mothership.Position.X += (float) ((double) this.dir * gameTime.ElapsedGameTime.TotalSeconds * 150.0);
      if (this.dir == 1 && (double) this.mothership.Position.X > 500.0)
        this.returnToPool = true;
      else if (this.dir == -1 && (double) this.mothership.Position.X < -20.0)
        this.returnToPool = true;
      base.Update(gameTime, ref worldTransform);
    }
  }
}
