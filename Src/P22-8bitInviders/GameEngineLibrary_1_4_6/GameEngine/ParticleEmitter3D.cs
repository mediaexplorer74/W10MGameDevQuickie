// Decompiled with JetBrains decompiler
// Type: GameEngine.ParticleEmitter3D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace GameEngine
{
  public class ParticleEmitter3D : SceneNode
  {
    private ObjectPool<Particle3D2> particlePool;
    private float texWidth;
    private float texHeight;
    private Texture2D tex;
    private Matrix World = Matrix.Identity;
    private Matrix WorldParentTransform = Matrix.Identity;
    private Matrix parentTransform = Matrix.Identity;
    private bool initialFire = true;
    protected ParticleQuad particleQuad;
    protected bool activated = true;
    protected Vector3 positionDelta;
    protected Vector3 lastPos;
    protected string imageName;
    protected int numparticles;
    protected int howManyEffects;
    public bool moveRelative;
    public Vector3 Origin;
    public float Opacity = 1f;
    public Vector3 Color = Vector3.One;
    public BlendState blendState = BlendState.AlphaBlend;
    public DepthStencilState depthState = DepthStencilState.DepthRead;
    public ParticleEmitterTemplate3D particleTemplate;

    public int particleCount => this.particlePool.ActiveCount;

    public override float Width
    {
      get => this.texWidth;
      set
      {
        base.Width = value;
        this.Pivot = new Vector3((float) ((double) this.Width * (double) this.Scale / 2.0), (float) ((double) this.Height * (double) this.Scale / 2.0), 0.0f);
      }
    }

    public override float Height
    {
      get => this.texHeight;
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
      set => base.Pivot = new Vector3(value.X, value.Y, value.Z);
    }

    public ParticleEmitter3D(
      string name,
      int maxparticles,
      int numberEffects,
      string texture,
      GameScreen screen)
      : base(name, screen)
    {
      this.particleQuad = new ParticleQuad(Vector3.Backward, Vector3.Up, 1f, 1f, texture, screen);
      this.imageName = texture;
      this.numparticles = maxparticles;
      this.howManyEffects = numberEffects;
      this.particlePool = new ObjectPool<Particle3D2>(maxparticles * numberEffects);
      this.tex = this.Screen.textureManager.Load<Texture2D>(this.imageName);
      this.texWidth = (float) this.tex.Width;
      this.texHeight = (float) this.tex.Height;
      this.Pivot = new Vector3((float) ((double) this.tex.Width * (double) this.Scale / 2.0), (float) ((double) this.tex.Height * (double) this.Scale / 2.0), 0.0f);
      this.ApplyTemplate(new ParticleEmitterTemplate3D());
    }

    public void ApplyTemplate(ParticleEmitterTemplate3D template)
    {
      this.particleTemplate = template;
    }

    public void Fire(Vector3 where)
    {
      Vector3 vector3 = new Vector3(where.X, where.Y, where.Z);
      this.Position = new Vector3(where.X, where.Y, where.Z);
      this.positionDelta = this.lastPos = vector3;
      this.Origin.X = vector3.X;
      this.Origin.Y = vector3.Y;
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
      Vector3 zero = Vector3.Zero;
      Vector3 velocity;
      if (this.particleTemplate.Directional)
      {
        velocity = Vector3.Normalize(this.particleTemplate.Direction + new Vector3(Tools.RandomFloat(-this.particleTemplate.Spread, this.particleTemplate.Spread), Tools.RandomFloat(-this.particleTemplate.Spread, this.particleTemplate.Spread), Tools.RandomFloat(-this.particleTemplate.Spread, this.particleTemplate.Spread))) * Tools.RandomFloat(this.particleTemplate.velocityMin, this.particleTemplate.velocityMax);
      }
      else
      {
        Vector3 vector3 = this.RandomVector(new Vector3(-1f, -1f, -1f), new Vector3(1f, 1f, 1f));
        vector3.Normalize();
        velocity = vector3 * Tools.RandomFloat(this.particleTemplate.velocityMin, this.particleTemplate.velocityMax);
      }
      float lifetime = (double) this.particleTemplate.Lifetime != 0.0 ? this.particleTemplate.Lifetime : Tools.RandomFloat(this.particleTemplate.lifeMinMax.X, this.particleTemplate.lifeMinMax.Y);
      float scale = (double) this.particleTemplate.Scale != 0.0 ? this.particleTemplate.Scale : Tools.RandomFloat(this.particleTemplate.scaleMin, this.particleTemplate.scaleMax);
      float degrees = (double) this.particleTemplate.rotateMax == -1.0 ? this.particleTemplate.Rotation : Tools.RandomFloat(this.particleTemplate.rotateMin, this.particleTemplate.rotateMax);
      float radians = MathHelper.ToRadians(this.particleTemplate.RotationSpeed);
      p.Rotation = MathHelper.ToRadians(degrees);
      p.doFade = this.particleTemplate.doFade;
      p.decayFactor = this.particleTemplate.decayFactor;
      p.scaleRate = this.particleTemplate.scaleRate;
      p.OrientToDirection = this.particleTemplate.OrientToDirection;
      p.Gravity = this.particleTemplate.Gravity;
      Matrix matrix;
      if (this.initialFire)
      {
        matrix = this.WorldParentTransform * Matrix.CreateTranslation(where);
        this.initialFire = false;
      }
      else
        matrix = Matrix.CreateTranslation(where);
      p.Initialize(matrix.Translation, velocity, lifetime, scale, radians);
    }

    public void setOrigin(Vector3 ori)
    {
      this.lastPos.X = this.Origin.X;
      this.lastPos.Y = this.Origin.Y;
      this.Origin = ori;
      this.positionDelta.X = this.Origin.X - this.lastPos.X;
      this.positionDelta.Y = this.Origin.Y - this.lastPos.Y;
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      if (!this.activated)
        return;
      this.parentTransform = worldTransform;
      Matrix translation = Matrix.CreateTranslation(this.Position);
      this.WorldParentTransform = !this.moveRelative ? translation * this.parentTransform : translation;
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      foreach (ObjectPool<Particle3D2>.Node activeNode in this.particlePool.ActiveNodes)
      {
        Particle3D2 p = activeNode.Item;
        if (p.Active)
        {
          p.Update(totalSeconds, this.positionDelta);
          if (!p.Active)
          {
            if (this.particleTemplate.doDeathReset)
              this.InitializeParticle(p, this.Position);
            else
              this.particlePool.Return(activeNode);
          }
        }
      }
      base.Update(gameTime, ref this.WorldParentTransform);
    }

    protected Vector3 RandomVector(Vector3 min, Vector3 max)
    {
      return new Vector3()
      {
        X = Tools.RandomFloat(min.X, max.X),
        Y = Tools.RandomFloat(min.Y, max.Y),
        Z = Tools.RandomFloat(min.Z, max.Z)
      };
    }

    public override void Draw(ICamera camera)
    {
      if (!this.activated)
        return;
      Engine.gdm.GraphicsDevice.DepthStencilState = this.depthState;
      Engine.gdm.GraphicsDevice.BlendState = this.blendState;
      if (this.moveRelative)
      {
        foreach (Particle3D2 particle3D2 in this.particlePool)
        {
          if (particle3D2.Active)
          {
            Matrix translation1 = Matrix.CreateTranslation(particle3D2.Position);
            Matrix scale1 = Matrix.CreateScale(particle3D2.Scale);
            Matrix fromAxisAngle = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), particle3D2.Rotation);
            Vector3 scale2;
            Quaternion rotation;
            Vector3 translation2;
            (translation1 * this.parentTransform).Decompose(out scale2, out rotation, out translation2);
            Matrix world = Matrix.CreateWorld(translation2, translation2 - camera.camPosition, this.Up);
            Matrix matrix = Matrix.Invert(Matrix.CreateFromQuaternion(rotation));
            this.parentTransform.Decompose(out scale2, out rotation, out translation2);
            Matrix fromQuaternion = Matrix.CreateFromQuaternion(rotation);
            this.particleQuad.quadEffect.Alpha = particle3D2.Alpha;
            this.particleQuad.quadEffect.World = scale1 * fromAxisAngle * world * matrix * fromQuaternion;
            this.particleQuad.Draw(camera);
          }
        }
      }
      else
      {
        foreach (Particle3D2 particle3D2 in this.particlePool)
        {
          if (particle3D2.Active)
          {
            Matrix scale = Matrix.CreateScale(particle3D2.Scale);
            Matrix fromAxisAngle = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), particle3D2.Rotation);
            Matrix world = Matrix.CreateWorld(particle3D2.Position, particle3D2.Position - camera.camPosition, Vector3.Up);
            this.particleQuad.quadEffect.Alpha = particle3D2.Alpha;
            this.particleQuad.quadEffect.World = scale * fromAxisAngle * world;
            this.particleQuad.quadEffect.AmbientLightColor = this.Color;
            this.particleQuad.Draw(camera);
          }
        }
      }
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
