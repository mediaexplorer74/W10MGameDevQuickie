// Decompiled with JetBrains decompiler
// Type: GameManager.InvadersFXAdditiveLayer
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
  public class InvadersFXAdditiveLayer : Layer2D
  {
    private AdvancedObjectPool<InvaderExplosion> explosionPool;
    private AdvancedObjectPool<InvaderExplosion> explosionPool2;
    public ParticleEmitter2D expEmitter;
    public ParticleEmitter2D hitEmitter;
    private Sprite blastRing;

    public InvadersFXAdditiveLayer(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.blendState = BlendState.Additive;
      Sprite sprite = new Sprite("blastring", Vector2.Zero, "sheets/invaders", this.Screen);
      sprite.Visible = false;
      this.blastRing = sprite;
      this.Add((SceneNode) this.blastRing);
      this.explosionPool = new AdvancedObjectPool<InvaderExplosion>(20);
      this.explosionPool2 = new AdvancedObjectPool<InvaderExplosion>(20);
      for (int index = 0; index < 20; ++index)
      {
        this.explosionPool.AddToPool(new InvaderExplosion(1, this.Screen));
        this.explosionPool2.AddToPool(new InvaderExplosion(2, this.Screen));
      }
      this.AddParticles();
      this.SetSourceRect();
    }

    private void AddParticles()
    {
      this.hitEmitter = new ParticleEmitter2D("sparkblue", 15, 10, "sheets/invaders", this.Screen);
      this.hitEmitter.Color = Color.YellowGreen;
      this.hitEmitter.ApplyTemplate(new ParticleEmitterTemplate()
      {
        decayFactor = 0.2f,
        Spread = 90f,
        Scale = 0.0f,
        scaleMax = 0.5f,
        scaleMin = 0.2f,
        OrientToDirection = true,
        velocityMax = 250,
        velocityMin = 100,
        Rotation = 90f
      });
      this.Add((SceneNode) this.hitEmitter);
      this.expEmitter = new ParticleEmitter2D("spark", 20, 10, "sheets/invaders", this.Screen);
      this.expEmitter.ApplyTemplate(new ParticleEmitterTemplate()
      {
        scaleRate = -0.5f,
        decayFactor = 0.2f,
        Scale = 0.0f,
        scaleMax = 1.5f,
        scaleMin = 0.5f
      });
      this.Add((SceneNode) this.expEmitter);
    }

    public void AddSpark(Vector2 where) => this.hitEmitter.Fire(where);

    public void AddExplosion(int type, Vector2 where)
    {
      if (type == 1)
      {
        if (this.explosionPool.AvailableCount <= 0)
          return;
        this.Screen.audioManager.Play("explosion");
        this.explosionPool.Get().Item.InitFromPool(where);
        this.expEmitter.Fire(where);
      }
      else
      {
        if (this.explosionPool2.AvailableCount <= 0)
          return;
        this.Screen.audioManager.Play("explosion2");
        this.explosionPool2.Get().Item.InitFromPool(where);
        this.expEmitter.Fire(where);
      }
    }

    public void AddExplosionRandom(Vector2 where)
    {
      this.AddExplosion(Tools.RandomBool() ? 1 : 2, where);
    }

    public void AddBlastRing(Vector2 where)
    {
      if (this.blastRing.Visible)
        return;
      this.blastRing.Position = where.ToVector3();
      this.blastRing.Scale = 1f;
      this.blastRing.Opacity = 1f;
      this.blastRing.Rotation = (float) Tools.RandomInt(-90, 90);
      this.blastRing.Visible = true;
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      foreach (AdvancedObjectPool<InvaderExplosion>.Node activeNode in this.explosionPool.ActiveNodes)
      {
        activeNode.Item.Update(gameTime, ref worldTransform);
        if (activeNode.Item.returnToPool)
          this.explosionPool.Return(activeNode);
      }
      foreach (AdvancedObjectPool<InvaderExplosion>.Node activeNode in this.explosionPool2.ActiveNodes)
      {
        activeNode.Item.Update(gameTime, ref worldTransform);
        if (activeNode.Item.returnToPool)
          this.explosionPool2.Return(activeNode);
      }
      if (this.blastRing.Visible)
      {
        this.blastRing.Scale += totalSeconds * 3f;
        this.blastRing.Opacity = Math.Max(0.0f, this.blastRing.Opacity - totalSeconds * 0.5f);
        if ((double) this.blastRing.Opacity == 0.0)
          this.blastRing.Visible = false;
      }
      base.Update(gameTime, ref worldTransform);
    }

    public override void Draw()
    {
      this.Screen.spriteBatch.Begin(this.sortMode, this.blendState, this.samplerState, (DepthStencilState) null, (RasterizerState) null, (Effect) null, this.Camera.LocalTransform);
      this.blastRing.Draw();
      this.expEmitter.Draw();
      this.hitEmitter.Draw();
      foreach (SceneNode sceneNode in this.explosionPool)
        sceneNode.Draw();
      foreach (SceneNode sceneNode in this.explosionPool2)
        sceneNode.Draw();
      this.Screen.spriteBatch.End();
    }
  }
}
