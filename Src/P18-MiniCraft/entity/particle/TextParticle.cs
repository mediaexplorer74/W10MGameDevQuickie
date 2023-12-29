// Decompiled with JetBrains decompiler
// Type: GameManager.entity.particle.TextParticle


using GameManager.gfx;
using Microsoft.Xna.Framework;
using System.IO;

#nullable disable
namespace GameManager.entity.particle
{
  public class TextParticle : Entity
  {
    public double Xa;
    public double Ya;
    public double Za;
    public double Xx;
    public double Yy;
    public double Zz;
    private string msg;
    private int col;
    private int time;

    public TextParticle(string msg, int x, int y, int col)
    {
      this.msg = msg;
      this.X = x;
      this.Y = y;
      this.col = col;
      this.Xx = (double) x;
      this.Yy = (double) y;
      this.Zz = 2.0;
      this.Xa = (this.random.NextDouble() * 2.0 - 1.0) * 0.3;
      this.Ya = (this.random.NextDouble() * 2.0 - 1.0) * 0.2;
      this.Za = this.random.NextDouble() * 0.7 + 2.0;
    }

    public TextParticle(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.msg);
      writer.Write(this.col);
      writer.Write(this.time);
      writer.Write(this.Xa);
      writer.Write(this.Ya);
      writer.Write(this.Za);
      writer.Write(this.Xx);
      writer.Write(this.Yy);
      writer.Write(this.Zz);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.msg = reader.ReadString();
      this.col = reader.ReadInt32();
      this.time = reader.ReadInt32();
      this.Xa = reader.ReadDouble();
      this.Ya = reader.ReadDouble();
      this.Za = reader.ReadDouble();
      this.Xx = reader.ReadDouble();
      this.Yy = reader.ReadDouble();
      this.Zz = reader.ReadDouble();
    }

    public override void Tick()
    {
      ++this.time;
      if (this.time > 60)
        this.Remove();
      this.Xx += this.Xa;
      this.Yy += this.Ya;
      this.Zz += this.Za;
      if (this.Zz < 0.0)
      {
        this.Zz = 0.0;
        this.Za *= -0.5;
        this.Xa *= 0.6;
        this.Ya *= 0.6;
      }
      this.Za -= 0.15;
      this.X = (int) this.Xx;
      this.Y = (int) this.Yy;
    }

    public override void Render(Screen screen)
    {
      Font.Draw(this.msg, screen, this.X - this.msg.Length * 4 + 1, this.Y - (int) this.Zz + 1, 
          gfx.Color.Get(-1, 0, 0, 0));
      Font.Draw(this.msg, screen, this.X - this.msg.Length * 4, this.Y - (int) this.Zz, this.col);
    }
  }
}
