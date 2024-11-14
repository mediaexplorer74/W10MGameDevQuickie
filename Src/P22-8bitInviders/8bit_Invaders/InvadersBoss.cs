// Decompiled with JetBrains decompiler
// Type: GameManager.InvadersBoss
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameManager
{
  public class InvadersBoss : SceneNode
  {
    public bool returnToPool;
    private Sprite boss;
    private Sprite shield;
    private float flashTimer;
    private float genericTimer;
    private float mover;
    private float shotTimer;
    private float shotThreshold;
    private float missleTimer;
    private float missleThreshold;
    private int mode;
    private int Health;

    public InvadersBoss(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.boss = new Sprite("boss", Vector2.Zero, "sheets/invaders", this.Screen);
      this.Add((SceneNode) this.boss);
      this.shield = new Sprite("forcefield", Vector2.Zero, "sheets/invaders", this.Screen);
      this.Add((SceneNode) this.shield);
      this.SetSourceRect();
    }

    public void InitFromPool()
    {
      this.Health = 30;
      this.returnToPool = false;
      this.shield.Visible = true;
      this.boss.Position = new Vector3(240f, 20f, 0.0f);
      this.mode = 0;
      this.genericTimer = 0.0f;
      this.shotThreshold = 0.0f;
      this.shotTimer = 0.0f;
      this.missleThreshold = 0.0f;
      this.missleTimer = 0.0f;
      this.mover = 0.0f;
    }

    public bool HitCheck(Vector2 where)
    {
      if (this.Health <= 0)
        return false;
      if (this.shield.Visible)
      {
        if (!this.shield.Bounds.Contains(where))
          return false;
        ((InvadersScreen) this.Screen).fxAddLayer.AddSpark(where);
        return true;
      }
      bool flag = this.boss.Bounds.Contains(where);
      if (this.returnToPool || !flag)
        return false;
      --this.Health;
      if ((double) this.flashTimer > 0.10000000149011612)
        this.boss.SetTextureSource("bossflash");
      this.flashTimer = 0.0f;
      if (this.Health <= 0)
      {
        for (int index = 0; index < 5; ++index)
        {
          Vector2 vector2 = Tools.RandomV3(-60f, 60f).ToVector2();
          ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(Tools.RandomInt(1, 3), where + vector2);
        }
        this.mode = 3;
      }
      ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(Tools.RandomInt(1, 3), where);
      return true;
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      switch (this.mode)
      {
        case 0:
          this.BossLower(totalSeconds);
          break;
        case 1:
          this.BossAttack(totalSeconds);
          break;
        case 2:
          this.BossVulnerable(totalSeconds);
          break;
        case 3:
          this.BossOver(totalSeconds);
          break;
      }
      this.shield.Position = this.boss.Position;
      this.shield.Rotation = Tools.RandomFloat(0.0f, 360f);
      this.shield.Opacity = Tools.SineAnimation(gameTime, 10f, 0.5f, 1f);
      this.flashTimer += totalSeconds;
      if ((double) this.flashTimer > 0.05000000074505806)
        this.boss.SetTextureSource("boss");
      base.Update(gameTime, ref worldTransform);
    }

    private void ShootUpdate(float GT)
    {
      if ((double) this.shotTimer > (double) this.shotThreshold)
      {
        this.shotTimer = 0.0f;
        this.shotThreshold = this.Health < 15 ? Tools.RandomFloat(1f, 3f) : Tools.RandomFloat(2f, 4f);
        ((InvadersScreen) this.Screen).fxNode.EnemyShootBullet(new Vector2(this.boss.Position.X - 42f, 210f), new Vector2(0.0f, 1f));
        ((InvadersScreen) this.Screen).fxNode.EnemyShootBullet(new Vector2(this.boss.Position.X + 42f, 210f), new Vector2(0.0f, 1f));
      }
      if (this.Health >= 20)
        return;
      this.missleTimer += GT;
      if ((double) this.missleTimer <= (double) this.missleThreshold)
        return;
      this.missleTimer = 0.0f;
      this.missleThreshold = this.Health < 15 ? 1.5f : Tools.RandomFloat(2f, 4f);
      ((InvadersScreen) this.Screen).fxNode.EnemyShootMissle(this.boss.Position.ToVector2());
    }

    private void BossLower(float GT)
    {
      this.boss.Position.Y += GT * 70f;
      if ((double) this.boss.Position.Y < 161.0)
        return;
      this.boss.Position.Y = 161f;
      this.mode = 1;
    }

    private void BossAttack(float GT)
    {
      this.shield.Visible = true;
      this.shotTimer += GT;
      this.genericTimer += GT;
      if ((double) this.genericTimer > 8.0)
      {
        this.genericTimer = 0.0f;
        this.mode = 2;
      }
      if (!((InvadersScreen) this.Screen).critterFactory.stopTime)
      {
        this.boss.Position.X = Tools.SineAnimation(60f, 420f, (float) Math.Sin((double) this.mover * (double) GT * 15.0));
        this.mover += GT;
      }
      this.ShootUpdate(GT);
    }

    private void BossVulnerable(float GT)
    {
      this.shield.Visible = false;
      this.genericTimer += GT;
      if ((double) this.genericTimer > 3.0)
      {
        this.genericTimer = 0.0f;
        this.mode = 1;
      }
      if (!((InvadersScreen) this.Screen).critterFactory.stopTime)
      {
        this.boss.Position.X = Tools.SineAnimation(60f, 420f, (float) Math.Sin((double) this.mover * (double) GT * 15.0));
        this.mover += GT;
      }
      this.ShootUpdate(GT);
    }

    private void BossOver(float GT)
    {
      if (Tools.RandomInt(100) < 10)
      {
        Vector2 vector2 = Tools.RandomV3(-60f, 60f).ToVector2();
        ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(Tools.RandomInt(1, 3), this.boss.Position.ToVector2() + vector2);
      }
      this.boss.Position.Y -= GT * 30f;
      if ((double) this.boss.Position.Y > 20.0)
        return;
      MusicManager.Play("music/gameplay", true);
      ((InvadersScreen) this.Screen).uiNode.AddScore(10000);
      ((InvadersScreen) this.Screen).NotifyGameOver(true);
      this.returnToPool = true;
    }
  }
}
