// Decompiled with JetBrains decompiler
// Type: GameEngine.SpriteChild
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
  public class SpriteChild : SceneNode
  {
    protected Texture2D tex;
    private string contentName;
    public float Opacity = 1f;
    public float tintR;
    public float tintG;
    public float tintB;
    public Rectangle SourceRect;
    public SpriteEffects Facing;
    public float layerDepth;
    private float _Scale;
    private Matrix parentTransform;
    public Vector2 NonUniformScale = Vector2.One;
    public Matrix WorldTransform;
    public Matrix LocalTransform;
    public Vector2 WorldPosition;
    public float WorldRotation;
    public Vector2 WorldScale;

    public float Scale
    {
      get => this._Scale;
      set
      {
        this._Scale = value;
        this.NonUniformScale.X = this.NonUniformScale.Y = value;
      }
    }

    public Rectangle TransformedBounds
    {
      get
      {
        Vector2 position;
        this.DecomposeMatrix(ref this.WorldTransform, out position, out float _, out Vector2 _);
        Matrix matrix = Matrix.Invert(this.WorldTransform);
        Vector2 vector2 = Vector2.Transform(position, matrix);
        vector2.X -= this.Pivot.X;
        vector2.Y -= this.Pivot.Y;
        return new Rectangle((int) ((double) vector2.X - (double) this.Width), (int) ((double) vector2.Y - (double) this.Height), this.tex.Width, this.tex.Height);
      }
    }

    public bool ContainsPoint(Vector2 point)
    {
      Camera2D camera = ((Layer2D) this.Root.Parent).Camera;
      point = Vector2.Transform(point, Matrix.Invert(camera.LocalTransform));
      Matrix matrix = Matrix.Invert(this.WorldTransform);
      Vector2 point1 = Vector2.Transform(point, matrix);
      point1.X -= this.Width;
      point1.Y -= this.Height;
      return this.TransformedBounds.Contains(point1);
    }

    public int RealWidth => this.SourceRect.Width;

    public int RealHeight => this.SourceRect.Height;

    public new float Width => (float) this.SourceRect.Width * this.NonUniformScale.X;

    public new float Height => (float) this.SourceRect.Height * this.NonUniformScale.Y;

    public SpriteChild(string sourcename, Vector2 position, string imageName, GameScreen screen)
      : base(sourcename, position, screen)
    {
      this.Facing = SpriteEffects.None;
      this.Screen = screen;
      this.Name = sourcename;
      this.Position = position.ToVector3();
      this.contentName = imageName;
      this.tintR = this.tintG = this.tintB = 1f;
      this.Scale = 1f;
      this.Initialize();
    }

    ~SpriteChild()
    {
    }

    public override void Initialize()
    {
      this.tex = this.Screen.textureManager.Load(this.contentName);
      this.SourceRect = new Rectangle(0, 0, this.tex.Width, this.tex.Height);
      if (!(this.Pivot == Vector3.Zero))
        return;
      this.Pivot = new Vector3(this.Width / 2f, this.Height / 2f, 0.0f);
    }

    private void DecomposeMatrix(
      ref Matrix matrix,
      out Vector2 position,
      out float rotation,
      out Vector2 scale)
    {
      Vector3 scale1;
      Quaternion rotation1;
      Vector3 translation;
      matrix.Decompose(out scale1, out rotation1, out translation);
      Vector2 vector2 = Vector2.Transform(Vector2.UnitX, rotation1);
      rotation = (float) Math.Atan2((double) vector2.Y, (double) vector2.X);
      position = new Vector2(translation.X, translation.Y);
      scale = new Vector2(scale1.X, scale1.Y);
    }

    private void GetLocalTransform(out Matrix local)
    {
      Matrix result1;
      Matrix.CreateRotationZ(this.Rotation, out result1);
      Matrix result2;
      Matrix.CreateScale(this.NonUniformScale.X, this.NonUniformScale.Y, 1f, out result2);
      Matrix result3;
      Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0.0f, out result3);
      Matrix result4;
      Matrix.Multiply(ref result2, ref result1, out result4);
      Matrix.Multiply(ref result4, ref result3, out local);
      this.LocalTransform = local;
    }

    public void SetSourceRect(Rectangle rect)
    {
      this.SourceRect = rect;
      this.Pivot = new Vector3((float) rect.Width / 2f, (float) rect.Height / 2f, 0.0f);
    }

    public void SetTexture(string contentName)
    {
      this.tex = this.Screen.textureManager.Load(contentName);
      if (this.Pivot == Vector3.Zero)
        this.Pivot = new Vector3(this.Width / 2f, this.Height / 2f, 0.0f);
      this.SourceRect = new Rectangle(0, 0, this.tex.Width, this.tex.Height);
    }

    public override void SetSourceRect()
    {
      this.SourceRect = this.Screen.GetSpriteSource(this.Name);
      if (this.Pivot != Vector3.Zero)
        this.Pivot = new Vector3((float) this.SourceRect.Width / 2f, (float) this.SourceRect.Height / 2f, 0.0f);
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.SetSourceRect();
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      this.parentTransform = worldTransform;
      Matrix local;
      this.GetLocalTransform(out local);
      Matrix.Multiply(ref local, ref this.parentTransform, out this.WorldTransform);
      base.Update(gameTime, ref this.WorldTransform);
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
      this.DecomposeMatrix(ref this.WorldTransform, out this.WorldPosition, out this.WorldRotation, out this.WorldScale);
      this.Screen.spriteBatch.Draw(this.tex, this.WorldPosition, new Rectangle?(this.SourceRect), new Color(new Vector3(this.tintR, this.tintG, this.tintB)) * this.Opacity, this.WorldRotation, this.Pivot.ToVector2(), this.WorldScale, this.Facing, this.layerDepth);
      base.Draw();
    }
  }
}
