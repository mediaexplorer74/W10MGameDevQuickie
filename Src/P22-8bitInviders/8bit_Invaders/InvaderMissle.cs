// Decompiled with JetBrains decompiler
// Type: GameManager.InvaderMissle
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public class InvaderMissle : SceneNode
  {
    public Sprite missle;

    public InvaderMissle(GameScreen screen)
      : base(screen)
    {
      this.missle = new Sprite(nameof (missle), Vector2.Zero, "sheets/invaders", screen);
      this.Add((SceneNode) this.missle);
      this.SetSourceRect();
    }

    public void InitFromPool(Vector2 where)
    {
      this.missle.Rotation = 0.0f;
      this.missle.Position = where.ToVector3();
      this.missle.Direction = ((InvadersScreen) this.Screen).hero.hero.Position - this.missle.Position;
      this.missle.Direction.Z = 0.0f;
      this.missle.Direction.Normalize();
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.missle.Rotation += totalSeconds + 100f;
      if ((double) this.missle.Position.Y <= 540.0)
      {
        this.missle.Direction = ((InvadersScreen) this.Screen).hero.hero.Position - this.missle.Position;
        this.missle.Direction.Z = 0.0f;
        this.missle.Direction.Normalize();
      }
      Sprite missle = this.missle;
      missle.Position = missle.Position + this.missle.Direction * totalSeconds * 200f;
      base.Update(gameTime, ref worldTransform);
    }
  }
}
