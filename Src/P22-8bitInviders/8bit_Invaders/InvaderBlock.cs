// Decompiled with JetBrains decompiler
// Type: GameManager.InvaderBlock
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public class InvaderBlock : SceneNode
  {
    private Sprite[] rock;
    private Rectangle block;
    private bool destroyed;

    public InvaderBlock(Vector2 where, GameScreen screen)
      : base(where, screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.block = new Rectangle((int) this.Position.X - 30, (int) this.Position.Y - 30, 60, 60);
      this.rock = new Sprite[9];
      for (int index = 0; index < 9; ++index)
      {
        this.rock[index] = new Sprite("rock" + Tools.RandomInt(1, 4).ToString(), Vector2.Zero, "sheets/invaders", this.Screen);
        this.Add((SceneNode) this.rock[index]);
      }
      this.SetSourceRect();
    }

    public void Rebuild()
    {
      this.destroyed = false;
      float x = this.Position.X - 30f;
      float y = this.Position.Y - 30f;
      for (int index = 0; index < 9; ++index)
      {
        this.rock[index].Visible = true;
        this.rock[index].Position = new Vector3(x, y, 0.0f) + Tools.RandomV3(-5f, 5f);
        this.rock[index].SetTextureSource("rock" + Tools.RandomInt(1, 4).ToString());
        this.rock[index].Scale = Tools.RandomFloat(0.6f, 0.8f);
        this.rock[index].Rotation = Tools.RandomFloat(0.0f, 360f);
        x += 20f;
        if ((double) x == (double) this.Position.X + 30.0)
        {
          x = this.Position.X - 30f;
          y += 20f;
        }
      }
    }

    public bool BlockHitCheck(Rectangle where, out Vector2 position)
    {
      if (!this.destroyed && this.block.Intersects(where))
      {
        this.destroyed = true;
        for (int index = 0; index < 9; ++index)
          this.rock[index].Visible = false;
        position = this.block.CenterPoint();
        return true;
      }
      position = Vector2.Zero;
      return false;
    }

    public bool HitCheck(Vector2 where)
    {
      for (int index = 0; index < 9; ++index)
      {
        if (this.rock[index].Visible && this.rock[index].Bounds.Contains(where))
        {
          this.rock[index].Visible = false;
          return true;
        }
      }
      return false;
    }

    public bool isEmpty()
    {
      for (int index = 0; index < 9; ++index)
      {
        if (this.rock[index].Visible)
          return false;
      }
      return true;
    }
  }
}
