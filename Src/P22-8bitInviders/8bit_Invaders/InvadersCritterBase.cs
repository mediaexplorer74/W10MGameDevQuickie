// Decompiled with JetBrains decompiler
// Type: GameManager.InvadersCritterBase
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public abstract class InvadersCritterBase : SceneNode
  {
    public bool returnToPool;
    public int Row;
    public int Column;
    public float startX;
    public Sprite ship;
    protected int Health;
    protected Color colorTint;

    public InvadersCritterBase(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public virtual void InitFromPool(int row, int column, Color color)
    {
      this.returnToPool = false;
      this.Row = row;
      this.Column = column;
      this.colorTint = color;
      this.ship.color = color;
      this.ship.Position.X = (float) (40 + this.Column * 40);
      this.ship.Position.Y = (float) (130 + this.Row * 40);
      this.startX = this.ship.Position.X;
    }

    public virtual bool HitCheck(Vector2 where)
    {
      if (this.returnToPool || !this.ship.Bounds.Contains(where))
        return false;
      --this.Health;
      return true;
    }

    public virtual void Update(Vector2 root, GameTime gameTime, ref Matrix worldTransform)
    {
    }
  }
}
