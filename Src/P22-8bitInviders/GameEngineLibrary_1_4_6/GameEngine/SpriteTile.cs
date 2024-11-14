// Decompiled with JetBrains decompiler
// Type: GameEngine.SpriteTile
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class SpriteTile : SceneNode
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

    public SpriteTile(
      string sourcename,
      Vector2 position,
      Rectangle dimensions,
      string imageName,
      GameScreen screen)
      : base(sourcename, position, screen)
    {
      this.destination = dimensions;
      this.Facing = SpriteEffects.None;
      this.contentName = imageName;
      this.color = Color.White;
      this.Initialize();
    }

    ~SpriteTile()
    {
    }

    public Texture2D GetTexture() => this.tex;

    public void SetTexture(string contentName)
    {
      this.tex = contentName.Contains("JPG") || this.Name.Contains("JPG") ? this.Screen.textureManager.LoadJPG(contentName) : this.Screen.textureManager.Load(contentName);
      this.SourceRect = new Rectangle(0, 0, this.tex.Width, this.tex.Height);
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
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.SetSourceRect();
    }

    public override void Initialize()
    {
      this.tex = this.contentName.Contains("JPG") || this.Name.Contains("JPG") ? this.Screen.textureManager.LoadJPG(this.contentName) : this.Screen.textureManager.Load(this.contentName);
      this.SourceRect = new Rectangle(0, 0, this.tex.Width, this.tex.Height);
    }

    public override void Draw()
    {
      base.Draw();
      if (!this.Visible || this.Delete)
        return;
      this.Screen.spriteBatch.Draw(this.tex, this.Position.ToVector2(), new Rectangle?(this.destination), this.color * this.Opacity, MathHelper.ToRadians(this.Rotation), this.Pivot.ToVector2(), this.Scale, this.Facing, this.layerDepth);
    }
  }
}
