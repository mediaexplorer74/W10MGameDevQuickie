// Decompiled with JetBrains decompiler
// Type: GameManager.InvadersUI
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameManager
{
  public class InvadersUI : SceneNode
  {
    public BitmapNumbers scoreNumber;
    public BitmapNumbers lifeNumber;
    public BitmapNumbers levelNumber;
    public int Lives;
    private bool Rehydrate;
    private Sprite level;
    private Sprite stopTime;
    private float levelTimer;
    private float stopTimer;

    public InvadersUI(GameScreen screen, bool rehydrate)
      : base(screen)
    {
      this.Rehydrate = rehydrate;
      this.Initialize();
    }

    public override void Initialize()
    {
      this.Add((SceneNode) new Sprite("title", new Vector2(240f, 52f), "sheets/invaders", this.Screen));
      this.scoreNumber = new BitmapNumbers(new Vector2(273f, 86f), 10, this.Screen, "sheets/invaders");
      this.scoreNumber.HorizontalSqueaze = -5;
      this.Add((SceneNode) this.scoreNumber);
      this.lifeNumber = new BitmapNumbers(new Vector2(55f, 86f), this.Screen, "sheets/invaders");
      this.lifeNumber.HorizontalSqueaze = -5;
      this.Add((SceneNode) this.lifeNumber);
      BitmapNumbers bitmapNumbers = new BitmapNumbers(new Vector2(290f, 389f), this.Screen, "sheets/invaders");
      bitmapNumbers.Visible = false;
      this.levelNumber = bitmapNumbers;
      this.levelNumber.HorizontalSqueaze = -5;
      this.Add((SceneNode) this.levelNumber);
      CelSprite childItem = new CelSprite("icon", new Vector2(32f, 99f), new Point(60, 38), "sheets/invaders", this.Screen);
      childItem.AddClip("hero", 30, 175, 801, 19, CelAnimType.Bounce);
      childItem.Scale = 0.7f;
      childItem.Play();
      this.Add((SceneNode) childItem);
      Sprite sprite1 = new Sprite("level", new Vector2(240f, 400f), "sheets/invaders", this.Screen);
      sprite1.Visible = false;
      this.level = sprite1;
      this.Add((SceneNode) this.level);
      Sprite sprite2 = new Sprite("stoptime", new Vector2(240f, 400f), "sheets/invaders", this.Screen);
      sprite2.Visible = false;
      this.stopTime = sprite2;
      this.Add((SceneNode) this.stopTime);
      this.SetSourceRect();
      if (this.Rehydrate)
      {
        this.Lives = Globals.saveManager.saveState.Lives;
        this.scoreNumber.Number = Globals.saveManager.saveState.Score;
      }
      else
        this.Lives = 3;
    }

    public void Reset()
    {
      this.Lives = 3;
      this.scoreNumber.Number = 0;
      this.stopTimer = 0.0f;
      this.stopTime.Visible = false;
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      this.lifeNumber.Number = this.Lives;
      this.levelTimer = Math.Max(0.0f, this.levelTimer - (float) gameTime.ElapsedGameTime.TotalSeconds);
      if ((double) this.levelTimer == 0.0)
      {
        this.level.Visible = false;
        this.levelNumber.Visible = false;
      }
      else
      {
        this.level.Opacity = Tools.RemapValue(this.levelTimer, 0.0f, 5f, 0.0f, 1f);
        this.levelNumber.Opacity = this.level.Opacity;
      }
      if (this.stopTime.Visible)
      {
        this.stopTime.Scale = Tools.SineAnimation(gameTime, 10f, 1.25f, 0.975f);
        this.stopTime.Opacity = Tools.RemapValue(this.stopTimer, 0.0f, 5f, 0.0f, 1f);
        this.stopTimer = Math.Max(0.0f, this.stopTimer - (float) gameTime.ElapsedGameTime.TotalSeconds);
        if ((double) this.stopTimer == 0.0)
          this.stopTime.Visible = false;
      }
      base.Update(gameTime, ref worldTransform);
    }

    public void AddScore(int amount)
    {
      this.scoreNumber.Number += amount;
      if (this.scoreNumber.Number < Globals.saveManager.saveState.lastOneUp + 10000)
        return;
      this.Screen.audioManager.Play("oneup");
      ++this.Lives;
      Globals.saveManager.saveState.lastOneUp += 10000;
    }

    public void RemoveLife()
    {
      this.Lives = Math.Max(0, this.Lives - 1);
      if (this.Lives == 0)
        ((InvadersScreen) this.Screen).NotifyGameOver(false);
      else
        ((InvadersScreen) this.Screen).hero.Reset();
    }

    public void NotifyNewLevel()
    {
      this.levelNumber.Number = Globals.saveManager.saveState.Level;
      this.levelNumber.Visible = true;
      this.level.Visible = true;
      this.Screen.audioManager.Play("newlevel");
      this.levelTimer = 5f;
    }

    public void NotifyStopTime()
    {
      this.stopTime.Visible = true;
      this.stopTime.Opacity = 1f;
      this.stopTimer = 5f;
    }
  }
}
