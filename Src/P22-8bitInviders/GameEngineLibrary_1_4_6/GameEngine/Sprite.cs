// Decompiled with JetBrains decompiler
// Type: GameEngine.Sprite
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class Sprite : SceneNode
  {
    protected Texture2D tex;
    protected string contentName;
    public float Opacity = 1f;
    public Color color;
    public Rectangle destination;
    public bool useRectangleScalar;
    public SpriteEffects Facing;
    public Rectangle SourceRect;
    public float layerDepth;

    public Texture2D Texture => this.tex;

    public Rectangle Bounds
    {
      get
      {
        return this.Pivot == Vector3.Zero ? new Rectangle((int) this.Position.X, (int) this.Position.Y, this.SourceRect.Width, this.SourceRect.Height) : new Rectangle((int) ((double) this.Position.X - (double) this.Pivot.X * (double) this.Scale), (int) ((double) this.Position.Y - (double) this.Pivot.Y * (double) this.Scale), (int) ((double) this.SourceRect.Width * (double) this.Scale), (int) ((double) this.SourceRect.Height * (double) this.Scale));
      }
    }

    public override float Width
    {
      get => (float) this.SourceRect.Width * this.Scale;
      set => base.Width = value;
    }

    public override float Height
    {
      get => (float) this.SourceRect.Height * this.Scale;
      set => base.Height = value;
    }

    public Sprite(string sourcename, Vector2 position, string imageName, GameScreen screen)
      : base(sourcename, position, screen)
    {
      this.Facing = SpriteEffects.None;
      this.contentName = imageName;
      this.color = Color.White;
      this.Initialize();
    }

    ~Sprite()
    {
    }

    public Texture2D GetTexture() => this.tex;

    public void SetTexture(string contentName)
    {
      this.tex = this.Screen.textureManager.Load(contentName);
      this.SourceRect = new Rectangle(0, 0, this.tex.Width, this.tex.Height);
      if (!(this.Pivot == Vector3.Zero))
        return;
      this.Pivot = new Vector3(this.Width / 2f, this.Height / 2f, 0.0f);
    }

    public void SetTextureSource(string name)
    {
      this.Name = name;
      this.SetSourceRect();
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

    public override void Initialize()
    {
      this.tex = this.Screen.textureManager.Load(this.contentName);
      this.SourceRect = new Rectangle(0, 0, this.tex.Width, this.tex.Height);
      if (!(this.Pivot == Vector3.Zero))
        return;
      this.Pivot = new Vector3(this.Width / 2f, this.Height / 2f, 0.0f);
    }

    public override void Draw()
    {
      base.Draw();
      if (!this.Visible || this.Delete)
        return;
      if (!this.useRectangleScalar)
        this.Screen.spriteBatch.Draw(this.tex, this.Position.ToVector2(), new Rectangle?(this.SourceRect), this.color * this.Opacity, MathHelper.ToRadians(this.Rotation), this.Pivot.ToVector2(), this.Scale, this.Facing, this.layerDepth);
      else
        this.Screen.spriteBatch.Draw(this.tex, this.destination, new Rectangle?(this.SourceRect), this.color * this.Opacity, MathHelper.ToRadians(this.Rotation), this.Pivot.ToVector2(), this.Facing, this.layerDepth);
    }
  }
}
