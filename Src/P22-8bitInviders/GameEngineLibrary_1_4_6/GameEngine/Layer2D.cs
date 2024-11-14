// Decompiled with JetBrains decompiler
// Type: GameEngine.Layer2D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameEngine
{
  public class Layer2D : SceneNode
  {
    public BlendState blendState = BlendState.AlphaBlend;
    public SamplerState samplerState = SamplerState.LinearClamp;
    public Camera2D Camera;
    public Vector2 NonUniformScale = Vector2.One;
    public SceneNode FocusTarget;
    public SpriteSortMode sortMode;

    public Layer2D(Camera2D cam, GameScreen screen)
      : base("layer2D", screen)
    {
      this.Camera = cam;
      if (Engine.Orientation == GameOrientation.Portrait)
        cam.Position = new Vector2(240f, 400f);
      else
        cam.Position = new Vector2(400f, 240f);
    }

    public Layer2D(GameScreen screen)
      : base("layer2D", screen)
    {
      if (Engine.Orientation == GameOrientation.Portrait)
        this.Camera = new Camera2D()
        {
          Position = new Vector2(240f, 400f)
        };
      else
        this.Camera = new Camera2D()
        {
          Position = new Vector2(400f, 240f)
        };
    }

    public Layer2D(string name, Camera2D cam, GameScreen screen)
      : base(name, screen)
    {
      this.Camera = cam;
      if (Engine.Orientation == GameOrientation.Portrait)
        cam.Position = new Vector2(240f, 400f);
      else
        cam.Position = new Vector2(400f, 240f);
    }

    public Layer2D(string name, GameScreen screen)
      : base(name, screen)
    {
      if (Engine.Orientation == GameOrientation.Portrait)
        this.Camera = new Camera2D()
        {
          Position = new Vector2(240f, 400f)
        };
      else
        this.Camera = new Camera2D()
        {
          Position = new Vector2(400f, 240f)
        };
    }

    public override void Draw()
    {
      this.Screen.spriteBatch.Begin(this.sortMode, this.blendState, this.samplerState, (DepthStencilState) null, (RasterizerState) null, (Effect) null, this.Camera.LocalTransform);
      base.Draw();
      this.Screen.spriteBatch.End();
    }
  }
}
