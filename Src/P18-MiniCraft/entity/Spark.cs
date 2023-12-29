// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Spark


using GameManager.gfx;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Spark : Entity
  {
    public double Xa;
    public double Ya;
    public double Xx;
    public double Yy;
    private int lifeTime;
    private int time;
    private AirWizard owner;

    public Spark(AirWizard owner, double xa, double ya)
    {
      this.owner = owner;
      this.Xx = (double) (this.X = owner.X);
      this.Yy = (double) (this.Y = owner.Y);
      this.Xr = 0;
      this.Yr = 0;
      this.Xa = xa;
      this.Ya = ya;
      this.lifeTime = 600 + this.random.Next(30);
    }

    public Spark(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
      this.owner = game.ActiveAirWizard;
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.Xa);
      writer.Write(this.Ya);
      writer.Write(this.Xx);
      writer.Write(this.Yy);
      writer.Write(this.time);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.Xa = reader.ReadDouble();
      this.Ya = reader.ReadDouble();
      this.Xx = reader.ReadDouble();
      this.Yy = reader.ReadDouble();
      this.time = reader.ReadInt32();
    }

    public override void Tick()
    {
      ++this.time;
      if (this.time >= this.lifeTime)
      {
        this.Remove();
      }
      else
      {
        this.Xx += this.Xa;
        this.Yy += this.Ya;
        this.X = (int) this.Xx;
        this.Y = (int) this.Yy;
        List<Entity> entities = this.MyLevel.GetEntities(this.X, this.Y, this.X, this.Y);
        for (int index = 0; index < entities.Count; ++index)
        {
          Entity entity = entities[index];
          if (entity is Mob && !(entity is AirWizard))
            entity.Hurt((Mob) this.owner, 1, ((Mob) entity).Dir ^ 1);
        }
      }
    }

    public override bool IsBlockableBy(Mob mob) => false;

    public override void Render(Screen screen)
    {
      if (this.time >= this.lifeTime - 120 && this.time / 6 % 2 == 0)
        return;
      int num1 = 8;
      int num2 = 13;

      screen.Render(this.X - 4, this.Y - 4 - 2, num1 + num2 * 32,
          gfx.Color.Get(-1, 555, 555, 555), this.random.Next(4));

      screen.Render(this.X - 4, this.Y - 4 + 2, num1 + num2 * 32,
          gfx.Color.Get(-1, 0, 0, 0), this.random.Next(4));
    }
  }
}
