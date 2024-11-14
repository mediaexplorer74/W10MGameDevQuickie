// Decompiled with JetBrains decompiler
// Type: GameManager.AsteroidGenerator
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public class AsteroidGenerator : SceneNode
  {
    private AdvancedObjectPool<ASTAsteroid> astPool;
    private ParticleEmitter2D astEmitter;
    private ParticleEmitter2D smokeEmitter;
    private ParticleEmitter2D sparkEmitter;
    private ParticleEmitter2D shardEmitter;

    public AsteroidGenerator(GameScreen screen)
      : base(screen)
    {
      this.astPool = new AdvancedObjectPool<ASTAsteroid>(50);
      for (int index = 0; index < 50; ++index)
        this.astPool.AddToPool(new ASTAsteroid(this.Screen));
      Vector2[] vector2Array = new Vector2[4]
      {
        new Vector2(2060f, Tools.RandomFloat(2100f, 2500f)),
        new Vector2(2940f, Tools.RandomFloat(2100f, 2500f)),
        new Vector2(2060f, Tools.RandomFloat(2600f, 2900f)),
        new Vector2(2940f, Tools.RandomFloat(2600f, 2900f))
      };
      Vector2 vector2 = new Vector2(2500f, 2500f);
      for (int index = 0; index < 4; ++index)
      {
        Vector2 direction = vector2 - vector2Array[index];
        direction.Normalize();
        this.astPool.Get().Item.InitFromPool(vector2Array[index], ASTSize.ast_large, direction);
      }
      this.astEmitter = new ParticleEmitter2D("ast_particle1", 5, 20, "sheets/asteroids", this.Screen);
      this.astEmitter.ApplyTemplate(new ParticleEmitterTemplate()
      {
        rotateMax = 360f,
        velocityMax = 200,
        RotationSpeed = 200f
      });
      this.astEmitter.SetSourceRect();
      this.Add((SceneNode) this.astEmitter);
      this.smokeEmitter = new ParticleEmitter2D("smoke", 10, 20, "sheets/asteroids", this.Screen);
      this.smokeEmitter.ApplyTemplate(new ParticleEmitterTemplate()
      {
        scaleRate = 2f,
        velocityMax = 100,
        RotationSpeed = 150f,
        Scale = 0.0f,
        scaleMin = 0.5f
      });
      this.smokeEmitter.SetSourceRect();
      this.Add((SceneNode) this.smokeEmitter);
      this.shardEmitter = new ParticleEmitter2D("ast_particle1", 10, 20, "sheets/asteroids", this.Screen);
      this.shardEmitter.ApplyTemplate(new ParticleEmitterTemplate()
      {
        Directional = true,
        Scale = 0.0f,
        scaleMin = 0.5f,
        scaleMax = 0.75f,
        scaleRate = -0.5f,
        velocityMax = 250,
        velocityMin = 100,
        RotationSpeed = 150f
      });
      this.shardEmitter.SetSourceRect();
      this.Add((SceneNode) this.shardEmitter);
      this.sparkEmitter = new ParticleEmitter2D("sparkgreen", 10, 20, "sheets/asteroids", this.Screen);
      this.sparkEmitter.ApplyTemplate(new ParticleEmitterTemplate()
      {
        Directional = true,
        Scale = 0.0f,
        scaleMin = 0.5f,
        scaleRate = -0.5f
      });
      this.sparkEmitter.SetSourceRect();
      this.Add((SceneNode) this.sparkEmitter);
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      this.AsteroidCollisionCheck();
      foreach (AdvancedObjectPool<ASTAsteroid>.Node activeNode in this.astPool.ActiveNodes)
      {
        activeNode.Item.Update(gameTime, ref worldTransform);
        if (activeNode.Item.returnToPool)
          this.astPool.Return(activeNode);
      }
      this.AsteroidSpawnCheck();
      base.Update(gameTime, ref worldTransform);
    }

    private void AsteroidSpawnCheck()
    {
      if (this.astPool.ActiveCount >= 15 || this.astPool.AvailableCount <= 0)
        return;
      Vector2 vector = Tools.AngleToVector((!(((AsteroidScreen) this.Screen).uiNode.stickValue != Vector2.Zero) ? MathHelper.ToRadians(Tools.RandomFloat(0.0f, 360f)) : Tools.VectorToAngle(((AsteroidScreen) this.Screen).uiNode.stickValue)) + Tools.RandomFloat(-0.5235988f, 0.5235988f));
      vector.Normalize();
      Vector2 vector2 = ((AsteroidScreen) this.Screen).playLayer.heroSprite.Position.ToVector2();
      Vector2 where = vector2 + vector * 600f;
      Vector2 direction = vector2 - where;
      direction.Normalize();
      this.astPool.Get().Item.InitFromPool(where, (ASTSize) Tools.RandomInt(4), direction);
    }

    private void AsteroidCollisionCheck()
    {
      foreach (ASTAsteroid astAsteroid1 in this.astPool)
      {
        foreach (ASTAsteroid astAsteroid2 in this.astPool)
        {
          if (astAsteroid2.ID != astAsteroid1.ID && !astAsteroid1.returnToPool && !astAsteroid2.returnToPool && (double) astAsteroid1.armTimer >= 1.0 && (double) astAsteroid2.armTimer >= 1.0)
          {
            float num1 = astAsteroid2.asteroid.Position.X - astAsteroid1.asteroid.Position.X;
            float num2 = astAsteroid2.asteroid.Position.Y - astAsteroid1.asteroid.Position.Y;
            if ((double) num1 * (double) num1 + (double) num2 * (double) num2 < ((double) astAsteroid1.radius + (double) astAsteroid2.radius) * ((double) astAsteroid1.radius + (double) astAsteroid2.radius))
            {
              this.CollisionSound();
              astAsteroid1.returnToPool = true;
              astAsteroid2.returnToPool = true;
              this.SpawnAsteroid(astAsteroid1.astSize, astAsteroid1.asteroid.Position.ToVector2());
              this.SpawnAsteroid(astAsteroid2.astSize, astAsteroid2.asteroid.Position.ToVector2());
              this.shardEmitter.particleTemplate.Directional = false;
              this.shardEmitter.Fire(astAsteroid1.asteroid.Position.ToVector2());
              this.shardEmitter.Fire(astAsteroid2.asteroid.Position.ToVector2());
            }
          }
        }
      }
    }

    public bool AsteroidBulletCheck(Vector2 where)
    {
      foreach (ASTAsteroid astAsteroid in this.astPool)
      {
        if (!astAsteroid.returnToPool)
        {
          float num1 = where.X - astAsteroid.asteroid.Position.X;
          float num2 = where.Y - astAsteroid.asteroid.Position.Y;
          if ((double) num1 * (double) num1 + (double) num2 * (double) num2 < ((double) astAsteroid.radius + 10.0) * ((double) astAsteroid.radius + 10.0))
          {
            Vector2 vector = ((AsteroidScreen) this.Screen).playLayer.heroSprite.Position.ToVector2() - where;
            vector.Normalize();
            this.sparkEmitter.particleTemplate.Directional = true;
            this.sparkEmitter.particleTemplate.Direction = MathHelper.ToDegrees(Tools.VectorToAngle(vector));
            this.sparkEmitter.Fire(where);
            this.shardEmitter.particleTemplate.Directional = true;
            this.shardEmitter.particleTemplate.Direction = MathHelper.ToDegrees(Tools.VectorToAngle(vector));
            this.shardEmitter.Fire(where);
            --astAsteroid.hitPoints;
            if (astAsteroid.hitPoints <= 0)
            {
              ((AsteroidScreen) this.Screen).additiveLayer.DoBlastRing(astAsteroid.asteroid.Position.ToVector2());
              ((AsteroidScreen) this.Screen).uiNode.AddScore(250);
              this.CollisionSound();
              astAsteroid.returnToPool = true;
              this.SpawnAsteroid(astAsteroid.astSize, astAsteroid.asteroid.Position.ToVector2());
            }
            return true;
          }
        }
      }
      return false;
    }

    public bool AsteroidHeroCheck(Vector2 where)
    {
      bool flag = false;
      foreach (ASTAsteroid astAsteroid in this.astPool)
      {
        float num1 = where.X - astAsteroid.asteroid.Position.X;
        float num2 = where.Y - astAsteroid.asteroid.Position.Y;
        if ((double) num1 * (double) num1 + (double) num2 * (double) num2 < ((double) astAsteroid.radius + 30.0) * ((double) astAsteroid.radius + 30.0))
        {
          flag = true;
          Vector2 vector = where - astAsteroid.asteroid.Position.ToVector2();
          vector.Normalize();
          this.shardEmitter.particleTemplate.Directional = true;
          this.shardEmitter.particleTemplate.Direction = MathHelper.ToDegrees(Tools.VectorToAngle(vector));
          this.shardEmitter.Fire(where);
          astAsteroid.hitPoints = 0;
          if (astAsteroid.hitPoints <= 0)
          {
            this.CollisionSound();
            astAsteroid.returnToPool = true;
            this.SpawnAsteroid(astAsteroid.astSize, astAsteroid.asteroid.Position.ToVector2());
          }
        }
      }
      return flag;
    }

    private void CollisionSound()
    {
      if (Tools.RandomBool())
        this.Screen.audioManager.Play("explosion2");
      else
        this.Screen.audioManager.Play("explosion");
      if (Tools.RandomInt(100) >= 10)
        return;
      if (Tools.RandomBool())
        this.Screen.audioManager.Play("asthit1");
      else
        this.Screen.audioManager.Play("asthit2");
    }

    private void SpawnAsteroid(ASTSize size, Vector2 where)
    {
      this.astEmitter.Fire(where);
      this.smokeEmitter.Fire(where);
      Vector2 vector2_1 = new Vector2(Tools.RandomFloat(-1f, 1f), 1f);
      vector2_1.Normalize();
      Vector2 where1 = where + vector2_1 * 40f;
      ASTSize size1 = ASTSize.ast_medium;
      switch (size)
      {
        case ASTSize.ast_large:
          size1 = ASTSize.ast_medium;
          break;
        case ASTSize.ast_medium:
          size1 = Tools.RandomBool() ? ASTSize.ast_small1 : ASTSize.ast_small2;
          break;
        case ASTSize.ast_small1:
          return;
        case ASTSize.ast_small2:
          return;
      }
      Vector2 vector2_2 = Tools.RandomV3(-1f, 1f).ToVector2();
      vector2_2.Normalize();
      if (this.astPool.AvailableCount > 0)
        this.astPool.Get().Item.InitFromPool(where, size1, vector2_2);
      if (this.astPool.AvailableCount <= 0)
        return;
      this.astPool.Get().Item.InitFromPool(where1, size1, vector2_2 * -1f);
    }

    public override void Draw()
    {
      foreach (SceneNode sceneNode in this.astPool)
        sceneNode.Draw();
      base.Draw();
    }
  }
}
