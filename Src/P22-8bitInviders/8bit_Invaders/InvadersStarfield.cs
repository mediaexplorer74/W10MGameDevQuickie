// Decompiled with JetBrains decompiler
// Type: GameManager.InvadersStarfield
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
  public class InvadersStarfield : Layer2D
  {
    private Sprite starfieldBG;
    private Sprite starfieldBG2;
    private Sprite starfield1;
    private Sprite starfield2;
    private Sprite universe1;
    private Sprite universe2;
    private Vector3 targetColor;
    private float R;
    private float G;
    private float B;

    public InvadersStarfield(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.blendState = BlendState.Additive;
      string imageName = "sheets/invaders";
      this.universe1 = new Sprite("starfield3", new Vector2(240f, 400f), imageName, this.Screen);
      this.Add((SceneNode) this.universe1);
      this.universe2 = new Sprite("starfield3", new Vector2(240f, -400f), imageName, this.Screen);
      this.Add((SceneNode) this.universe2);
      this.starfieldBG = new Sprite("starfield2", new Vector2(240f, 400f), imageName, this.Screen)
      {
        Opacity = 0.5f
      };
      this.Add((SceneNode) this.starfieldBG);
      this.starfieldBG2 = new Sprite("starfield2", new Vector2(240f, -400f), imageName, this.Screen)
      {
        Opacity = 0.5f
      };
      this.Add((SceneNode) this.starfieldBG2);
      this.starfield1 = new Sprite("starfield", new Vector2(240f, 400f), imageName, this.Screen);
      this.starfield1.Opacity = 0.6f;
      this.Add((SceneNode) this.starfield1);
      this.starfield2 = new Sprite("starfield", new Vector2(240f, -400f), imageName, this.Screen);
      this.starfield2.Opacity = 0.6f;
      this.Add((SceneNode) this.starfield2);
      this.SetSourceRect();
      this.R = 1f;
      this.G = 1f;
      this.B = 1f;
      this.targetColor = Tools.RandomV3(0.0f, 1f);
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.universe1.Position.Y += totalSeconds * 100f;
      if ((double) this.universe1.Position.Y > 1200.0)
        this.universe1.Position.Y = -400f;
      this.universe2.Position.Y += totalSeconds * 100f;
      if ((double) this.universe2.Position.Y > 1200.0)
        this.universe2.Position.Y = -400f;
      this.starfieldBG.Position.Y += totalSeconds * 200f;
      if ((double) this.starfieldBG.Position.Y > 1200.0)
        this.starfieldBG.Position.Y = -400f;
      this.starfieldBG2.Position.Y += totalSeconds * 200f;
      if ((double) this.starfieldBG2.Position.Y > 1200.0)
        this.starfieldBG2.Position.Y = -400f;
      this.starfield1.Position.Y += totalSeconds * 400f;
      if ((double) this.starfield1.Position.Y > 1200.0)
        this.starfield1.Position.Y = -400f;
      this.starfield2.Position.Y += totalSeconds * 400f;
      if ((double) this.starfield2.Position.Y > 1200.0)
        this.starfield2.Position.Y = -400f;
      this.R = (double) this.R >= (double) this.targetColor.X ? Math.Max(this.R - totalSeconds * 0.25f, this.targetColor.X) : Math.Min(this.R + totalSeconds * 0.25f, this.targetColor.X);
      this.G = (double) this.G >= (double) this.targetColor.Y ? Math.Max(this.G - totalSeconds * 0.25f, this.targetColor.Y) : Math.Min(this.G + totalSeconds * 0.25f, this.targetColor.Y);
      this.B = (double) this.B >= (double) this.targetColor.Z ? Math.Max(this.B - totalSeconds * 0.25f, this.targetColor.Z) : Math.Min(this.B + totalSeconds * 0.25f, this.targetColor.Z);
      this.universe1.color = new Color(this.R, this.G, this.B);
      this.universe2.color = this.universe1.color;
      if ((double) this.R == (double) this.targetColor.X && (double) this.G == (double) this.targetColor.Y && (double) this.B == (double) this.targetColor.Z)
        this.targetColor = Tools.RandomV3(0.0f, 1f);
      base.Update(gameTime, ref worldTransform);
    }
  }
}
