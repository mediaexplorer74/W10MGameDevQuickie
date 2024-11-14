// Decompiled with JetBrains decompiler
// Type: GameManager.InvadersSputnik
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public class InvadersSputnik : SceneNode
  {
    public bool returnToPool;
    private Sprite sputnik;
    private Vector3 targetPosition;

    public InvadersSputnik(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.sputnik = new Sprite("sputnikbomb", Vector2.Zero, "sheets/invaders", this.Screen);
      this.Add((SceneNode) this.sputnik);
      this.SetSourceRect();
    }

    public void InitFromPool()
    {
      this.returnToPool = false;
      float num = Tools.RandomFloat(-300f, 300f);
      this.sputnik.Position = new Vector3(Tools.RandomBool() ? -80f : 560f, 400f + num, 0.0f);
      this.targetPosition = (double) this.sputnik.Position.X != -80.0 ? new Vector3(-80f, (float) (400.0 + (double) num * -1.0), 0.0f) : new Vector3(560f, (float) (400.0 + (double) num * -1.0), 0.0f);
      this.sputnik.Direction = this.targetPosition - this.sputnik.Position;
      this.sputnik.Direction.Normalize();
      this.sputnik.Rotation = 0.0f;
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      Sprite sputnik = this.sputnik;
      sputnik.Position = sputnik.Position + (float) gameTime.ElapsedGameTime.TotalSeconds * 100f * this.sputnik.Direction;
      this.sputnik.Rotation += (float) (gameTime.ElapsedGameTime.TotalSeconds * 150.0);
      if ((double) this.targetPosition.X == -80.0 && (double) this.sputnik.Position.X <= -80.0)
        this.returnToPool = true;
      else if ((double) this.targetPosition.X == 560.0 && (double) this.sputnik.Position.X >= 560.0)
        this.returnToPool = true;
      base.Update(gameTime, ref worldTransform);
    }

    public bool HitCheck(Vector2 where)
    {
      bool flag = this.sputnik.Bounds.Contains(where);
      if (this.returnToPool || !flag)
        return false;
      ((InvadersScreen) this.Screen).uiNode.AddScore(500);
      this.returnToPool = true;
      ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(2, this.sputnik.Position.ToVector2());
      ((InvadersScreen) this.Screen).fxNode.SputnikBurst(this.sputnik.Position.ToVector2());
      return true;
    }
  }
}
