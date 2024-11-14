// Decompiled with JetBrains decompiler
// Type: GameManager.InvadersFX
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace GameManager
{
  public class InvadersFX : SceneNode
  {
    private ObjectPool<InvaderHeroBullet> heroBulletPool;
    private ObjectPool<InvaderEnemyBullet> enemyBulletPool;
    private AdvancedObjectPool<InvaderMissle> misslePool;
    public bool isHeroShooting;
    private float heroShootTimer;
    private float ROFTimer;
    private float shotTime;
    private Rectangle screenBounds = new Rectangle(0, 0, 480, 800);
    private InvaderBlock[] blocks;
    private ParticleEmitter2D rockEmitter;
    private Rectangle[] noShootZone;

    public InvadersFX(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.heroBulletPool = new ObjectPool<InvaderHeroBullet>(30);
      this.enemyBulletPool = new ObjectPool<InvaderEnemyBullet>(20);
      this.misslePool = new AdvancedObjectPool<InvaderMissle>(3);
      for (int index = 0; index < 3; ++index)
        this.misslePool.AddToPool(new InvaderMissle(this.Screen));
      this.blocks = new InvaderBlock[3];
      float x = 90f;
      for (int index = 0; index < 3; ++index)
      {
        this.blocks[index] = new InvaderBlock(new Vector2(x, 650f), this.Screen);
        this.Add((SceneNode) this.blocks[index]);
        x += 150f;
      }
      this.noShootZone = new Rectangle[3];
      this.noShootZone[0] = new Rectangle(50, 740, 80, 60);
      this.noShootZone[1] = new Rectangle(200, 740, 80, 60);
      this.noShootZone[2] = new Rectangle(350, 740, 80, 60);
      this.rockEmitter = new ParticleEmitter2D("rock1", 15, 5, "sheets/invaders", this.Screen);
      this.rockEmitter.ApplyTemplate(new ParticleEmitterTemplate()
      {
        Gravity = 400f,
        Directional = true,
        Spread = 90f,
        decayFactor = 0.2f,
        Scale = 0.0f,
        scaleMax = 0.4f,
        scaleMin = 0.2f,
        rotateMax = 360f,
        RotationSpeed = 150f,
        velocityMax = 250,
        velocityMin = 100
      });
      this.rockEmitter.SetSourceRect();
      this.Add((SceneNode) this.rockEmitter);
    }

    public void Reset()
    {
      for (int index = 0; index < 3; ++index)
        this.blocks[index].Rebuild();
      this.ROFTimer = 0.0f;
      this.shotTime = 0.2f;
    }

    public bool CheckCritterBlockHit(Rectangle where)
    {
      for (int index = 0; index < 3; ++index)
      {
        Vector2 position;
        if (this.blocks[index].BlockHitCheck(where, out position))
        {
          this.AddExplosion(position);
          ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(1, position);
          break;
        }
      }
      return false;
    }

    public void EnemyShootBullet(Vector2 where, Vector2 direction)
    {
      int num = (int) Tools.RemapValue((float) Globals.saveManager.saveState.Level, 1f, 10f, 5f, 10f);
      if (this.enemyBulletPool.AvailableCount <= 0 || this.enemyBulletPool.ActiveCount >= num)
        return;
      this.Screen.audioManager.Play("iontachyon");
      this.enemyBulletPool.Get().Item.InitFromPool(where, direction);
    }

    public void EnemyShootMissle(Vector2 where)
    {
      int num = (int) Tools.RemapValue((float) Globals.saveManager.saveState.Level, 1f, 10f, 1f, 5f);
      if (Globals.saveManager.saveState.Level == 1)
        num = 0;
      if (this.misslePool.AvailableCount <= 0 || this.misslePool.ActiveCount >= num)
        return;
      this.Screen.audioManager.Play("eblaster");
      this.misslePool.Get().Item.InitFromPool(where);
    }

    public void SputnikBurst(Vector2 where)
    {
      int num = Tools.RandomInt(100);
      if (num < 25)
      {
        for (int index = 0; index < 20; ++index)
        {
          if (this.heroBulletPool.AvailableCount > 0)
          {
            Vector2 vector = Tools.AngleToVector(MathHelper.ToRadians((float) (index * 18)));
            this.heroBulletPool.Get().Item.InitFromPool(where, vector);
          }
        }
      }
      else if (num >= 25 && num < 50)
      {
        ((InvadersScreen) this.Screen).critterFactory.SputnikExplosion(where);
        ((InvadersScreen) this.Screen).fxAddLayer.AddBlastRing(where);
      }
      else if (num >= 50 && num < 75)
      {
        this.Screen.audioManager.Play("stoptime");
        ((InvadersScreen) this.Screen).critterFactory.StopTime();
        ((InvadersScreen) this.Screen).uiNode.NotifyStopTime();
      }
      else
      {
        this.Screen.audioManager.Play("stoptime");
        this.shotTime = 0.1f;
        this.ROFTimer = 10f;
      }
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.heroShootTimer = Math.Max(0.0f, this.heroShootTimer - totalSeconds);
      this.ROFTimer = Math.Max(0.0f, this.ROFTimer - totalSeconds);
      if ((double) this.ROFTimer == 0.0)
        this.shotTime = 0.2f;
      if (this.isHeroShooting && ((InvadersScreen) this.Screen).hero.isAlive && (double) this.heroShootTimer == 0.0 && this.heroBulletPool.AvailableCount > 0)
      {
        this.Screen.audioManager.Play("heroshoot");
        this.heroShootTimer = this.shotTime;
        this.heroBulletPool.Get().Item.InitFromPool(((InvadersScreen) this.Screen).hero.hero.Position.ToVector2() + new Vector2(0.0f, -14f));
      }
      foreach (ObjectPool<InvaderHeroBullet>.Node activeNode in this.heroBulletPool.ActiveNodes)
      {
        activeNode.Item.Position += totalSeconds * 600f * activeNode.Item.Direction;
        if (((InvadersScreen) this.Screen).critterFactory.BulletCheck(activeNode.Item.Position))
        {
          this.heroBulletPool.Return(activeNode);
          ((InvadersScreen) this.Screen).fxAddLayer.AddSpark(activeNode.Item.Position);
        }
        if (!this.screenBounds.Contains(activeNode.Item.Position))
          this.heroBulletPool.Return(activeNode);
      }
      foreach (ObjectPool<InvaderEnemyBullet>.Node activeNode in this.enemyBulletPool.ActiveNodes)
      {
        activeNode.Item.Position += totalSeconds * 300f * activeNode.Item.Direction;
        if (!((InvadersScreen) this.Screen).hero.isFlashing && ((InvadersScreen) this.Screen).hero.isAlive && ((InvadersScreen) this.Screen).hero.hero.Bounds.Contains(activeNode.Item.Position))
        {
          this.enemyBulletPool.Return(activeNode);
          ((InvadersScreen) this.Screen).hero.Kill();
          this.Screen.audioManager.Play("explosion2");
          ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(2, ((InvadersScreen) this.Screen).hero.hero.Position.ToVector2());
        }
        for (int index = 0; index < 3; ++index)
        {
          if (this.blocks[index].HitCheck(activeNode.Item.Position))
          {
            this.enemyBulletPool.Return(activeNode);
            this.AddExplosion(activeNode.Item.Position);
            break;
          }
        }
        if (!this.screenBounds.Contains(activeNode.Item.Position))
          this.enemyBulletPool.Return(activeNode);
      }
      foreach (AdvancedObjectPool<InvaderMissle>.Node activeNode in this.misslePool.ActiveNodes)
      {
        activeNode.Item.Update(gameTime, ref worldTransform);
        if (!((InvadersScreen) this.Screen).hero.isFlashing && ((InvadersScreen) this.Screen).hero.isAlive && ((InvadersScreen) this.Screen).hero.hero.Bounds.Contains(activeNode.Item.missle.Position.ToVector2()))
        {
          this.misslePool.Return(activeNode);
          ((InvadersScreen) this.Screen).hero.Kill();
          this.Screen.audioManager.Play("explosion2");
          ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(2, ((InvadersScreen) this.Screen).hero.hero.Position.ToVector2());
        }
        for (int index = 0; index < 3; ++index)
        {
          if (this.blocks[index].HitCheck(activeNode.Item.missle.Position.ToVector2()))
          {
            this.misslePool.Return(activeNode);
            this.AddExplosion(activeNode.Item.missle.Position.ToVector2());
            ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(2, activeNode.Item.missle.Position.ToVector2());
            this.Screen.audioManager.Play("explosion2");
            break;
          }
        }
        if (!this.screenBounds.Contains(activeNode.Item.missle.Position.ToVector2()))
          this.misslePool.Return(activeNode);
      }
      base.Update(gameTime, ref worldTransform);
    }

    private void AddExplosion(Vector2 where)
    {
      this.rockEmitter.Fire(where);
      ((InvadersScreen) this.Screen).fxAddLayer.expEmitter.Fire(where);
    }

    public override void Draw()
    {
      foreach (InvaderHeroBullet invaderHeroBullet in this.heroBulletPool)
        this.Screen.spriteBatch.Draw(this.Screen.textureManager.Load("sheets/invaders"), invaderHeroBullet.Position, new Rectangle?(this.Screen.GetSpriteSource("herobullet")), Color.White, invaderHeroBullet.Rotation, new Vector2(4f, 14f), 1f, SpriteEffects.None, 0.0f);
      foreach (InvaderEnemyBullet invaderEnemyBullet in this.enemyBulletPool)
        this.Screen.spriteBatch.Draw(this.Screen.textureManager.Load("sheets/invaders"), invaderEnemyBullet.Position, new Rectangle?(this.Screen.GetSpriteSource("enemybullet")), Color.White, invaderEnemyBullet.Rotation, new Vector2(4f, 14f), 1f, SpriteEffects.None, 0.0f);
      foreach (SceneNode sceneNode in this.misslePool)
        sceneNode.Draw();
      base.Draw();
    }
  }
}
