// Decompiled with JetBrains decompiler
// Type: GameEngine.Sprite3DV2
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class Sprite3DV2 : SceneNode
  {
    protected BasicEffect basicEffect;
    protected Texture2D tex;
    protected string contentName;
    public Rectangle SourceRect;
    public bool useBillboard = true;
    public Color Color = Color.White;
    public float Opacity = 1f;
    public SpriteEffects Facing;

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

    public Sprite3DV2(string sourcename, Vector3 position, string imageName, GameScreen screen)
      : base(sourcename, position, screen)
    {
      this.contentName = imageName;
      this.Color = Color.White;
      this.basicEffect = new BasicEffect(Engine.game.GraphicsDevice)
      {
        TextureEnabled = true,
        VertexColorEnabled = true
      };
      this.contentName = imageName;
      this.Initialize();
    }

    public override void Initialize()
    {
      this.tex = this.Screen.textureManager.Load<Texture2D>(this.contentName);
      this.SourceRect = new Rectangle(0, 0, this.tex.Width, this.tex.Height);
      if (!(this.Pivot == Vector3.Zero))
        return;
      this.Pivot = new Vector3(this.Width / 2f, this.Height / 2f, 0.0f);
    }

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

    public override void Draw(ICamera camera)
    {
      if (!this.Visible)
        return;
      Matrix scale = Matrix.CreateScale(1f, -1f, 1f);
      if (this.useBillboard)
      {
        this.basicEffect.World = Matrix.Identity;
        this.basicEffect.View = Matrix.Identity;
        this.basicEffect.Projection = camera.ProjectionMatrix;
        this.Screen.spriteBatch.Begin(SpriteSortMode.Deferred, (BlendState) null, (SamplerState) null, (DepthStencilState) null, RasterizerState.CullNone, (Effect) this.basicEffect);
        Vector3 vector3 = Vector3.Transform(this.Position, camera.ViewMatrix * scale);
        this.Screen.spriteBatch.Draw(this.tex, new Vector2(vector3.X, -vector3.Y), new Rectangle?(this.SourceRect), this.Color * this.Opacity, this.Rotation, this.Pivot.ToVector2(), this.Scale, this.Facing, vector3.Z);
        this.Screen.spriteBatch.End();
      }
      else
      {
        this.basicEffect.World = Matrix.CreateConstrainedBillboard(new Vector3(0.0f, this.Position.Y, 0.0f), new Vector3(0.0f, this.Position.Y + 1f, 0.0f), Vector3.Right, new Vector3?(), new Vector3?());
        this.basicEffect.View = camera.ViewMatrix;
        this.basicEffect.Projection = camera.ProjectionMatrix;
        this.Screen.spriteBatch.Begin(SpriteSortMode.Deferred, (BlendState) null, (SamplerState) null, (DepthStencilState) null, RasterizerState.CullNone, (Effect) this.basicEffect);
        this.Screen.spriteBatch.Draw(this.tex, new Vector2(-this.Position.Z, this.Position.X), new Rectangle?(this.SourceRect), this.Color * this.Opacity, this.Rotation, this.Pivot.ToVector2(), this.Scale, this.Facing, 0.0f);
        this.Screen.spriteBatch.End();
      }
      base.Draw(camera);
    }
  }
}
