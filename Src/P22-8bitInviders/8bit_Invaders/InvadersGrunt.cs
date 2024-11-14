// Decompiled with JetBrains decompiler
// Type: GameManager.InvadersGrunt
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public class InvadersGrunt : InvadersCritterBase
  {
    public InvadersGrunt(GameScreen screen)
      : base(screen)
    {
    }

    public override void Initialize()
    {
      this.ship = new Sprite("grunt", Vector2.Zero, "sheets/invaders", this.Screen);
      this.Add((SceneNode) this.ship);
      this.SetSourceRect();
    }

    public override void InitFromPool(int row, int column, Color color)
    {
      base.InitFromPool(row, column, color);
      this.Health = 1;
    }

    public override void Update(Vector2 root, GameTime gameTime, ref Matrix worldTransform)
    {
      this.ship.Position.X = root.X + (float) (this.Column * 40);
      this.ship.Position.Y = root.Y + (float) (this.Row * 40);
      if (this.Health <= 0)
      {
        ((InvadersScreen) this.Screen).fxAddLayer.AddExplosionRandom(this.ship.Position.ToVector2());
        this.returnToPool = true;
      }
      this.Update(gameTime, ref worldTransform);
    }
  }
}
