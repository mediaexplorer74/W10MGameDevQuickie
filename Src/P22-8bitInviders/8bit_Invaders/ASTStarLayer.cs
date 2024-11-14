// Decompiled with JetBrains decompiler
// Type: GameManager.ASTStarLayer
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager
{
  public class ASTStarLayer : Layer2D
  {
    public ASTStarLayer(Camera2D cam, GameScreen screen)
      : base(cam, screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.samplerState = SamplerState.LinearWrap;
      this.blendState = BlendState.Additive;
      this.Add((SceneNode) new SpriteTile("startile", new Vector2(0.0f, 0.0f), new Rectangle(0, 0, 5000, 5000), "sheets/startile", this.Screen));
      this.SetSourceRect();
    }
  }
}
