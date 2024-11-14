// Decompiled with JetBrains decompiler
// Type: GameManager.InvaderExplosion
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameManager
{
  public class InvaderExplosion : SceneNode
  {
    private CelSprite exp;
    public bool returnToPool;
    private int maxframes;

    public InvaderExplosion(int type, GameScreen screen)
      : base(screen)
    {
      switch (type)
      {
        case 1:
          this.exp = new CelSprite("explosion", Vector2.Zero, new Point(128, 128), "sheets/invaders", screen);
          this.exp.AddClip(nameof (exp), 20, 175, 840, 6, CelAnimType.PlayOnce);
          this.Add((SceneNode) this.exp);
          this.maxframes = 5;
          break;
        case 2:
          this.exp = new CelSprite("explosion2", Vector2.Zero, new Point(64, 64), "sheets/invaders", screen);
          this.exp.AddClip(nameof (exp), 20, 995, 840, 16, CelAnimType.PlayOnce, 256);
          this.Add((SceneNode) this.exp);
          this.maxframes = 15;
          break;
        case 3:
          this.exp = new CelSprite("explosion2", Vector2.Zero, new Point(64, 64), "sheets/invaders", screen);
          this.exp.AddClip(nameof (exp), 20, 995, 840, 16, CelAnimType.PlayOnce, 256);
          this.Add((SceneNode) this.exp);
          this.maxframes = 15;
          this.exp.Scale = 2f;
          break;
      }
    }

    public void InitFromPool(Vector2 where)
    {
      this.returnToPool = false;
      this.exp.Position = where.ToVector3();
      this.exp.Opacity = 1f;
      this.exp.SetFrame("exp", 0);
      this.exp.Rotation = (float) Tools.RandomInt(360);
      this.exp.Play();
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.exp.Opacity = Math.Max(0.0f, this.exp.Opacity - totalSeconds * 0.45f);
      this.exp.Rotation += totalSeconds * 50f;
      if (this.exp.CurrentClip.currentFrame == this.maxframes)
        this.returnToPool = true;
      base.Update(gameTime, ref worldTransform);
    }
  }
}
