// Decompiled with JetBrains decompiler
// Type: GameEngine.ParticleEmitter3DV2
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
  public class ParticleEmitter3DV2 : SceneNode
  {
    protected BasicEffect basicEffect;
    protected ObjectPool<Particle3D2> particlePool;
    protected Texture2D tex;
    protected bool activated = true;
    protected Vector3 positionDelta;
    protected Vector3 lastPos;
    protected string contentName;
    protected int numparticles;
    protected int howManyEffects;
    public Rectangle SourceRect;
    public bool useBillboard = true;
    public Vector3 Origin;
    public Color Color = Color.White;
    public float Opacity = 1f;
    public ParticleEmitterTemplate particleTemplate;

    public int particleCount => this.particlePool.ActiveCount;

    public ParticleEmitter3DV2(
      string sourcename,
      int maxparticles,
      int numberEffects,
      string texture,
      GameScreen screen)
      : base(sourcename, screen)
    {
      this.basicEffect = new BasicEffect(Engine.game.GraphicsDevice)
      {
        TextureEnabled = true,
        VertexColorEnabled = true
      };
      this.contentName = texture;
      this.numparticles = maxparticles;
      this.howManyEffects = numberEffects;
      this.particlePool = new ObjectPool<Particle3D2>(maxparticles * numberEffects);
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

    public void ApplyTemplate(ParticleEmitterTemplate template) => this.particleTemplate = template;

    public void Fire(Vector3 where)
    {
      Vector3 vector3 = new Vector3(where.X, where.Y, where.Z);
      this.Position = new Vector3(where.X, where.Y, where.Z);
      this.positionDelta = this.lastPos = vector3;
      this.Origin.X = vector3.X;
      this.Origin.Y = vector3.Y;
      this.Origin.Z = vector3.Z;
      int num = Math.Min(this.numparticles, this.particlePool.AvailableCount);
      while (num-- > 0)
        this.InitializeParticle(this.particlePool.Get().Item, where);
    }

    public void FireAndStart(Vector3 where)
    {
      this.activated = true;
      this.Fire(where);
    }

    protected void InitializeParticle(Particle3D2 p, Vector3 where)
    {
      Vector3 velocity = Vector3.Zero;
      if (this.particleTemplate.Directional)
      {
        float num1 = Tools.RandomFloat((float) (-(double) this.particleTemplate.Spread / 2.0), this.particleTemplate.Spread / 2f);
        velocity.X = (float) Math.Cos((double) MathHelper.ToRadians(this.particleTemplate.Direction + num1) - 1.5707962512969971) * Tools.RandomFloat((float) this.particleTemplate.velocityMin, (float) this.particleTemplate.velocityMax);
        float num2 = Tools.RandomFloat((float) this.particleTemplate.velocityMin, (float) this.particleTemplate.velocityMax);
        velocity.Z = (float) Math.Sin((double) MathHelper.ToRadians(this.particleTemplate.Direction + num1) - 1.5707962512969971) * num2;
      }
      else
      {
        Vector3 vector3 = Tools.RandomV3(-1f, 1f);
        vector3.Normalize();
        velocity = vector3 * Tools.RandomFloat((float) this.particleTemplate.velocityMin, (float) this.particleTemplate.velocityMax);
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
      this.lastPos.Z = this.Origin.Z;
      this.Origin = ori;
      this.positionDelta.X = this.Origin.X - this.lastPos.X;
      this.positionDelta.Y = this.Origin.Y - this.lastPos.Y;
      this.positionDelta.Z = this.Origin.Z - this.lastPos.Z;
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
      foreach (ObjectPool<Particle3D2>.Node activeNode in this.particlePool.ActiveNodes)
      {
        Particle3D2 p = activeNode.Item;
        p.useBillboard = this.useBillboard;
        if (p.Active)
        {
          p.Update(totalSeconds, this.positionDelta);
          if (!p.Active)
          {
            if (this.particleTemplate.doDeathReset)
            {
              if (!this.activated)
                this.InitializeParticle(p, new Vector3(1000000f, 1000000f, 1000000f));
              else
                this.InitializeParticle(p, this.Position);
            }
            else
              this.particlePool.Return(activeNode);
          }
        }
      }
    }

    public override void Draw(ICamera camera)
    {
      if (!this.Visible || this.particlePool.ActiveCount == 0)
        return;
      Matrix scale = Matrix.CreateScale(1f, -1f, 1f);
      if (this.useBillboard)
      {
        this.basicEffect.World = Matrix.Identity;
        this.basicEffect.View = Matrix.Identity;
        this.basicEffect.Projection = camera.ProjectionMatrix;
        this.Screen.spriteBatch.Begin(SpriteSortMode.Deferred, (BlendState) null, (SamplerState) null, (DepthStencilState) null, RasterizerState.CullNone, (Effect) this.basicEffect);
        foreach (Particle3D2 particle3D2 in this.particlePool)
        {
          if (particle3D2.Active)
          {
            Vector3 vector3 = Vector3.Transform(particle3D2.Position, camera.ViewMatrix * scale);
            this.Screen.spriteBatch.Draw(this.tex, new Vector2(vector3.X, -vector3.Y), new Rectangle?(this.SourceRect), particle3D2.color * particle3D2.Alpha * this.Opacity, particle3D2.Rotation, this.Pivot.ToVector2(), particle3D2.Scale, SpriteEffects.None, vector3.Z);
          }
        }
        this.Screen.spriteBatch.End();
      }
      else
      {
        this.basicEffect.World = Matrix.CreateConstrainedBillboard(Vector3.Zero, Vector3.Up, Vector3.Right, new Vector3?(), new Vector3?());
        this.basicEffect.View = camera.ViewMatrix;
        this.basicEffect.Projection = camera.ProjectionMatrix;
        this.Screen.spriteBatch.Begin(SpriteSortMode.Deferred, (BlendState) null, (SamplerState) null, (DepthStencilState) null, RasterizerState.CullNone, (Effect) this.basicEffect);
        foreach (Particle3D2 particle3D2 in this.particlePool)
        {
          if (particle3D2.Active)
            this.Screen.spriteBatch.Draw(this.tex, new Vector2(-particle3D2.Position.Z, particle3D2.Position.X), new Rectangle?(this.SourceRect), this.Color * particle3D2.Alpha * this.Opacity, particle3D2.Rotation, this.Pivot.ToVector2(), particle3D2.Scale, SpriteEffects.None, 0.0f);
        }
        this.Screen.spriteBatch.End();
      }
      base.Draw(camera);
    }

    public void Start() => this.activated = true;

    public void Stop() => this.activated = false;

    public void StopAndRemoveParticles()
    {
      this.activated = false;
      foreach (ObjectPool<Particle3D2>.Node activeNode in this.particlePool.ActiveNodes)
        this.particlePool.Return(activeNode);
    }
  }
}
