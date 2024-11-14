// Decompiled with JetBrains decompiler
// Type: GameManager.AsteroidUINode
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;

#nullable disable
namespace GameManager
{
  public class AsteroidUINode : SceneNode
  {
    public VirtualStickV2 moveStick;
    public VirtualStickV2 shootStick;
    public BitmapNumbers scoreNumber;
    public BitmapNumbers lifeNumber;
    public BitmapNumbers levelNumber;
    public Vector2 stickValue = Vector2.Zero;
    public float rotationFromStick;
    public int Lives;
    private UIManagerMultiTouch uiNode;
    private bool tractorON;
    private int sputSaved;
    private Sprite victory;
    private Sprite victoryGlow;

    public AsteroidUINode(GameScreen screen)
      : base(screen)
    {
      this.handleInput = true;
      this.Initialize();
    }

    public override void Initialize()
    {
      string str = "sheets/asteroids";
      this.Add((SceneNode) new Sprite("title", new Vector2(240f, 52f), str, this.Screen));
      this.scoreNumber = new BitmapNumbers(new Vector2(273f, 86f), 10, this.Screen, str);
      this.scoreNumber.HorizontalSqueaze = -5;
      this.Add((SceneNode) this.scoreNumber);
      this.lifeNumber = new BitmapNumbers(new Vector2(55f, 86f), this.Screen, str);
      this.lifeNumber.HorizontalSqueaze = -5;
      this.Add((SceneNode) this.lifeNumber);
      BitmapNumbers bitmapNumbers = new BitmapNumbers(new Vector2(290f, 389f), this.Screen, str);
      bitmapNumbers.Visible = false;
      this.levelNumber = bitmapNumbers;
      this.levelNumber.HorizontalSqueaze = -5;
      this.Add((SceneNode) this.levelNumber);
      CelSprite childItem = new CelSprite("icon", new Vector2(32f, 99f), new Point(60, 38), str, this.Screen);
      childItem.AddClip("hero", 30, 0, 115, 19, CelAnimType.Bounce);
      childItem.Scale = 0.7f;
      childItem.Play();
      this.Add((SceneNode) childItem);
      this.Add((SceneNode) new Sprite("movepot", new Vector2(100f, 700f), str, this.Screen)
      {
        Opacity = 0.5f
      });
      this.moveStick = new VirtualStickV2(new Vector2(100f, 700f), "movenub", str, 60, 200, this.Screen);
      this.Add((SceneNode) this.moveStick);
      this.Add((SceneNode) new Sprite("shootpot", new Vector2(380f, 700f), str, this.Screen)
      {
        Opacity = 0.5f
      });
      this.shootStick = new VirtualStickV2(new Vector2(380f, 700f), "shootnub", str, 60, 200, this.Screen);
      this.Add((SceneNode) this.shootStick);
      this.uiNode = new UIManagerMultiTouch(this.Screen);
      this.uiNode.FirePressedWithName += new UIManagerMultiTouch.OnPressedWithNameHandler(this.uiNode_FirePressedWithName);
      this.uiNode.RegisterWidget((Widget) new WidgetButton("tractor_off", new Vector2(424f, 540f), str, this.Screen)
      {
        pressedColor = Color.DodgerBlue
      });
      this.Add((SceneNode) this.uiNode);
      this.victory = new Sprite("victory", new Vector2(240f, 400f), str, this.Screen);
      this.victory.Visible = false;
      this.Add((SceneNode) this.victory);
      this.victoryGlow = new Sprite("victoryglow", new Vector2(240f, 400f), str, this.Screen);
      this.victoryGlow.Visible = false;
      this.Add((SceneNode) this.victoryGlow);
      this.SetSourceRect();
      this.Lives = Globals.saveManager.saveState.Lives;
      this.scoreNumber.Number = Globals.saveManager.saveState.Score;
      this.tractorON = false;
    }

    private void uiNode_FirePressedWithName(int ID, string btnName)
    {
      if (ID != 0)
        return;
      if ((double) this.uiNode[0].Opacity == 0.5)
        this.Screen.audioManager.Play("cancel");
      else if (!this.tractorON)
      {
        this.tractorON = true;
        this.Screen.audioManager.Play("sputnik");
        ((AsteroidScreen) this.Screen).playLayer.ActivateTractor();
      }
      else
      {
        this.tractorON = false;
        this.Screen.audioManager.Play("cancel");
        ((AsteroidScreen) this.Screen).playLayer.DisableTractor();
        ((AsteroidScreen) this.Screen).additiveLayer.DisableTractor();
      }
    }

    public void ActivateTractorButton()
    {
      this.uiNode[0].Opacity = 1f;
      ((WidgetButton) this.uiNode[0]).RenameSourceRect("tractor_on");
    }

    public void DisableTractorButton()
    {
      this.uiNode[0].Opacity = 0.5f;
      ((WidgetButton) this.uiNode[0]).RenameSourceRect("tractor_off");
    }

    public void Reset()
    {
      this.Lives = 3;
      this.scoreNumber.Number = 0;
    }

    public override bool HandleInput(GestureSample gesture)
    {
      this.uiNode.HandleTouch(gesture);
      return base.HandleInput(gesture);
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      this.lifeNumber.Number = this.Lives;
      if (!((AsteroidScreen) this.Screen).gameOver)
      {
        if (this.moveStick.ThumbstickAngle.HasValue)
        {
          this.stickValue = this.moveStick.ThumbstickValue;
          if ((double) this.stickValue.X != 0.0 || (double) this.stickValue.Y != 0.0)
          {
            double degrees = (double) MathHelper.ToDegrees(this.moveStick.ThumbstickAngle.Value);
            this.rotationFromStick = (float) Math.Atan2((double) this.stickValue.Y, (double) this.stickValue.X);
            this.rotationFromStick = MathHelper.ToDegrees(this.rotationFromStick) + 90f;
          }
        }
        else
          this.stickValue = Vector2.Zero;
        if (this.shootStick.ThumbstickAngle.HasValue)
        {
          Vector2 thumbstickValue = this.shootStick.ThumbstickValue;
          if ((double) thumbstickValue.X != 0.0 || (double) thumbstickValue.Y != 0.0)
            ((AsteroidScreen) this.Screen).playLayer.HeroShoot(thumbstickValue);
        }
      }
      if ((double) this.uiNode[0].Opacity == 1.0)
        this.uiNode[0].Scale = Tools.SineAnimation(gameTime, 40f, 0.95f, 1.05f);
      else
        this.uiNode[0].Scale = 1f;
      if (this.victory.Visible)
      {
        this.victory.Scale = Tools.SineAnimation(gameTime, 20f, 0.9f, 1.1f);
        this.victoryGlow.Scale = this.victory.Scale;
        this.victoryGlow.Opacity = Tools.SineAnimation(gameTime, 40f, 0.0f, 1f);
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
      if (this.Lives != 0)
        return;
      ((AsteroidScreen) this.Screen).NotifyGameOver(false);
    }

    public void AddSputnik()
    {
      this.sputSaved = Math.Min(this.sputSaved + 1, 4);
      this.AddScore(2500);
      this.Screen.audioManager.Play("collected");
      if (this.sputSaved != 4)
        return;
      ((AsteroidScreen) this.Screen).NotifyGameOver(true);
    }

    public void NotifyVictory()
    {
      this.victory.Visible = true;
      this.victoryGlow.Visible = true;
    }
  }
}
