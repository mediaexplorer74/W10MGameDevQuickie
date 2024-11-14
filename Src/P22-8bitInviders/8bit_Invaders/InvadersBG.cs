// Decompiled with JetBrains decompiler
// Type: GameManager.InvadersBG
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public class InvadersBG : SceneNode
  {
    private Sprite planet;
    private float planetTimer;
    private float planetThreshold;

    public InvadersBG(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.planet = new Sprite("planet", new Vector2(240f, -150f), "sheets/invaders", this.Screen);
      this.Add((SceneNode) this.planet);
      this.SetSourceRect();
      this.planetThreshold = Tools.RandomFloat(5f, 10f);
      this.planet.Position = new Vector3(Tools.RandomFloat(0.0f, 480f), -150f, 0.0f);
      this.planet.color = new Color(Tools.RandomV3(0.0f, 1f));
      this.planet.Scale = Tools.RandomFloat(0.35f, 1f);
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.planet.Position.Y > 950.0)
        this.planetTimer += totalSeconds;
      if ((double) this.planetTimer > (double) this.planetThreshold)
      {
        this.planetThreshold = Tools.RandomFloat(2f, 5f);
        this.planetTimer = 0.0f;
        this.planet.Position = new Vector3(Tools.RandomFloat(0.0f, 480f), -150f, 0.0f);
        this.planet.color = new Color(Tools.RandomV3(0.0f, 1f));
        this.planet.Scale = Tools.RandomFloat(0.35f, 1f);
      }
      this.planet.Position.Y += totalSeconds * 150f;
      base.Update(gameTime, ref worldTransform);
    }
  }
}
