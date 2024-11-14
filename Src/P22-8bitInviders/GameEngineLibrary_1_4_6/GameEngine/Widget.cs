// Decompiled with JetBrains decompiler
// Type: GameEngine.Widget
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public abstract class Widget : SceneNode
  {
    protected Texture2D tex;
    protected string contentName;
    protected GestureState buttonState;
    protected int ID;
    protected string sourceName;
    public ITouchManager uiManager;
    public float Opacity = 1f;
    public Alignment ObjectAlignment = Alignment.Center;
    public Color color;
    public Rectangle destination;
    public bool useRectangleScalar;
    public SpriteEffects Facing;
    public Rectangle SourceRect;
    public object MetaData;

    public GestureState ButtonState => this.buttonState;

    public int WidgetID
    {
      get => this.ID;
      set => this.ID = value;
    }

    public virtual Rectangle Bounds
    {
      get
      {
        return this.Pivot == Vector3.Zero ? new Rectangle((int) this.Position.X, (int) this.Position.Y, this.SourceRect.Width, this.SourceRect.Height) : new Rectangle((int) ((double) this.Position.X - (double) this.Pivot.X * (double) this.Scale), (int) ((double) this.Position.Y - (double) this.Pivot.Y * (double) this.Scale), this.SourceRect.Width, this.SourceRect.Height);
      }
    }

    public override float Width => (float) this.SourceRect.Width * this.Scale;

    public override float Height => (float) this.SourceRect.Height * this.Scale;

    public Widget(string sourcename, Vector2 position, string imageName, GameScreen screen)
      : base(sourcename, position, screen)
    {
      this.Direction = position.ToVector3();
      this.Facing = SpriteEffects.None;
      this.contentName = imageName;
      this.color = Color.White;
      this.sourceName = sourcename;
      this.Initialize();
    }

    public Widget(string sourcename, Vector2 position, GameScreen screen)
      : base(sourcename, position, screen)
    {
      this.Direction = position.ToVector3();
      this.Facing = SpriteEffects.None;
      this.color = Color.White;
      this.sourceName = sourcename;
    }

    public override void Initialize()
    {
      if (!string.IsNullOrEmpty(this.contentName))
      {
        this.tex = this.Screen.textureManager.Load(this.contentName);
      }
      else
      {
        this.tex = new Texture2D(Engine.gdm.GraphicsDevice, 1, 1);
        this.tex.SetData<Color>(new Color[1]{ Color.White });
      }
      this.SourceRect = new Rectangle(0, 0, this.tex.Width, this.tex.Height);
      if (!(this.Pivot == Vector3.Zero))
        return;
      this.Pivot = new Vector3(this.Width / 2f, this.Height / 2f, 0.0f);
    }

    public void SetAlignment(Alignment alignment)
    {
      if (alignment == Alignment.Left)
      {
        this.Pivot = Vector3.Zero;
      }
      else
      {
        if (alignment != Alignment.Center)
          return;
        this.Pivot = new Vector3(this.Width / 2f, this.Height / 2f, 0.0f);
      }
    }

    public void SetTextureSource(string name)
    {
      this.Name = name;
      this.sourceName = name;
      this.SetSourceRect();
    }

    public override void SetSourceRect()
    {
      if (this.tex != null)
      {
        this.SourceRect = this.Screen.GetSpriteSource(this.sourceName);
        if (this.SourceRect == Rectangle.Empty)
          this.SourceRect = new Rectangle(0, 0, this.tex.Width, this.tex.Height);
        if (this.Pivot != Vector3.Zero)
          this.Pivot = new Vector3((float) this.SourceRect.Width / 2f, (float) this.SourceRect.Height / 2f, 0.0f);
      }
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.SetSourceRect();
    }

    public void Enable(bool b)
    {
      if (b)
        this.buttonState = GestureState.Idle;
      else
        this.buttonState = GestureState.Disabled;
    }

    public virtual bool HandleTouch(GestureSample gesture, TouchInput touchInput) => false;

    public override void Draw() => base.Draw();
  }
}
