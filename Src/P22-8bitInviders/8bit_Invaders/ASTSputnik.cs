// Decompiled with JetBrains decompiler
// Type: GameManager.ASTSputnik
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameManager
{
  public class ASTSputnik : SceneNode
  {
    public Sprite sputnik;
    public Sprite blip;
    public Sprite arrow;
    public bool returntoPool;
    private bool tractorActive;
    private Vector2 target;
    private float weight;
    private float rotvalue;

    public ASTSputnik(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.blip = new Sprite("blip", Vector2.Zero, "sheets/asteroids", this.Screen);
      this.blip.Scale = 0.5f;
      this.Add((SceneNode) this.blip);
      this.sputnik = new Sprite("sputnikbomb", Vector2.Zero, "sheets/asteroids", this.Screen);
      this.Add((SceneNode) this.sputnik);
      this.arrow = new Sprite("arrow", Vector2.Zero, "sheets/asteroids", this.Screen);
      this.Add((SceneNode) this.arrow);
      this.SetSourceRect();
    }

    public void InitFromPool(Vector2 where)
    {
      this.sputnik.Position = where.ToVector3();
      this.blip.Position = where.ToVector3();
      this.returntoPool = false;
      this.tractorActive = false;
      this.weight = 0.0f;
      this.rotvalue = 50f;
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      this.sputnik.Rotation += (float) gameTime.ElapsedGameTime.TotalSeconds * this.rotvalue;
      this.blip.Scale += (float) (gameTime.ElapsedGameTime.TotalSeconds * 10.0);
      this.blip.Opacity -= (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.blip.Position = this.sputnik.Position;
      if ((double) this.blip.Opacity <= 0.0)
      {
        this.blip.Opacity = 1f;
        this.blip.Scale = 0.5f;
      }
      Vector2 vector2_1 = ((AsteroidScreen) this.Screen).playLayer.heroSprite.Position.ToVector2();
      this.arrow.Opacity = Tools.RemapValue(Vector2.Distance(vector2_1, this.sputnik.Position.ToVector2()), 0.0f, 2000f, 0.0f, 1f);
      this.arrow.Scale = Tools.SineAnimation(gameTime, 20f, 0.95f, 1.05f);
      Vector2 vector2_2 = this.sputnik.Position.ToVector2() - vector2_1;
      vector2_2.Normalize();
      this.arrow.Rotation = MathHelper.ToDegrees(Tools.VectorToAngle(vector2_2));
      this.arrow.Position = ((AsteroidScreen) this.Screen).theCamera.Position.ToVector3() + vector2_2.ToVector3() * 200f;
      if (this.tractorActive)
      {
        this.weight = Math.Min(1f, this.weight + (float) gameTime.ElapsedGameTime.TotalSeconds);
        Vector2 v = this.target - this.sputnik.Position.ToVector2();
        v.Normalize();
        Sprite sputnik = this.sputnik;
        sputnik.Position = sputnik.Position + v.ToVector3() * (float) gameTime.ElapsedGameTime.TotalSeconds * 15f;
        if ((double) Vector2.Distance(this.target, this.sputnik.Position.ToVector2()) < 10.0)
        {
          this.tractorActive = false;
          this.returntoPool = true;
          ((AsteroidScreen) this.Screen).uiNode.AddSputnik();
          ((AsteroidScreen) this.Screen).uiNode.DisableTractorButton();
          ((AsteroidScreen) this.Screen).playLayer.DisableTractor();
          ((AsteroidScreen) this.Screen).additiveLayer.DisableTractor();
        }
      }
      base.Update(gameTime, ref worldTransform);
    }

    public void ActivateTractor()
    {
      this.rotvalue = 150f;
      this.weight = 0.0f;
      this.tractorActive = true;
      this.target = ((AsteroidScreen) this.Screen).playLayer.heroSprite.Position.ToVector2();
    }

    public void DisableTractor()
    {
      this.rotvalue = 50f;
      this.weight = 0.0f;
      this.tractorActive = false;
    }
  }
}
