// Decompiled with JetBrains decompiler
// Type: GameManager.AdditiveLayer
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
  public class AdditiveLayer : Layer2D
  {
    private ParticleEmitter2D thrustEmitter;
    private ParticleEmitter2D thrustEmitter2;
    private ParticleEmitter2D sparkEmitter;
    private AdvancedObjectPool<InvaderExplosion> explosionPool;
    private Sprite[] blastRing;
    private Sprite beam;

    public AdditiveLayer(Camera2D cam, GameScreen screen)
      : base(cam, screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.blendState = BlendState.Additive;
      string str = "sheets/additive";
      this.thrustEmitter = new ParticleEmitter2D("fire", 40, 1, str, this.Screen);
      this.thrustEmitter.ApplyTemplate(new ParticleEmitterTemplate()
      {
        doDeathReset = true,
        Scale = 0.0f,
        scaleMin = 0.4f,
        scaleMax = 0.6f,
        Directional = true,
        scaleRate = -0.65f,
        rotateMax = 360f,
        RotationSpeed = 300f,
        velocityMax = 200,
        velocityMin = 100,
        Spread = 20f,
        lifeMinMax = new Vector2(0.4f, 0.7f)
      });
      this.thrustEmitter.Fire(new Vector2(240f, 400f));
      this.thrustEmitter2 = new ParticleEmitter2D("fire", 40, 1, str, this.Screen);
      this.thrustEmitter2.Color = Color.Red;
      this.thrustEmitter2.ApplyTemplate(new ParticleEmitterTemplate()
      {
        doDeathReset = true,
        Scale = 0.0f,
        scaleMin = 0.4f,
        scaleMax = 0.6f,
        Directional = true,
        scaleRate = -0.65f,
        rotateMax = 360f,
        RotationSpeed = 300f,
        velocityMax = 200,
        velocityMin = 100,
        Spread = 20f,
        lifeMinMax = new Vector2(0.4f, 0.7f)
      });
      this.thrustEmitter2.Fire(new Vector2(240f, 400f));
      this.sparkEmitter = new ParticleEmitter2D("thrustspark", 20, 1, str, this.Screen);
      this.sparkEmitter.ApplyTemplate(new ParticleEmitterTemplate()
      {
        doDeathReset = true,
        Scale = 0.0f,
        scaleMin = 0.4f,
        scaleMax = 0.6f,
        Directional = true,
        scaleRate = -0.65f,
        rotateMax = 360f,
        RotationSpeed = 300f,
        velocityMax = 200,
        velocityMin = 100,
        Spread = 20f,
        lifeMinMax = new Vector2(0.4f, 0.7f)
      });
      this.sparkEmitter.Fire(new Vector2(240f, 400f));
      this.beam = new Sprite("beam", Vector2.Zero, str, this.Screen);
      this.beam.Visible = false;
      this.Add((SceneNode) this.thrustEmitter);
      this.Add((SceneNode) this.thrustEmitter2);
      this.Add((SceneNode) this.sparkEmitter);
      this.Add((SceneNode) this.beam);
      this.explosionPool = new AdvancedObjectPool<InvaderExplosion>(20);
      for (int index = 0; index < 20; ++index)
        this.explosionPool.AddToPool(new InvaderExplosion(3, this.Screen));
      this.blastRing = new Sprite[10];
      for (int index1 = 0; index1 < 10; ++index1)
      {
        Sprite[] blastRing = this.blastRing;
        int index2 = index1;
        Sprite sprite1 = new Sprite("blastring", Vector2.Zero, str, this.Screen);
        sprite1.Visible = false;
        Sprite sprite2 = sprite1;
        blastRing[index2] = sprite2;
        this.Add((SceneNode) this.blastRing[index1]);
      }
      this.SetSourceRect();
      this.beam.Pivot = new Vector3(41f, 186f, 0.0f);
    }

    public void DoExplosion(Vector2 where)
    {
      this.Screen.audioManager.Play("explosion");
      this.explosionPool.Get().Item.InitFromPool(where);
    }

    public void DoBlastRing(Vector2 where)
    {
      for (int index = 0; index < 10; ++index)
      {
        if (!this.blastRing[index].Visible)
        {
          this.blastRing[index].Position = where.ToVector3();
          this.blastRing[index].Scale = 1f;
          this.blastRing[index].Opacity = 1f;
          this.blastRing[index].Rotation = (float) Tools.RandomInt(-90, 90);
          this.blastRing[index].Visible = true;
          break;
        }
      }
    }

    public void StartThrust() => this.thrustEmitter.Start();

    public void StopThrust() => this.thrustEmitter.Stop();

    public void ActivateTractor(Vector3 target)
    {
      this.beam.Visible = true;
      Vector2 vector = this.beam.Position.ToVector2() - target.ToVector2();
      vector.Normalize();
      this.beam.Rotation = 180f + MathHelper.ToDegrees(Tools.VectorToAngle(vector));
    }

    public void DisableTractor()
    {
      this.beam.Visible = false;
      this.beam.Opacity = 0.0f;
    }

    public void SetThrustPos(Vector3 where, float angle, float power)
    {
      this.thrustEmitter.Position = where;
      this.thrustEmitter.particleTemplate.Direction = 180f + angle;
      this.thrustEmitter2.Position = where;
      this.thrustEmitter2.particleTemplate.Direction = 180f + angle;
      this.sparkEmitter.Position = where;
      this.sparkEmitter.particleTemplate.Direction = 180f + angle;
      this.thrustEmitter.particleTemplate.velocityMax = (int) Tools.RemapValue(power, 0.0f, 60f, 100f, 400f);
      this.thrustEmitter2.particleTemplate.velocityMax = (int) Tools.RemapValue(power, 0.0f, 60f, 100f, 400f);
      this.sparkEmitter.particleTemplate.velocityMax = (int) Tools.RemapValue(power, 0.0f, 60f, 100f, 400f);
      this.thrustEmitter.particleTemplate.lifeMinMax.X = Tools.RemapValue(power, 0.0f, 60f, 0.4f, 0.8f);
      this.thrustEmitter.particleTemplate.lifeMinMax.Y = Tools.RemapValue(power, 0.0f, 60f, 0.7f, 1.2f);
      this.beam.Direction = where;
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      for (int index = 0; index < 10; ++index)
      {
        if (this.blastRing[index].Visible)
        {
          this.blastRing[index].Scale += totalSeconds * 3f;
          this.blastRing[index].Opacity = Math.Max(0.0f, this.blastRing[index].Opacity - totalSeconds * 0.5f);
          if ((double) this.blastRing[index].Opacity == 0.0)
            this.blastRing[index].Visible = false;
        }
      }
      foreach (AdvancedObjectPool<InvaderExplosion>.Node activeNode in this.explosionPool.ActiveNodes)
      {
        activeNode.Item.Update(gameTime, ref worldTransform);
        if (activeNode.Item.returnToPool)
          this.explosionPool.Return(activeNode);
      }
      this.beam.Position = this.beam.Direction + Tools.RandomV3(-2f, 2f);
      if (this.beam.Visible)
      {
        this.beam.Opacity = Math.Min(1f, this.beam.Opacity + (float) (4.0 * gameTime.ElapsedGameTime.TotalSeconds));
        this.beam.Scale = Tools.SineAnimation(gameTime, 20f, 0.9f, 1.1f);
      }
      base.Update(gameTime, ref worldTransform);
    }

    public override void Draw()
    {
      //this.Screen.spriteBatch.Begin(  this.sortMode, this.blendState, this.samplerState, 
      //    (DepthStencilState) null, (RasterizerState) null,  (Effect) null, this.Camera.LocalTransform);

      this.Screen.spriteBatch.Begin(SpriteSortMode.Deferred/*.BackToFront*/, null,
        null, null, null, null, Game1.globalTransformation);



      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.Draw();
      foreach (SceneNode sceneNode in this.explosionPool)
        sceneNode.Draw();
      this.Screen.spriteBatch.End();
    }
  }
}
