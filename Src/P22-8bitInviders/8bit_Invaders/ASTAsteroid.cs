// Decompiled with JetBrains decompiler
// Type: GameManager.ASTAsteroid
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public class ASTAsteroid : SceneNode
  {
    private float speed;
    private float rotRate;
    private static int staticID;
    public ASTSize astSize;
    public int ID;
    public Sprite asteroid;
    public bool returnToPool;
    public float radius;
    public float armTimer;
    public int hitPoints;

    public ASTAsteroid(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.asteroid = new Sprite("ast_large", Vector2.Zero, "sheets/asteroids", this.Screen);
      this.Add((SceneNode) this.asteroid);
    }

    public void InitFromPool(Vector2 where, ASTSize size, Vector2 direction)
    {
      this.armTimer = 0.0f;
      ++ASTAsteroid.staticID;
      this.ID = ASTAsteroid.staticID;
      this.returnToPool = false;
      this.asteroid.SetTextureSource(size.ToString());
      this.asteroid.Direction = direction.ToVector3();
      this.asteroid.Position = where.ToVector3();
      this.asteroid.Rotation = 0.0f;
      this.astSize = size;
      this.rotRate = Tools.RandomFloat(5f, 50f);
      switch (size)
      {
        case ASTSize.ast_large:
          this.radius = 119f;
          this.speed = Tools.RandomFloat(10f, 50f);
          this.hitPoints = 6;
          break;
        case ASTSize.ast_medium:
          this.radius = 57f;
          this.speed = Tools.RandomFloat(60f, 80f);
          this.hitPoints = 3;
          break;
        case ASTSize.ast_small1:
          this.radius = 35f;
          this.speed = Tools.RandomFloat(80f, 120f);
          this.hitPoints = 2;
          break;
        case ASTSize.ast_small2:
          this.radius = 28f;
          this.speed = Tools.RandomFloat(80f, 120f);
          this.hitPoints = 2;
          break;
      }
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.armTimer += totalSeconds;
      Vector2 vector2 = ((AsteroidScreen) this.Screen).playLayer.heroSprite.Position.ToVector2();
      this.asteroid.Rotation += totalSeconds * this.rotRate;
      Sprite asteroid = this.asteroid;
      asteroid.Position = asteroid.Position + this.asteroid.Direction * this.speed * totalSeconds;
      if ((double) Vector2.Distance(this.asteroid.Position.ToVector2(), vector2) > 800.0)
        this.returnToPool = true;
      base.Update(gameTime, ref worldTransform);
    }
  }
}
