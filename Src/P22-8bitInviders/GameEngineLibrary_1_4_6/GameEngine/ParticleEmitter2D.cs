// Decompiled with JetBrains decompiler
// Type: GameEngine.ParticleEmitter2D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class ParticleEmitter2D : SceneNode
  {
    protected ObjectPool<Particle2D> particlePool;
    protected Texture2D tex;
    protected bool activated = true;
    protected Vector2 positionDelta;
    protected Vector2 lastPos;
    protected string contentName;
    protected int numparticles;
    protected int howManyEffects;
    public Rectangle SourceRect;
    public Vector3 Origin;
    public Color Color = Color.White;
    public float Opacity = 1f;
    public ParticleEmitterTemplate particleTemplate;

    public int particleCount => this.particlePool.ActiveCount;

    public override float Width
    {
      get => (float) this.SourceRect.Width;
      set
      {
        base.Width = value;
        this.Pivot = new Vector3((float) ((double) this.Width * (double) this.Scale / 2.0), (float) ((double) this.Height * (double) this.Scale / 2.0), 0.0f);
      }
    }

    public override float Height
    {
      get => (float) this.SourceRect.Height;
      set
      {
        base.Height = value;
        this.Pivot = new Vector3((float) ((double) this.Width * (double) this.Scale / 2.0), (float) ((double) this.Height * (double) this.Scale / 2.0), 0.0f);
      }
    }

    public override Vector3 Pivot
    {
      get
      {
        return new Vector3((float) ((double) this.Width * (double) this.Scale / 2.0), (float) ((double) this.Height * (double) this.Scale / 2.0), 0.0f);
      }
      set => base.Pivot = new Vector3(value.X, value.Y, 0.0f);
    }

    public ParticleEmitter2D(
      string sourcename,
      int maxparticles,
      int numberEffects,
      string texture,
      GameScreen screen)
      : base(sourcename, screen)
    {
      this.contentName = texture;
      this.numparticles = maxparticles;
      this.howManyEffects = numberEffects;
      this.particlePool = new ObjectPool<Particle2D>(maxparticles * numberEffects);
      this.tex = this.Screen.textureManager.Load<Texture2D>(this.contentName);
      this.Pivot = new Vector3((float) ((double) this.tex.Width * (double) this.Scale / 2.0), (float) ((double) this.tex.Height * (double) this.Scale / 2.0), 0.0f);
      this.SourceRect = new Rectangle(0, 0, this.tex.Width, this.tex.Height);
      this.ApplyTemplate(new ParticleEmitterTemplate());
    }

    public override void SetSourceRect()
    {
      this.SourceRect = this.Screen.GetSpriteSource(this.Name);
      if (this.SourceRect == Rectangle.Empty)
        this.SourceRect = new Rectangle(0, 0, this.tex.Width, this.tex.Height);
      if (this.Pivot != Vector3.Zero)
        this.Pivot = new Vector3((float) this.SourceRect.Width / 2f, (float) this.SourceRect.Height / 2f, 0.0f);
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.SetSourceRect();
    }

    public void SetTextureSource(string name)
    {
      this.Name = name;
      this.SetSourceRect();
    }

    public void ApplyTemplate(ParticleEmitterTemplate template) => this.particleTemplate = template;

    public void Fire(Vector2 where)
    {
      Vector2 vector2 = new Vector2(where.X, where.Y);
      this.Position = new Vector3(where.X, where.Y, 0.0f);
      this.positionDelta = this.lastPos = vector2;
      this.Origin.X = vector2.X;
      this.Origin.Y = vector2.Y;
      int num = Math.Min(this.numparticles, this.particlePool.AvailableCount);
      while (num-- > 0)
        this.InitializeParticle(this.particlePool.Get().Item, where);
    }

    public void FireAndStart(Vector2 where)
    {
      this.activated = true;
      this.Fire(where);
    }

    protected void InitializeParticle(Particle2D p, Vector2 where)
    {
      Vector2 velocity = Vector2.Zero;
      if (this.particleTemplate.Directional)
      {
        float num = Tools.RandomFloat((float) (-(double) this.particleTemplate.Spread / 2.0), this.particleTemplate.Spread / 2f);
        velocity.X = (float) Math.Cos((double) MathHelper.ToRadians(this.particleTemplate.Direction + num) - 1.5707962512969971) * (float) Tools.RandomInt(this.particleTemplate.velocityMin, this.particleTemplate.velocityMax);
        velocity.Y = (float) Math.Sin((double) MathHelper.ToRadians(this.particleTemplate.Direction + num) - 1.5707962512969971) * (float) Tools.RandomInt(this.particleTemplate.velocityMin, this.particleTemplate.velocityMax);
      }
      else
      {
        Vector2 vector2 = this.RandomVector(new Vector2(-1f, -1f), new Vector2(1f, 1f));
        vector2.Normalize();
        velocity = vector2 * (float) Tools.RandomInt(this.particleTemplate.velocityMin, this.particleTemplate.velocityMax);
      }
      float lifetime = (double) this.particleTemplate.Lifetime != 0.0 ? this.particleTemplate.Lifetime : Tools.RandomFloat(this.particleTemplate.lifeMinMax.X, this.particleTemplate.lifeMinMax.Y);
      float scale = (double) this.particleTemplate.Scale != 0.0 ? this.particleTemplate.Scale : Tools.RandomFloat(this.particleTemplate.scaleMin, this.particleTemplate.scaleMax);
      float degrees = (double) this.particleTemplate.rotateMax == -1.0 ? this.particleTemplate.Rotation : Tools.RandomFloat(this.particleTemplate.rotateMin, this.particleTemplate.rotateMax);
      float radians = MathHelper.ToRadians(this.particleTemplate.RotationSpeed);
      p.color = this.Color;
      p.Rotation = p.startRotation = MathHelper.ToRadians(degrees);
      p.doFade = this.particleTemplate.doFade;
      p.decayFactor = this.particleTemplate.decayFactor;
      p.scaleRate = this.particleTemplate.scaleRate;
      p.OrientToDirection = this.particleTemplate.OrientToDirection;
      p.Gravity = this.particleTemplate.Gravity;
      p.Follow = this.particleTemplate.Follow;
      p.Initialize(where, velocity, lifetime, scale, radians);
    }

    public void setOrigin(Vector3 ori)
    {
      this.lastPos.X = this.Origin.X;
      this.lastPos.Y = this.Origin.Y;
      this.Origin = ori;
      this.positionDelta.X = this.Origin.X - this.lastPos.X;
      this.positionDelta.Y = this.Origin.Y - this.lastPos.Y;
    }

    protected Vector2 RandomVector(Vector2 min, Vector2 max)
    {
      return new Vector2()
      {
        X = Tools.RandomFloat(min.X, max.X),
        Y = Tools.RandomFloat(min.Y, max.Y)
      };
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      foreach (ObjectPool<Particle2D>.Node activeNode in this.particlePool.ActiveNodes)
      {
        Particle2D p = activeNode.Item;
        if (p.Active)
        {
          p.Update(totalSeconds, this.positionDelta);
          if (!p.Active)
          {
            if (this.particleTemplate.doDeathReset)
            {
              if (!this.activated)
                this.InitializeParticle(p, new Vector2(1000000f, 1000000f));
              else
                this.InitializeParticle(p, this.Position.ToVector2());
            }
            else
              this.particlePool.Return(activeNode);
          }
        }
      }
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
      foreach (Particle2D particle2D in this.particlePool)
      {
        if (particle2D.Active)
          this.Screen.spriteBatch.Draw(this.tex, particle2D.Position, new Rectangle?(this.SourceRect), particle2D.color * particle2D.Alpha * this.Opacity, particle2D.Rotation, this.Pivot.ToVector2(), particle2D.Scale, SpriteEffects.None, 1f);
      }
    }

    public void Start() => this.activated = true;

    public void Stop() => this.activated = false;

    public void StopAndRemoveParticles()
    {
      this.activated = false;
      foreach (ObjectPool<Particle2D>.Node activeNode in this.particlePool.ActiveNodes)
        this.particlePool.Return(activeNode);
    }
  }
}
