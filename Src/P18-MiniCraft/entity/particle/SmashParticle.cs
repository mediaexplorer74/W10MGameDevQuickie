// Decompiled with JetBrains decompiler
// Type: GameManager.entity.particle.SmashParticle


using GameManager.gfx;
using GameManager.sound;
using Microsoft.Xna.Framework;
using System.IO;

#nullable disable
namespace GameManager.entity.particle
{
  public class SmashParticle : Entity
  {
    private int time;

    public SmashParticle(int x, int y)
    {
      this.X = x;
      this.Y = y;
      Sound.MonsterHurt.Play();
    }

    public SmashParticle(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.time);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.time = reader.ReadInt32();
    }

    public override void Tick()
    {
      ++this.time;
      if (this.time <= 10)
        return;
      this.Remove();
    }

    public override void Render(Screen screen)
    {
      int colors = gfx.Color.Get(-1, 555, 555, 555);
      screen.Render(this.X - 8, this.Y - 8, 389, colors, 2);
      screen.Render(this.X, this.Y - 8, 389, colors, 3);
      screen.Render(this.X - 8, this.Y, 389, colors, 0);
      screen.Render(this.X, this.Y, 389, colors, 1);
    }
  }
}
