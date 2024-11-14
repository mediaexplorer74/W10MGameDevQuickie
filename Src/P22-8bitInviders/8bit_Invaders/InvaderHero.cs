// Decompiled with JetBrains decompiler
// Type: GameManager.InvaderHero
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameManager
{
  public class InvaderHero : SceneNode
  {
    public CelSprite hero;
    private bool isCentered = true;
    private float currentFrame;
    private float targetFrame;
    private float amount;
    private float flashTimer;
    public bool isFlashing;
    public bool isAlive = true;
    private int flashCount;
    private float targetX = 240f;
    private Rectangle bounds = new Rectangle(0, 0, 480, 800);

    public InvaderHero(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.hero = new CelSprite("hero", new Vector2(240f, 710f), new Point(60, 38), "sheets/invaders", this.Screen);
      this.hero.AddClip("hero", 30, 175, 801, 19, CelAnimType.PlayOnce);
      this.hero.SetFrame("hero", 9);
      this.Add((SceneNode) this.hero);
      this.currentFrame = 0.0f;
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.flashTimer = Math.Max(0.0f, this.flashTimer - totalSeconds);
      if (this.isAlive && (double) this.flashTimer == 0.0)
      {
        this.isFlashing = false;
        this.hero.Visible = true;
        this.hero.Opacity = 1f;
      }
      if (this.isFlashing)
      {
        ++this.flashCount;
        this.hero.Opacity = this.flashCount % 5 != 0 ? 1f : 0.25f;
      }
      if (this.isCentered)
      {
        if ((double) this.currentFrame < 9.0)
        {
          this.currentFrame += totalSeconds * 50f;
          if ((double) this.currentFrame > 9.0)
            this.currentFrame = 9f;
        }
        else if ((double) this.currentFrame > 9.0)
        {
          this.currentFrame -= totalSeconds * 50f;
          if ((double) this.currentFrame < 9.0)
            this.currentFrame = 9f;
        }
      }
      else if ((double) this.currentFrame < (double) this.targetFrame)
      {
        this.currentFrame += totalSeconds * 50f;
        if ((double) this.currentFrame > (double) this.targetFrame)
          this.currentFrame = this.targetFrame;
      }
      else if ((double) this.currentFrame > (double) this.targetFrame)
      {
        this.currentFrame -= totalSeconds * 50f;
        if ((double) this.currentFrame < (double) this.targetFrame)
          this.currentFrame = this.targetFrame;
      }
      if (!Globals.saveManager.optionsState.usingTouch)
      {
        this.hero.Position.X += totalSeconds * Tools.RemapValue(Math.Abs(this.amount), 0.0f, 1f, 50f, 700f) * this.Direction.X;
        this.hero.Position.X = MathHelper.Clamp(this.hero.Position.X, 30f, 450f);
      }
      else
      {
        if (Tools.WithinTolerance(this.hero.Position.X, this.targetX, 10f))
        {
          this.hero.Position.X = this.targetX;
          this.CenterStick();
        }
        else
        {
          if ((double) this.hero.Position.X < (double) this.targetX)
            this.MoveRight(1f);
          else
            this.MoveLeft(-1f);
          this.hero.Position.X += totalSeconds * Tools.RemapValue(Math.Abs(this.amount), 0.0f, 1f, 50f, 600f) * this.Direction.X;
        }
        this.hero.Position.X = MathHelper.Clamp(this.hero.Position.X, 30f, 450f);
      }
      this.hero.SetFrame("hero", (int) this.currentFrame);
      base.Update(gameTime, ref worldTransform);
    }

    public void MoveLeft(float amt)
    {
      this.isCentered = false;
      this.targetFrame = 0.0f;
      this.Direction = Vector3.Left;
      this.amount = amt;
    }

    public void MoveRight(float amt)
    {
      this.isCentered = false;
      this.targetFrame = 18f;
      this.Direction = Vector3.Right;
      this.amount = amt;
    }

    public void SetXTouchPosition(float xval)
    {
      this.isCentered = true;
      this.targetX = xval;
    }

    public void CenterStick()
    {
      this.isCentered = true;
      this.Direction = Vector3.Zero;
    }

    public void Kill()
    {
      if (this.isFlashing)
        return;
      ((InvadersScreen) this.Screen).uiNode.RemoveLife();
      if (((InvadersScreen) this.Screen).uiNode.Lives != 0)
        return;
      this.hero.Visible = false;
      this.isAlive = false;
    }

    public void Reset()
    {
      this.CenterStick();
      this.targetX = 240f;
      this.hero.Visible = true;
      this.isAlive = true;
      this.isFlashing = true;
      this.flashTimer = 3f;
      this.flashCount = 0;
    }
  }
}
