// Decompiled with JetBrains decompiler
// Type: GameManager.PlayLayer
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameManager
{
  public class PlayLayer : Layer2D
  {
    public CelSprite heroSprite;
    private float thrustPower;
    private float heroShootTimer;
    private float scaleBounce;
    private float invTimer;
    private bool doHit;
    private Sprite heroShield;
    private Sprite shieldHit;
    private int heroHealth = 3;
    private bool canMove = true;
    private bool gameOver;
    private ObjectPool<InvaderHeroBullet> heroBulletPool;
    private AsteroidGenerator astGenerator;
    private SputnikGenerator sputnikGenerator;

    public PlayLayer(Camera2D cam, GameScreen screen)
      : base(cam, screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      string imageName = "sheets/asteroids";
      this.heroSprite = new CelSprite("icon", new Vector2(2500f, 2500f), new Point(60, 38), imageName, this.Screen);
      this.heroSprite.AddClip("hero", 30, 0, 115, 19, CelAnimType.Bounce);
      this.heroSprite.Play();
      this.Add((SceneNode) this.heroSprite);
      this.heroShield = new Sprite("heroshield", new Vector2(2500f, 2500f), imageName, this.Screen);
      this.heroShield.SetSourceRect();
      this.Add((SceneNode) this.heroShield);
      this.shieldHit = new Sprite("shieldhit", new Vector2(2500f, 2500f), imageName, this.Screen);
      this.shieldHit.Visible = false;
      this.heroShield.color = Color.Lime;
      this.shieldHit.SetSourceRect();
      this.Add((SceneNode) this.shieldHit);
      this.heroBulletPool = new ObjectPool<InvaderHeroBullet>(30);
      this.astGenerator = new AsteroidGenerator(this.Screen);
      this.Add((SceneNode) this.astGenerator);
      this.sputnikGenerator = new SputnikGenerator(this.Screen);
      this.Add((SceneNode) this.sputnikGenerator);
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      if (this.gameOver)
        return;
      this.thrustPower = ((AsteroidScreen) this.Screen).uiNode.moveStick.ThumbstickValue.Length() * 60f;
      ((AsteroidScreen) this.Screen).additiveLayer.SetThrustPos(this.heroSprite.Position, this.heroSprite.Rotation, this.thrustPower);
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      if (this.canMove)
      {
        CelSprite heroSprite = this.heroSprite;
        heroSprite.Position = heroSprite.Position + ((AsteroidScreen) this.Screen).uiNode.stickValue.ToVector3() * totalSeconds * 200f;
      }
      this.heroSprite.Position.X = MathHelper.Clamp(this.heroSprite.Position.X, 200f, 4800f);
      this.heroSprite.Position.Y = MathHelper.Clamp(this.heroSprite.Position.Y, 200f, 4800f);
      this.heroShootTimer = Math.Max(0.0f, this.heroShootTimer - totalSeconds);
      this.heroShield.Position = this.heroSprite.Position;
      this.heroShield.Rotation += totalSeconds * 50f;
      this.heroShield.Scale = Tools.SineAnimation(gameTime, 2f, 0.8f, 1.2f);
      this.shieldHit.Position = this.heroSprite.Position;
      this.UpdateBullets(gameTime);
      this.UpdateHeroStuff(gameTime);
      if (this.astGenerator.AsteroidHeroCheck(this.heroSprite.Position.ToVector2()) && (double) this.invTimer == 0.0)
      {
        ((AsteroidScreen) this.Screen).additiveLayer.DoExplosion(this.heroSprite.Position.ToVector2());
        ((AsteroidScreen) this.Screen).additiveLayer.DoBlastRing(this.heroSprite.Position.ToVector2());
        this.invTimer = 2f;
        this.doHit = true;
        this.scaleBounce = 0.0f;
        this.heroHealth = Math.Max(0, this.heroHealth - 1);
      }
      float closestSputnik = this.sputnikGenerator.GetClosestSputnik(this.heroSprite.Position);
      if (this.canMove)
      {
        if ((double) closestSputnik >= 150.0 && (double) closestSputnik <= 250.0)
          ((AsteroidScreen) this.Screen).uiNode.ActivateTractorButton();
        else
          ((AsteroidScreen) this.Screen).uiNode.DisableTractorButton();
      }
      base.Update(gameTime, ref worldTransform);
    }

    private void UpdateHeroStuff(GameTime gameTime)
    {
      this.heroSprite.Rotation = ((AsteroidScreen) this.Screen).uiNode.rotationFromStick;
      if (this.doHit)
      {
        this.shieldHit.Visible = true;
        this.shieldHit.Opacity = Tools.Bounce(this.scaleBounce, 1f);
        this.shieldHit.Rotation = (float) Tools.RandomInt(360);
        this.scaleBounce += (float) gameTime.ElapsedGameTime.TotalSeconds;
        if ((double) this.scaleBounce >= 1.0)
        {
          this.doHit = false;
          this.shieldHit.Visible = false;
        }
      }
      if (this.heroHealth == 3)
        this.heroShield.color = Color.Lime;
      else if (this.heroHealth == 2)
        this.heroShield.color = Color.Yellow;
      else if (this.heroHealth == 1)
        this.heroShield.color = Color.Red;
      else if (this.heroHealth == 0)
      {
        ((AsteroidScreen) this.Screen).uiNode.RemoveLife();
        this.heroHealth = 3;
      }
      this.heroSprite.Opacity = (double) this.invTimer <= 0.0 ? 1f : ((double) this.heroSprite.Opacity == 1.0 ? 0.0f : 1f);
      this.invTimer = Math.Max(0.0f, this.invTimer - (float) gameTime.ElapsedGameTime.TotalSeconds);
    }

    private void UpdateBullets(GameTime gameTime)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      foreach (ObjectPool<InvaderHeroBullet>.Node activeNode in this.heroBulletPool.ActiveNodes)
      {
        activeNode.Item.Position += totalSeconds * 600f * activeNode.Item.Direction;
        activeNode.Item.Life += totalSeconds;
        if (this.astGenerator.AsteroidBulletCheck(activeNode.Item.Position))
          this.heroBulletPool.Return(activeNode);
        if ((double) activeNode.Item.Life > 0.800000011920929)
          this.heroBulletPool.Return(activeNode);
      }
    }

    public void HeroShoot(Vector2 direction)
    {
      if ((double) this.heroShootTimer != 0.0 || this.heroBulletPool.AvailableCount <= 0)
        return;
      this.Screen.audioManager.Play("heroshoot");
      this.heroShootTimer = 0.15f;
      direction.Normalize();
      this.heroBulletPool.Get().Item.InitFromPool(this.heroSprite.Position.ToVector2(), direction);
    }

    public void ActivateTractor()
    {
      this.canMove = false;
      ((AsteroidScreen) this.Screen).additiveLayer.ActivateTractor(this.sputnikGenerator.ActivateTractor(this.heroSprite.Position));
    }

    public void DisableTractor()
    {
      this.canMove = true;
      this.sputnikGenerator.DisableTractor();
    }

    public void StopPlay()
    {
      this.canMove = false;
      this.gameOver = true;
    }

    public override void Draw()
    {
      this.Screen.spriteBatch.Begin(this.sortMode, this.blendState, this.samplerState, (DepthStencilState) null, (RasterizerState) null, (Effect) null, this.Camera.LocalTransform);
      foreach (InvaderHeroBullet invaderHeroBullet in this.heroBulletPool)
        this.Screen.spriteBatch.Draw(this.Screen.textureManager.Load("sheets/asteroids"), invaderHeroBullet.Position, new Rectangle?(this.Screen.GetSpriteSource("herobullet")), Color.White, invaderHeroBullet.Rotation, new Vector2(4f, 14f), 1f, SpriteEffects.None, 0.0f);
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.Draw();
      this.Screen.spriteBatch.End();
    }
  }
}
