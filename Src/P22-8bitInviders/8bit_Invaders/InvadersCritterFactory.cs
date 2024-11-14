// Decompiled with JetBrains decompiler
// Type: GameManager.InvadersCritterFactory
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameManager
{
  public class InvadersCritterFactory : SceneNode
  {
    private AdvancedObjectPool<InvadersCritterBase>[] critterPoolArray;
    private AdvancedObjectPool<InvaderMeteorite> meteorPool;
    private AdvancedObjectPool<InvadersSputnik> sputnikPool;
    private AdvancedObjectPool<InvadersMothership> motherPool;
    private AdvancedObjectPool<InvadersBoss> bossPool;
    private Vector2 rootPosition;
    private float rightExtent;
    private float leftExtent;
    private float baseSpeed = 1f;
    private float speedModifier = 1f;
    private float dir = 1f;
    private InvadersCritterFactory.RootMode rootMode;
    private int rootRow;
    private float meteorTimer;
    private float meteorThreshold;
    private int maxMeteors;
    private float sputnikTimer;
    private float sputnikThreshold;
    private float motherShipTimer;
    private float motherShipThreshold;
    public bool stopTime;
    private float stopTimer;
    public bool startGame;

    public InvadersCritterFactory(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.critterPoolArray = new AdvancedObjectPool<InvadersCritterBase>[4];
      for (int index = 0; index < this.critterPoolArray.Length; ++index)
        this.critterPoolArray[index] = new AdvancedObjectPool<InvadersCritterBase>(22);
      for (int index = 0; index < 22; ++index)
      {
        this.critterPoolArray[0].AddToPool((InvadersCritterBase) new InvadersGrunt(this.Screen));
        this.critterPoolArray[1].AddToPool((InvadersCritterBase) new InvadersBomber(this.Screen));
        this.critterPoolArray[2].AddToPool((InvadersCritterBase) new InvadersFlyer(this.Screen));
        this.critterPoolArray[3].AddToPool((InvadersCritterBase) new InvadersTough(this.Screen));
      }
      this.meteorPool = new AdvancedObjectPool<InvaderMeteorite>(8);
      for (int index = 0; index < 8; ++index)
        this.meteorPool.AddToPool(new InvaderMeteorite(this.Screen));
      this.sputnikPool = new AdvancedObjectPool<InvadersSputnik>(1);
      this.sputnikPool.AddToPool(new InvadersSputnik(this.Screen));
      this.motherPool = new AdvancedObjectPool<InvadersMothership>(1);
      this.motherPool.AddToPool(new InvadersMothership(this.Screen));
      this.bossPool = new AdvancedObjectPool<InvadersBoss>(1);
      this.bossPool.AddToPool(new InvadersBoss(this.Screen));
      this.rootPosition = new Vector2(40f, 130f);
      this.rootMode = InvadersCritterFactory.RootMode.Sliding;
      this.rootRow = 0;
    }

    private void SetupEnemyGrid()
    {
      int row1 = 0;
      Color color = new Color(Tools.RandomV3(0.5f, 1f));
      for (int column = 0; column < 11; ++column)
      {
        if (this.critterPoolArray[0].AvailableCount > 0)
          this.critterPoolArray[0].Get().Item.InitFromPool(row1, column, color);
      }
      color = new Color(Tools.RandomV3(0.5f, 1f));
      int row2 = row1 + 1;
      for (int column = 0; column < 11; ++column)
      {
        if (this.critterPoolArray[1].AvailableCount > 0)
          this.critterPoolArray[1].Get().Item.InitFromPool(row2, column, color);
      }
      color = new Color(Tools.RandomV3(0.5f, 1f));
      int row3 = row2 + 1;
      for (int column = 0; column < 11; ++column)
      {
        if (this.critterPoolArray[2].AvailableCount > 0)
          this.critterPoolArray[2].Get().Item.InitFromPool(row3, column, color);
      }
      color = new Color(Tools.RandomV3(0.5f, 1f));
      int row4 = row3 + 1;
      for (int column = 0; column < 11; ++column)
      {
        if (this.critterPoolArray[3].AvailableCount > 0)
          this.critterPoolArray[3].Get().Item.InitFromPool(row4, column, color);
      }
      color = new Color(Tools.RandomV3(0.5f, 1f));
      int row5 = row4 + 1;
      for (int column = 0; column < 11; ++column)
      {
        if (this.critterPoolArray[0].AvailableCount > 0)
          this.critterPoolArray[0].Get().Item.InitFromPool(row5, column, color);
      }
    }

    public bool BulletCheck(Vector2 where)
    {
      for (int index = 0; index < this.critterPoolArray.Length; ++index)
      {
        foreach (InvadersCritterBase invadersCritterBase in this.critterPoolArray[index])
        {
          if (invadersCritterBase.HitCheck(where))
            return true;
        }
      }
      foreach (InvaderMeteorite invaderMeteorite in this.meteorPool)
      {
        if (invaderMeteorite.HitCheck(where))
          return true;
      }
      foreach (InvadersSputnik invadersSputnik in this.sputnikPool)
      {
        if (invadersSputnik.HitCheck(where))
          return true;
      }
      foreach (InvadersMothership invadersMothership in this.motherPool)
      {
        if (invadersMothership.HitCheck(where))
          return true;
      }
      foreach (InvadersBoss invadersBoss in this.bossPool)
      {
        if (invadersBoss.HitCheck(where))
          return true;
      }
      return false;
    }

    public void Reset(int level)
    {
      this.baseSpeed = Math.Min((float) (1.0 + 0.079999998211860657 * (double) level), 2.3f);
      this.speedModifier = 1f;
      for (int index = 0; index < this.critterPoolArray.Length; ++index)
      {
        foreach (AdvancedObjectPool<InvadersCritterBase>.Node activeNode in this.critterPoolArray[index].ActiveNodes)
          this.critterPoolArray[index].Return(activeNode);
      }
      foreach (AdvancedObjectPool<InvadersBoss>.Node activeNode in this.bossPool.ActiveNodes)
        this.bossPool.Return(activeNode);
      this.rootRow = 0;
      this.rootPosition = new Vector2(40f, 130f);
      switch (level % 10)
      {
        case 1:
        case 2:
        case 5:
        case 6:
        case 8:
        case 9:
          this.SetupEnemyGrid();
          break;
      }
      this.stopTime = false;
      this.meteorTimer = 0.0f;
      this.meteorThreshold = Tools.RemapValue((float) level, 1f, 15f, 30f, 5f);
      this.maxMeteors = (int) Tools.RemapValue((float) level, 1f, 15f, 1f, 8f);
      this.sputnikThreshold = Tools.RemapValue((float) level, 1f, 20f, 30f, 8f);
      this.motherShipThreshold = 30f;
    }

    public void StopTime()
    {
      this.stopTime = true;
      this.stopTimer = 5f;
    }

    public void SpawnBoss()
    {
      if (this.bossPool.AvailableCount <= 0)
        return;
      MusicManager.Play("music/boss", true);
      this.bossPool.Get().Item.InitFromPool();
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      if (!this.startGame || ((InvadersScreen) this.Screen).gameOver)
        return;
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.stopTimer = Math.Max(0.0f, this.stopTimer - totalSeconds);
      if ((double) this.stopTimer == 0.0)
        this.stopTime = false;
      this.UpdateSputnik(gameTime, ref worldTransform);
      this.UpdateMeteors(totalSeconds);
      this.UpdateCritterArray(gameTime, ref worldTransform);
      this.UpdateMotherShip(gameTime, ref worldTransform);
      this.UpdateBoss(gameTime, ref worldTransform);
      base.Update(gameTime, ref worldTransform);
    }

    private void UpdateBoss(GameTime gameTime, ref Matrix worldTransform)
    {
      double totalSeconds = gameTime.ElapsedGameTime.TotalSeconds;
      foreach (AdvancedObjectPool<InvadersBoss>.Node activeNode in this.bossPool.ActiveNodes)
      {
        activeNode.Item.Update(gameTime, ref worldTransform);
        if (activeNode.Item.returnToPool)
          this.bossPool.Return(activeNode);
      }
    }

    private void UpdateMotherShip(GameTime gameTime, ref Matrix worldTransform)
    {
      this.motherShipTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if (this.CrittersInPlay() >= 8 && (double) this.motherShipTimer > (double) this.motherShipThreshold && this.motherPool.AvailableCount > 0)
      {
        this.Screen.audioManager.Play("mothership");
        this.motherShipTimer = 0.0f;
        this.motherShipThreshold = Tools.RandomFloat(15f, 30f);
        this.motherPool.Get().Item.InitFromPool();
      }
      foreach (AdvancedObjectPool<InvadersMothership>.Node activeNode in this.motherPool.ActiveNodes)
      {
        activeNode.Item.Update(gameTime, ref worldTransform);
        if (activeNode.Item.returnToPool)
          this.motherPool.Return(activeNode);
      }
    }

    private void UpdateSputnik(GameTime gameTime, ref Matrix worldTransform)
    {
      this.sputnikTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.sputnikTimer > (double) this.sputnikThreshold && this.sputnikPool.AvailableCount > 0)
      {
        this.Screen.audioManager.Play("sputnik");
        this.sputnikTimer = 0.0f;
        this.sputnikPool.Get().Item.InitFromPool();
      }
      foreach (AdvancedObjectPool<InvadersSputnik>.Node activeNode in this.sputnikPool.ActiveNodes)
      {
        activeNode.Item.Update(gameTime, ref worldTransform);
        if (activeNode.Item.returnToPool)
          this.sputnikPool.Return(activeNode);
      }
    }

    private void UpdateMeteors(float GT)
    {
      this.meteorTimer += GT;
      if ((double) this.meteorTimer > (double) this.meteorThreshold && this.meteorPool.ActiveCount < this.maxMeteors)
      {
        this.meteorTimer = 0.0f;
        for (int index = 0; index < this.maxMeteors; ++index)
        {
          if (Tools.RandomBool() && this.meteorPool.AvailableCount > 0)
            this.meteorPool.Get().Item.InitFromPool();
        }
      }
      foreach (AdvancedObjectPool<InvaderMeteorite>.Node activeNode in this.meteorPool.ActiveNodes)
      {
        activeNode.Item.meteor.Position.Y += GT * activeNode.Item.speed;
        activeNode.Item.meteor.Rotation += GT * 100f;
        if ((double) activeNode.Item.meteor.Position.Y > 850.0 || activeNode.Item.returnToPool)
          this.meteorPool.Return(activeNode);
        if ((double) Vector3.Distance(activeNode.Item.meteor.Position, ((InvadersScreen) this.Screen).hero.hero.Position) < 40.0)
        {
          ((InvadersScreen) this.Screen).hero.Kill();
          this.Screen.audioManager.Play("explosion2");
          ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(2, activeNode.Item.meteor.Position.ToVector2());
          this.meteorPool.Return(activeNode);
        }
      }
    }

    private void UpdateCritterArray(GameTime gameTime, ref Matrix worldTransform)
    {
      switch (Globals.saveManager.saveState.Level % 10)
      {
        case 1:
        case 2:
        case 5:
        case 6:
        case 8:
        case 9:
          float num1 = (float) gameTime.ElapsedGameTime.TotalSeconds;
          if (this.stopTime)
            num1 = 0.0f;
          this.rightExtent = this.GetRightExtent();
          this.leftExtent = this.GetLeftExtent();
          if (this.rootMode == InvadersCritterFactory.RootMode.Sliding)
          {
            float num2 = (float) ((double) this.dir * (double) num1 * 6.0) * this.baseSpeed * this.speedModifier;
            this.rootPosition.X += num2;
            if ((double) this.rightExtent > 480.0)
            {
              this.rootPosition.X -= num2 * 2f;
              this.dir = -1f;
              this.speedModifier += 0.25f;
              ++this.rootRow;
              this.rootMode = InvadersCritterFactory.RootMode.Dropping;
            }
            else if ((double) this.leftExtent < 0.0)
            {
              this.rootPosition.X += (float) (-(double) num2 * 2.0);
              this.dir = 1f;
              this.speedModifier += 0.25f;
              ++this.rootRow;
              this.rootMode = InvadersCritterFactory.RootMode.Dropping;
            }
          }
          else if (this.rootMode == InvadersCritterFactory.RootMode.Dropping)
          {
            this.rootPosition.Y += num1 * 50f * this.baseSpeed;
            if ((double) this.rootPosition.Y >= (double) (130 + this.rootRow * 20))
            {
              this.rootPosition.Y = (float) (130 + this.rootRow * 20);
              this.rootMode = InvadersCritterFactory.RootMode.Sliding;
            }
          }
          for (int index = 0; index < this.critterPoolArray.Length; ++index)
          {
            foreach (AdvancedObjectPool<InvadersCritterBase>.Node activeNode in this.critterPoolArray[index].ActiveNodes)
            {
              activeNode.Item.Update(this.rootPosition, gameTime, ref worldTransform);
              if ((double) activeNode.Item.ship.Position.Y > 800.0)
                ((InvadersScreen) this.Screen).NotifyGameOver(false);
              if (((InvadersScreen) this.Screen).fxNode.CheckCritterBlockHit(activeNode.Item.ship.Bounds))
              {
                activeNode.Item.returnToPool = true;
                ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(2, activeNode.Item.ship.Position.ToVector2());
                this.Screen.audioManager.Play("explosion2");
              }
              if (!((InvadersScreen) this.Screen).hero.isFlashing && ((InvadersScreen) this.Screen).hero.isAlive && (double) Vector3.Distance(activeNode.Item.ship.Position, ((InvadersScreen) this.Screen).hero.hero.Position) < 40.0)
              {
                activeNode.Item.returnToPool = true;
                ((InvadersScreen) this.Screen).hero.Kill();
                this.Screen.audioManager.Play("explosion2");
                ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(1, activeNode.Item.ship.Position.ToVector2());
                ((InvadersScreen) this.Screen).fxAddLayer.AddExplosion(2, ((InvadersScreen) this.Screen).hero.hero.Position.ToVector2());
              }
              if (activeNode.Item.returnToPool)
              {
                ((InvadersScreen) this.Screen).uiNode.AddScore(100);
                this.critterPoolArray[index].Return(activeNode);
              }
            }
          }
          if (this.CrittersInPlay() != 0)
            break;
          ((InvadersScreen) this.Screen).NotifyGameOver(true);
          break;
      }
    }

    private float GetLeftExtent()
    {
      float num = 480f;
      for (int index = 0; index < this.critterPoolArray.Length; ++index)
      {
        foreach (InvadersCritterBase invadersCritterBase in this.critterPoolArray[index])
        {
          if ((double) invadersCritterBase.ship.Position.X < (double) num)
            num = invadersCritterBase.ship.Position.X;
        }
      }
      return num - 20f;
    }

    private float GetRightExtent()
    {
      float num = 0.0f;
      for (int index = 0; index < this.critterPoolArray.Length; ++index)
      {
        foreach (InvadersCritterBase invadersCritterBase in this.critterPoolArray[index])
        {
          if ((double) invadersCritterBase.ship.Position.X > (double) num)
            num = invadersCritterBase.ship.Position.X;
        }
      }
      return num + 20f;
    }

    private int CrittersInPlay()
    {
      int num = 0;
      for (int index = 0; index < this.critterPoolArray.Length; ++index)
        num += this.critterPoolArray[index].ActiveCount;
      return num;
    }

    public void SputnikExplosion(Vector2 where)
    {
      for (int index = 0; index < this.critterPoolArray.Length; ++index)
      {
        foreach (InvadersCritterBase invadersCritterBase in this.critterPoolArray[index])
        {
          if ((double) Vector2.Distance(where, invadersCritterBase.ship.Position.ToVector2()) <= 150.0)
          {
            invadersCritterBase.returnToPool = true;
            this.Screen.audioManager.Play("explosion");
            ((InvadersScreen) this.Screen).fxAddLayer.AddExplosionRandom(invadersCritterBase.ship.Position.ToVector2());
          }
        }
      }
    }

    public override void Draw()
    {
      base.Draw();
      for (int index = 0; index < this.critterPoolArray.Length; ++index)
      {
        foreach (SceneNode sceneNode in this.critterPoolArray[index])
          sceneNode.Draw();
      }
      foreach (SceneNode sceneNode in this.meteorPool)
        sceneNode.Draw();
      foreach (SceneNode sceneNode in this.sputnikPool)
        sceneNode.Draw();
      foreach (SceneNode sceneNode in this.motherPool)
        sceneNode.Draw();
      foreach (SceneNode sceneNode in this.bossPool)
        sceneNode.Draw();
    }

    private enum RootMode
    {
      Sliding,
      Dropping,
    }
  }
}
